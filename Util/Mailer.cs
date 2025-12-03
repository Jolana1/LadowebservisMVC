using LadowebservisMVC.Controllers.Models;
using LadowebservisMVC.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LadowebservisMVC.Util
{
    
    public class Mailer
    {
        // Accepts optional attachment (HttpPostedFileBase) from MVC form file input
        public void OdoslanieSpravy(ContactModel_Sk model, HttpPostedFileBase attachment = null)
        {
            // Block known spammer patterns (case-insensitive) - reject any containing 'loori'
            if (model != null)
            {
                var bannedFragments = new[] { "loori" };
                var email = (model.Email ?? string.Empty);
                var name = (model.Name ?? string.Empty);
                if (bannedFragments.Any(b =>
                        (!string.IsNullOrEmpty(email) && email.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(name) && name.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    // silently ignore/send no mail for blocked senders
                    return;
                }
            }

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("info@ladowebservis.sk", "ladowebservis.sk");
                mail.Headers["X-Mailer"] = "https://ladowebservis.sk/zdravie";
                mail.Subject = "Ďakujeme! Nech sa páči E-book je v prílohe";
                mail.IsBodyHtml = false;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;

                // determine filename to display in body
                string attachedFileName = string.Empty;
                if (attachment != null && attachment.ContentLength > 0)
                {
                    attachedFileName = attachment.FileName;
                }
                else if (model.File != null && model.File.ContentLength > 0)
                {
                    attachedFileName = model.File.FileName;
                }

                // Build body with correct placeholders (admin notification)
                mail.Body = string.Format(
                    "\r\n Ďakujeme, že ste nás kontaktovali.\r\n Vaše meno: {0} ,\r\n Váš email: {1} ,\r\n Potvrdenie hesla: {2}," +
                    "\r\n Priložený súbor:Päť_prirodzených_ciest_k_väčšej_energii od_Zinzina {4}" +
                    "\r\n Vaša správa: {5}," +
                    "\r\n Posielam Vám e‑book od Ladowebservis zdarma.\r\n Prečítajte si ho prosím a dozviete sa viac ako dostať svoje telo späť do rovnováhy zdravia.\r\n\r\nS pozdravom,\nTím ladowebservis.sk",
                    model.Name,
                    model.Email,
                    model.Password,
                    model.File,
                    attachedFileName,
                    model.Text);
                mail.BodyEncoding = Encoding.UTF8;

                // Only send to valid non-empty recipient
                var recipientIsValid = false;
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    try
                    {
                        var addr = new MailAddress(model.Email);
                        mail.To.Add(model.Email);
                        recipientIsValid = true;
                    }
                    catch
                    {
                        // invalid email, do not attempt send to user
                        mail.To.Add("info@ladowebservis.sk");
                    }
                }
                else
                {
                    mail.To.Add("info@ladowebservis.sk");
                }

                mail.Bcc.Add("info@ladowebservis.sk");

                // Attach file from parameter if provided, otherwise from model.File if present
                if (attachment != null && attachment.ContentLength > 0)
                {
                    var mailAttachment = new Attachment(attachment.InputStream, attachment.FileName, attachment.ContentType);
                    mail.Attachments.Add(mailAttachment);
                }
                else if (model.File != null && model.File.ContentLength > 0)
                {
                    var mailAttachment = new Attachment(model.File.InputStream, model.File.FileName, model.File.ContentType);
                    mail.Attachments.Add(mailAttachment);
                }

                // Also attach a bundled PDF (App_Data/MailAttachment.pdf) if it exists
                try
                {
                    var context = HttpContext.Current;
                    if (context != null)
                    {
                        var pdfPath = context.Server.MapPath("~/App_Data/MailAttachment.pdf");
                        if (File.Exists(pdfPath))
                        {
                            var pdfAttachment = new Attachment(pdfPath);
                            mail.Attachments.Add(pdfAttachment);
                        }
                    }
                }
                catch
                {
                    // ignore failures attaching the bundled PDF
                }

                using (var client = new SmtpClient("email.active24.com"))
                {
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("info@ladowebservis.sk", "a98HdiBMYNRH");


                    client.Send(mail);
                }

                // Send a friendly HTML confirmation to the customer with CTA and forwarding option
                if (recipientIsValid)
                {
                    try
                    {
                        var customerEmail = model.Email.Trim();
                        var customerName = string.IsNullOrWhiteSpace(model.Name) ? "" : HttpUtility.HtmlEncode(model.Name);
                        using (var conf = new MailMessage())
                        {
                            conf.From = new MailAddress("info@ladowebservis.sk", "ladowebservis.sk");
                            conf.To.Add(customerEmail);
                            conf.Subject = "Ďakujeme! Špeciálna ponuka";
                            conf.SubjectEncoding = Encoding.UTF8;
                            conf.BodyEncoding = Encoding.UTF8;
                            conf.IsBodyHtml = false;

                            var productsLink = "https://ladowebservis.sk/Home/Produkty";
                            // plain text confirmation body
                            var bodyText = new StringBuilder();
                            bodyText.AppendLine($"Dobrý deň {customerName},");
                            bodyText.AppendLine();
                            bodyText.AppendLine("Ďakujeme, že ste nás kontaktovali. Ako malú pomoc sme pre Vás pripravili špeciálnu ponuku:");
                            bodyText.AppendLine();
                            bodyText.AppendLine("Špeciálna ponuka: Zľava 10% pri okamžitej objednávke.");
                            bodyText.AppendLine();
                            bodyText.AppendLine("Pozrite si naše odporúčané produkty:");
                            bodyText.AppendLine(productsLink);
                            bodyText.AppendLine();
                            
                            bodyText.AppendLine();
                            bodyText.AppendLine("Ak máte otázky, odpíšte priamo na tento e‑mail a radi Vám pomôžeme.");
                            bodyText.AppendLine();
                            bodyText.AppendLine("S pozdravom,\nTím ladowebservis.sk");

                            conf.Body = bodyText.ToString();
                            conf.BodyEncoding = Encoding.UTF8;

                            using (var client2 = new SmtpClient("email.active24.com"))
                            {
                                client2.EnableSsl = true;
                                client2.Port = 587;
                                client2.UseDefaultCredentials = false;
                                client2.Credentials = new NetworkCredential("info@ladowebservis.sk", "a98HdiBMYNRH");
                                client2.Send(conf);
                            }
                        }
                    }
                    catch
                    {
                        // ignore confirmation send errors
                    }
                }
            }
        }

        // Accepts optional attachment (HttpPostedFileBase) from MVC form file input
        public void OdoslanieEmailu(RegisterModel model, HttpPostedFileBase attachment = null)
        {
            // Block known spammer patterns (case-insensitive) - reject any containing 'loori'
            if (model != null)
            {
                var bannedFragments = new[] { "loori" };
                var email = (model.Email ?? string.Empty);
                var name = (model.Name ?? string.Empty);
                if (bannedFragments.Any(b =>
                        (!string.IsNullOrEmpty(email) && email.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(name) && name.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    return;
                }
            }

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("info@ladowebservis.sk", "ladowebservis.sk");
                mail.Headers["X-Mailer"] = "ladowebservis.sk";
                mail.Subject = "Ďakujeme za Vašu registráciu.";
                mail.IsBodyHtml = false;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;

                mail.Body = string.Format("\r\n Ďakujeme, že ste sa u nás zaregistrovali.Ako zaregistrovaný zákazník okrem iného získavate následovné výhody:" +
                    "\r\n Rýchlejší nákup vďaka vlastnému zoznamu obľúbených položiek." +
                    "\r\n Možnosť byť informovaný o novinkách a akciách." +
                    "\r\n Prístup do členskej sekcie a zľavového programu." + "\r\n\r\n" +
                    "\r\n Váš email: {0}" +
                    "\r\n Vaše meno: {1}" +
                    "\r\n Priezvisko: {2}," +
                    "\r\n Telefón: {3}" +
                    "\r\n Adresa: {4}" +
                    "\r\n Vaše mesto: {5}" +
                    "\r\n Potvrdenie emailu-(Captcha): {6}" +
                    "\r\n Vaše heslo: {7}" +
                    "\r\n Váš text:\r\n {8}" +
                    "\r\n\r\n Prajeme príjemný deň. \r\n\r\n S pozdravom ladowebservis.sk",
                    model.Email,
                    model.Name,
                    model.Priezvisko,
                    model.Phone,
                    model.Adresa,
                    model.City,
                    model.Captcha,
                    model.Password,
                    model.Text);

                mail.To.Add(model.Email);
                mail.Bcc.Add("info@ladowebservis.sk");

                if (attachment != null && attachment.ContentLength > 0)
                {
                    var mailAttachment = new Attachment(attachment.InputStream, attachment.FileName, attachment.ContentType);
                    mail.Attachments.Add(mailAttachment);
                }

                using (var client = new SmtpClient("email.active24.com"))
                {
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("info@ladowebservis.sk", "a98HdiBMYNRH");


                    client.Send(mail);
                }
            }
        }

        // Send a follow-up email containing product links to the specified recipient.
        // This can be called later to send product recommendations or reminders.
        public void SendProductsEmail(string customerEmail, string customerName = null, IEnumerable<string> cartProductIds = null)
        {
            if (string.IsNullOrWhiteSpace(customerEmail)) return;
            try
            {
                // validate email
                try { var _ = new MailAddress(customerEmail); } catch { return; }

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress("info@ladowebservis.sk", "Ladowebservis");
                    mail.To.Add(customerEmail);
                    mail.Bcc.Add("info@ladowebservis.sk");
                    mail.Subject = "Odporúčané produkty od Ladowebservis";
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = false; // send plain-text only

                    var nameSafe = string.IsNullOrWhiteSpace(customerName) ? "" : HttpUtility.HtmlEncode(customerName);

                    // Build plain-text body
                    var text = new StringBuilder();
                    text.AppendLine($"Dobrý deň {nameSafe},");
                    text.AppendLine();
                    text.AppendLine("Posielame výber odporúčaných produktov, ktoré by Vás mohli zaujímať:");
                    text.AppendLine();

                    try
                    {
                        var prodList = ProductCatalog.GetAll() ?? Enumerable.Empty<object>();

                        // If cartProductIds provided, list those first with more detail
                        if (cartProductIds != null && cartProductIds.Any())
                        {
                            text.AppendLine("Produkty, ktoré zostali vo Vašom košíku:");
                            foreach (var id in cartProductIds)
                            {
                                if (string.IsNullOrWhiteSpace(id)) continue;
                                dynamic found = null;
                                foreach (dynamic p in prodList)
                                {
                                    try {
                                        var pidTry = (p?.Id ?? string.Empty).ToString();
                                        if (pidTry.Equals(id, StringComparison.OrdinalIgnoreCase)) { found = p; break; }
                                    } catch { }
                                }
                                if (found != null)
                                {
                                    var pname = (found.Name ?? string.Empty).ToString();
                                    decimal pprice = 0m;
                                    try { pprice = Convert.ToDecimal(found.Price); } catch { decimal.TryParse((found.Price ?? "0").ToString(), out pprice); }
                                    string pdesc = string.Empty;
                                    try { pdesc = (found.Description ?? found.Text ?? found.Info ?? found.ShortDescription ?? string.Empty).ToString(); } catch { pdesc = string.Empty; }
                                    var productUrl = UrlForProduct((found.Id ?? string.Empty).ToString());

                                    text.AppendLine($"- {pname} — €{pprice:0.00}");
                                    if (!string.IsNullOrWhiteSpace(pdesc))
                                    {
                                        var shortDesc = pdesc.Length > 300 ? pdesc.Substring(0, 300) + "..." : pdesc;
                                        text.AppendLine("  Popis: " + shortDesc);
                                    }
                                    text.AppendLine("  Stránka produktu: " + productUrl);
                                    text.AppendLine();
                                }
                                else
                                {
                                    // fallback: include id and link
                                    text.AppendLine($"- Produkt ID: {id} (zobraziť: {UrlForProduct(id)})");
                                    text.AppendLine();
                                }
                            }

                            text.AppendLine();
                            text.AppendLine("Nižšie sú ďalšie odporúčané produkty:");
                            text.AppendLine();
                        }

                        // General product list (brief)
                        foreach (dynamic p in prodList)
                        {
                            if (p == null) continue;
                            var pid = (p.Id ?? string.Empty).ToString();
                            var pname = (p.Name ?? string.Empty).ToString();
                            decimal pprice = 0m;
                            try { pprice = Convert.ToDecimal(p.Price); } catch { decimal.TryParse((p.Price ?? "0").ToString(), out pprice); }
                            var link = UrlForProduct(pid);
                            text.AppendLine($"- {pname} — €{pprice:0.00} — {link}");
                        }
                    }
                    catch
                    {
                        text.AppendLine("Zobraziť naše produkty: https://ladowebservis.sk/Home/Produkty");
                    }

                    text.AppendLine();
                    text.AppendLine("Ak chcete, môžeme Vám produkty zaslať neskôr znova. Stačí odpovedať na tento e‑mail.");
                    text.AppendLine();
                    text.AppendLine("S pozdravom,\nTím Ladowebservis");

                    mail.Body = text.ToString();
                    mail.BodyEncoding = Encoding.UTF8;

                    // do not add HTML alternate view; send plain text only

                    using (var client = new SmtpClient("email.active24.com"))
                    {
                        client.EnableSsl = true;
                        client.Port = 587;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("info@ladowebservis.sk", "a98HdiBMYNRH");
                        client.Send(mail);
                    }
                }
            }
            catch
            {
                // ignore send errors
            }
        }

        // helper to build product URL (simple fallback)
        private static string UrlForProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return "https://ladowebservis.sk/Home/Produkty";
            // attempt to link to a product details route
            return "https://ladowebservis.sk/Home/Produkt?id=" + HttpUtility.UrlEncode(productId);
        }
    }
}


































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































