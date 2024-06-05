using API_Dehvi.Entities;
using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class UsuarioController : Controller
    {
        //----------INSTANCIAS----------
        private readonly UsuarioModel UM = new UsuarioModel();
        private readonly UtililitariosEnt Util = new UtililitariosEnt();

        //------------------ADMINISTRACION DE USUARIOS------------------







        /*----------------------USUARIO COMUN-------------------------------*/


        //-----Perfil de Usuario
        [HttpGet]
        public ActionResult PerfilUsuario()
        {
            if (Session["idUsuario"] != null)
            {
                long idUsuario = long.Parse(Session["idUsuario"].ToString());
                var datos = UM.ConsultaUsuariosID(idUsuario);
                return View(datos);
            }
            else
            {
                return RedirectToAction("InicioSesion", "Login");
            }

        }//fin perfil usuario

        //-----INICIO: Cambio de contraseña
        [HttpGet]
        public ActionResult CambioContrasena()
        {
            if (Session["idUsuario"] != null)
            {
                var datos = UM.ConsultaUsuariosID(long.Parse(Session["idUsuario"].ToString()));
                return View(datos);
            }
            else
            {
                return RedirectToAction("InicioSesion", "Login");
            }

        }//fin cambio contraseña

        [HttpPost]
        public ActionResult CambioContrasena(long idUsuario, string contrasenaActual, string contrasenaNueva, string contrasenaRepetida)
        {
            var datos = UM.ConsultaUsuariosID(long.Parse(Session["idUsuario"].ToString()));
            try
            {
                if (idUsuario == 0 || contrasenaActual.Equals("") || contrasenaNueva.Equals("") || contrasenaRepetida.Equals(""))
                {
                    return View(datos);
                }
                else
                {
                    UsuarioEnt user = new UsuarioEnt
                    {
                        idUsuario = idUsuario,
                        contrasenaActual = Util.encrpytar(contrasenaActual),
                        contrasenaNueva = Util.encrpytar(contrasenaNueva),
                        contrasenaRepetida = Util.encrpytar(contrasenaRepetida)
                    };

                    var resp = UM.CambiarContrasena(user);

                    switch (resp)
                    {
                        case 1:
                            return RedirectToAction("PerfilUsuario");
                        case 3:
                            ViewBag.mensaje = "Contraseña actual erronea";
                            return View(datos);
                        case 2:
                            ViewBag.mensaje = "La contraseña nueva y contraseña repetida no son iguales";
                            return View(datos);
                        default:
                            ViewBag.mensaje = "¡Se produjo error al cambiar la costraseña!";
                            return View(datos);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

        }


    }//fin de clase
}//fin de namespace