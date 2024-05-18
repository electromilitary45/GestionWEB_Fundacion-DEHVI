using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

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
    }
}