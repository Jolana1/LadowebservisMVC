using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace LadowebservisMVC.Util
{
    public class MailAttachement
    {
        /// <summary>
        /// Default mail attachement path
        /// </summary>
        public static string DefaultPath = "\\App_Data\\MailAttachement";

        /// <summary>
        /// Gets the attachement file full path name
        /// </summary>
        /// <param name="filePath">Attachement file directory</param>
        /// <param name="fileName">Attachement file name</param>
        /// <returns>Returns file full path name</returns>
        public static string GetAttachementPath(string filePath, string fileName)
        {
            string fileFullName = string.Format("{0}\\{1}",
                string.IsNullOrEmpty(filePath) ? HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath) + MailAttachement.DefaultPath : filePath,
                fileName);


            return fileFullName;
        }

        /// <summary>
        /// Returns an Attachment object for the given file name located in App_Data (or null if missing).
        /// Default file name is "MailAttachment.pdf".
        /// </summary>
        /// <param name="fileName">File name (example: "MailAttachment.pdf")</param>
        /// <returns>Attachment or null</returns>
        public static Attachment GetDefaultAttachment(string fileName = "MailAttachment.pdf")
        {
            try
            {
                // Prefer App_Data root where the sample file exists (adjust if your files are stored under a subfolder)
                var virtualPath = string.Format("~/App_Data/{0}", fileName);
                var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                if (string.IsNullOrEmpty(physicalPath) || !File.Exists(physicalPath))
                {
                    return null;
                }

                var attachment = new Attachment(physicalPath, MediaTypeNames.Application.Pdf)
                {
                    Name = fileName
                };

                return attachment;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Attaches the default file (or provided fileName) to the provided MailMessage if present.
        /// </summary>
        /// <param name="mail">MailMessage to attach to</param>
        /// <param name="fileName">Optional file name under App_Data (default: "MailAttachment.pdf")</param>
        public static void AddDefaultAttachmentToMail(MailMessage mail, string fileName = "MailAttachment.pdf")
        {
            if (mail == null) return;

            var att = GetDefaultAttachment(fileName);
            if (att != null)
            {
                mail.Attachments.Add(att);
            }
        }
    }
}