using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Configuration;

namespace LadowebservisMVC.Util
{
    // App_Data must be writable by the application identity; keep it outside publicly served folders.
    public static class MarketingConsentStore
    {
        private static readonly object Sync = new object();
        private static string DirectoryPath { get { return HttpContext.Current.Server.MapPath("~/App_Data/MarketingConsent"); } }
        private static string Key(string email)
        {
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLowerInvariant()))).Replace("-", "");
        }
        private static string PathFor(string email) { return Path.Combine(DirectoryPath, Key(email) + ".txt"); }
        private static string Secret
        {
            get
            {
                var secret = ConfigurationManager.AppSettings["MarketingTokenSecret"];
                if (string.IsNullOrWhiteSpace(secret) || secret.Length < 32)
                    throw new InvalidOperationException("Set MarketingTokenSecret (at least 32 random characters) in Web.config.");
                return secret;
            }
        }
        public static string Token(string email, string purpose)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret)))
                return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(purpose + "|" + email.Trim().ToLowerInvariant()))).Replace("-", "").ToLowerInvariant();
        }
        public static bool VerifyToken(string email, string purpose, string supplied)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(supplied)) return false;
            var expected = Token(email, purpose);
            var a = Encoding.ASCII.GetBytes(expected);
            var b = Encoding.ASCII.GetBytes(supplied.Trim().ToLowerInvariant());
            if (a.Length != b.Length) return false;
            var diff = 0;
            for (var i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
        public static void SetStatus(string email, bool subscribed, string source)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required");
            lock (Sync)
            {
                Directory.CreateDirectory(DirectoryPath);
                var path = PathFor(email);
                var temp = path + ".tmp";
                File.WriteAllText(temp, (subscribed ? "subscribed" : "unsubscribed") + "\n" + DateTime.UtcNow.ToString("o") + "\n" + source, Encoding.UTF8);
                if (File.Exists(path)) File.Delete(path);
                File.Move(temp, path);
            }
        }
        public static bool IsSubscribed(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            lock (Sync)
            {
                var path = PathFor(email);
                return File.Exists(path) && File.ReadAllText(path, Encoding.UTF8).StartsWith("subscribed\n", StringComparison.Ordinal);
            }
        }
    }
}
