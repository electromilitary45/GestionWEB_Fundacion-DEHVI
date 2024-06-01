﻿using Fundacion_Dehvi.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;

namespace Fundacion_Dehvi.Models
{
    public class UsuarioModel
    {
        //----- INSTANCIAS -----
        private readonly string urlAPI = ConfigurationManager.AppSettings["urlApi"];

        /*----------------------ADMINISTRACION DE USUARIOS--------------------------*/

        //---INICIO: RESGISTRAR USUARIO---
        public UsuarioEnt registrarUsuario(UsuarioEnt usuario)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "CrearUsuario";
                JsonContent cont = JsonContent.Create(usuario);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<UsuarioEnt>().Result;
            }
        }

        //---INICIO: LISTAR USUARIOS---
        public List<UsuarioEnt> ListaCompletaUsuarios()
        {
            using(var client = new HttpClient())
            {
                string url = urlAPI + "ListaUsuarios";
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<UsuarioEnt>>().Result;
            }
        }

        //---INICIO: CONSULTAR USUARIO POR ID---
        public UsuarioEnt ConsultaUsuariosID(long q)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ConsultarUsuarioID?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<UsuarioEnt>().Result;
            }
        }


        /*----------------------USUARIOS COMUNES--------------------------*/

        //---INICIO: INICIAR SESION---
        public UsuarioEnt InicioSesion(UsuarioEnt usuario)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "InicioSesion";
                JsonContent cont = JsonContent.Create(usuario);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<UsuarioEnt>().Result;
            }
        }

        //---INICIO: RECUPERAR CONTRASENA---
        public UsuarioEnt RecuperarContrasena(UsuarioEnt usuario)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "RecuperacionContrasena";
                JsonContent cont = JsonContent.Create(usuario);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<UsuarioEnt>().Result;
            }
        }

    }//fin de la clase
}//fin del namespace