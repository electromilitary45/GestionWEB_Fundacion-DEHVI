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
    

        //------------------ADMINISTRACION DE USUARIOS------------------







        //------------------USUARIO COMUN-------------------------------
        [HttpGet]
        public ActionResult PerfilUsuario ()
        {
            if(Session["idUsuario"] != null)
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

    }//fin de clase
}//fin de namespace