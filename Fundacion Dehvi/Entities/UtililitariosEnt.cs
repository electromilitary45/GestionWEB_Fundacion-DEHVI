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
    public class UtililitariosEnt
    {
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