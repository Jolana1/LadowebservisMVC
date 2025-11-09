using LadowebservisMVC.Controllers.Models;
using LadowebservisMVC.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.IO;


namespace LadowebservisMVC.Util
{

    public class Mailer
    {
        // Accepts optional attachment (HttpPostedFileBase) from MVC form file input
        public void OdoslanieSpravy(ContactModel_Sk model, HttpPostedFileBase attachment = null)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("info@ladowebservis.sk", "ladowebservis.sk");
                mail.Headers["X-Mailer"] = "https://ladowebservis.sk/";
                mail.Subject = "Pozdravujeme Vás!";
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

                // Build body with correct placeholders
                mail.Body = string.Format(
                    "\r\n Ďakujeme, že ste nás kontaktovali.E‑book bol priložený k zaslanému e‑mailu\r\n Vaše meno: {0} ,\r\n Váš email: {1} ,\r\n Potvrdenie hesla: {2}," +
                    //"\r\n Váš Telefón: {3}" +
                    "\r\n Priložený súbor: 5_prirodzenych_ciest_k_vačšej_energii_Zinzino {4}" +
                    "\r\n Vaša správa: {5}," +
                    "\r\n\r\n Ďakujeme za prejavenú dôveru a správu. \r\n\r\n S pozdravom a úctou.",
                    model.Name,
                    model.Email,
                    model.Password,
                    model.File,
                    attachedFileName,
                    model.Text);
mail.BodyEncoding = Encoding.UTF8;

                mail.To.Add(model.Email);
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
            }
        }

        // Accepts optional attachment (HttpPostedFileBase) from MVC form file input
        public void OdoslanieEmailu(RegisterModel model, HttpPostedFileBase attachment = null)
        {
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
    }
}










