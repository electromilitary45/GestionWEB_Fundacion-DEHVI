using API_Dehvi.Entities;
using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class LoginController : Controller
    {
        //---INSTANCIAS---
        private readonly UsuarioModel UM = new UsuarioModel();
        private readonly UtililitariosEnt util = new UtililitariosEnt();

        //----INICIO: Inicio de Sesión----

        [HttpGet]
        public ActionResult InicioSesion()
        {
            ViewBag.mensaje = TempData["mensaje"];
            return View();
        }

        [HttpPost]
        public ActionResult InicioSesion(UsuarioEnt usuario)
        {
            try
            {
                if (!Regex.IsMatch(usuario.correo, @"^[a-zA-Z0-9._%+-]+@dehvi\.org$"))
                {
                    TempData["mensaje"] = "El correo electrónico debe pertenecer al dominio @dehvi.org";
                    return RedirectToAction("InicioSesion", "Login");
                }

                usuario.contrasena = util.encrpytar(usuario.contrasena);
                var user = UM.InicioSesion(usuario);

                if (user != null)
                {
                    Session["idUsuario"] = user.idUsuario;
                    Session["nombre"] = user.nombre;
                    Session["apellido1"] = user.apellido1;
                    Session["apellido2"] = user.apellido2;
                    Session["correo"] = user.correo;
                    Session["idRol"] = user.idRol;
                    Session["idDepartamento"] = user.idDepartamento;
                    Session["nomDepartamento"] = user.nombreDepartamento;
                    Session["rutaImg"] = user.rutaImg;
                    Session["fechaCreacion"]= user.fechaCreacion;

                    return RedirectToAction("Index", "Home");
                }

                TempData["mensaje"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("InicioSesion", "Login");


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //----INICIO: RECUPERAR CONTRASEÑA----
        [HttpGet]
        public ActionResult RecuperarContrasena()
        {
            ViewBag.mensaje = TempData["mensaje"];
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarContrasena(UsuarioEnt usuario)
        {
            try
            {
                // Validar que el correo electrónico pertenezca al dominio @dehvi.org
                if (!Regex.IsMatch(usuario.correo, @"^[a-zA-Z0-9._%+-]+@dehvi\.org$"))
                {
                    TempData["mensaje"] = "El correo electrónico debe pertenecer al dominio @dehvi.org";
                    return View();
                }

                var resp = UM.RecuperarContrasena(usuario);
                if (resp != null)
                {
                    TempData["mensaje"] = "Se ha enviado un correo con su nueva contraseña";
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    TempData["mensaje"] = "Correo/Cédula no registrado o encontrado";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Manejo del error
                TempData["mensaje"] = "Ocurrió un error al procesar la solicitud.";
                return View();
            }
        }

        //----INICIO: Cerrar sesion----
        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("InicioSesion", "Login");
        }

    }//Fin de la clase
}//Fin del namespace
