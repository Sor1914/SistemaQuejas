using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Net;


namespace FE_QUEJAS.Services
{
    public class clsEnvioEmail
    {
        public bool enviarCorreo(string[] correos, string asunto, string html)
        {
            string username = "jsorm@miumg.edu.gt";
            string password = "p(ZX(-^vd<";

            // Configuración del cliente SMTP
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, password);

            // Creación del mensaje de correo electrónico
            MailMessage mailMessage = new MailMessage();            
            mailMessage.From = new MailAddress(username);
            foreach (string correo in correos)
            {
                mailMessage.To.Add(correo);
            }
            mailMessage.Subject =asunto;
            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            // Envío del mensaje
            client.Send(mailMessage);
            return true;
        }
    }
}