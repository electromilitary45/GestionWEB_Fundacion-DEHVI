using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.WebPages;

namespace API_Dehvi.Entities
{
    public class Utililitarios_Ent
    {
        //---------Metodo para generar una contraseña aleatoria---------
        public string randomPassword()
        {
            int length = 8;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%&*?";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }//fin de metodo randomPassword

        //---------Metodo para encryptar una cadena de texto---------
        public string encrpytar(string cadena)
        {
            byte[] cadenaBytes = System.Text.Encoding.UTF8.GetBytes(cadena); //Convierte la cadena a bytes
            using (SHA256 sha256 = SHA256.Create()) //Crea una instancia de SHA256
            {
                byte[] hashBytes = sha256.ComputeHash(cadenaBytes); //Calcula el hash de la cadena
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); //Convierte el hash a string y lo retorna
            }
        }//fin de metodo encrpytar

        //---------Metodo para enviar un correo de manera generica---------
        public void enviarCorreo(string correo, string asunto, string contenido)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["Correo"]);
            message.To.Add(new MailAddress(correo));
            message.Subject = asunto;
            message.IsBodyHtml = true;
            message.Body = contenido;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Puerto"]);
            smtp.Host = ConfigurationManager.AppSettings["Host"];
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Correo"], ConfigurationManager.AppSettings["Contrasena"]);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}