using API_Dehvi.Entities;
using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class UsuarioController : Controller
    {
        //----------INSTANCIAS----------
        private readonly UsuarioModel UM = new UsuarioModel();
        private readonly ManualModel MM = new ManualModel();
        private readonly DepartamentoModel DM = new DepartamentoModel();
        private readonly UtililitariosEnt Util = new UtililitariosEnt();

        //------------------ADMINISTRACION DE USUARIOS------------------

        //----- INICIO: Lista Usuarios Admin
        [HttpGet]
        public ActionResult ListaUsuariosAdmin()
        {

            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            var idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }

            var datos = UM.ListaCompletaUsuarios();
            return View(datos);

        }

        //----- INICIO: Activar y desactivar Usuarios
        [HttpGet]
        public ActionResult ActivarUsuario(long q)
        {
            var resp = UM.ActUsuario(q);

            if (resp != 0)
            {
                return RedirectToAction("ListaUsuariosAdmin", "Usuario");
            }
            else
            {
                return RedirectToAction("ListaUsuariosAdmin", "Usuario");
            }
        }

        [HttpGet]
        public ActionResult DesactivarUsuario(long q)
        {
            var resp = UM.InacUsuario(q);

            if (resp != 0)
            {
                return RedirectToAction("ListaUsuariosAdmin", "Usuario");
            }
            else
            {
                return RedirectToAction("ListaUsuariosAdmin", "Usuario");
            }
        }

        //----- INICIO: Nuevo Usuario Admin
        [HttpGet]
        public ActionResult NuevoUsuarioAdmin()
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            var idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }


            ViewBag.listaDepartamentos = DM.LItemDepartamento();
            ViewBag.listaRoles = UM.LItemRol();
            //ViewBag.listaDepartamentos 
            return View();


        }

        [HttpPost]
        public ActionResult NuevoUsuarioAdmin(UsuarioEnt usuario)
        {
            try
            {
                if (usuario.cedulaFisica != null && usuario.nombre != null && usuario.apellido1 != null && usuario.apellido2 != null && usuario.idDepartamento != 0 && usuario.idRol != 0)
                {
                    ViewBag.listaDepartamentos = DM.LItemDepartamento();
                    ViewBag.listaRoles = UM.LItemRol();
                    var user = UM.registrarUsuario(usuario);

                    switch (user)
                    {
                        case 1:
                            ViewBag.mensaje = "Ya existe un usuario con la cedula fisica digitada!";
                            return View();
                        case 2:
                            return RedirectToAction("ListaUsuariosAdmin", "Usuario");
                        default:
                            return View();
                    }
                }
                else
                {
                    ViewBag.mensaje = "Debe rellenar los espacios requeridos!";
                    return View();
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        //----- INICIO: Editar Usuario
        [HttpGet]
        public ActionResult EditarUsuarioAdmin(long q)
        {
            
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            var idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }



            ViewBag.listaDepartamentos = DM.LItemDepartamento();
            ViewBag.listaRoles = UM.LItemRol();
            var datos = UM.ConsultaUsuariosID(q);
            return View(datos);

        }

        [HttpPost]
        public ActionResult EditarUsuarioAdmin(UsuarioEnt usuario)
        {
            try
            {
                ViewBag.listaDepartamentos = DM.LItemDepartamento();
                ViewBag.listaRoles = UM.LItemRol();
                var user = UM.ActualizarUsuario(usuario);
                if (user != 500)
                {
                    ViewBag.mensaje = "Usuario editado con exito";
                    return RedirectToAction("ListaUsuariosAdmin", "Usuario");
                }

                ViewBag.mensaje = "Error al editar!";
                return View(UM.ConsultaUsuariosID(usuario.idUsuario));
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        /*----------------------USUARIO COMUN-------------------------------*/


        //-----Perfil de Usuario
        [HttpGet]
        public ActionResult PerfilUsuario()
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            long idUsuario = long.Parse(Session["idUsuario"].ToString());
            var datosU = UM.ConsultaUsuariosID(idUsuario);
            var datosM = MM.ConsultaManualesXUsuarios(idUsuario);
            return View(datosM);
        }//fin perfil usuario

        //-----INICIO: Cambio de contraseña
        [HttpGet]
        public ActionResult CambioContrasena()
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            var datos = UM.ConsultaUsuariosID(long.Parse(Session["idUsuario"].ToString()));
            return View(datos);


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

        //-----INICIO: EDITAR DATOS PERSONALES
        [HttpGet]
        public ActionResult EditarMisDatos()
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
        }

        [HttpPost]
        public ActionResult EditarMisDatos(UsuarioEnt usuario)
        {
            try
            {
                var user = UM.EditarMisDatos(usuario);
                switch (user)
                {
                    case 1:
                        return RedirectToAction("PerfilUsuario", "Usuario");
                    default:
                        ViewBag.mensaje = "Error al actualizar su informacion.";
                        return View();
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        //----INICIO: EDITAR MI FOTO
        [HttpGet]
        public ActionResult EditarMiAvatar(UsuarioEnt usuario)
        {
            if (Session["idUsuario"] != null)
            {
                return View(UM.ConsultaUsuariosID(long.Parse(Session["idUsuario"].ToString())));
            }
            return RedirectToAction("InicioSesion", "Login");
        }

        [HttpPost]
        public ActionResult SubirAvatar(HttpPostedFileBase inputArchivo, UsuarioEnt usuario)
        {
            try
            {
                if (inputArchivo == null && usuario.idUsuario == 0)
                {
                    ViewBag.mensaje = "¡Lo sentimos! Debe agregar una imagen!";
                    return RedirectToAction("EditarMiAvatar");
                }

                //se guardar la extension del archivo temporalmente
                string extension = Path.GetExtension(Path.GetFileName(inputArchivo.FileName));

                /*comprobar que la carpeta donde se guardan las cosas este creada*/
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserImages");

                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                //Se crea la ruta donde se guarda el archivo y se guarda el archivo
                string ruta = Path.Combine(directorio, usuario.idUsuario + extension);
                inputArchivo.SaveAs(ruta);

                //actualizar ruta en la base de datos
                usuario.rutaImg = "/UserImages/" + usuario.idUsuario + extension;

                UM.ActualizarImgUsuario(usuario);

                @Session["rutaImg"] = usuario.rutaImg;

                return RedirectToAction("EditarMiAvatar");

            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpPost]
        public ActionResult ActualizarAvatar(HttpPostedFileBase inputArchivo, UsuarioEnt usuario)
        {
            try
            {
                if (inputArchivo == null && usuario.idUsuario == 0)
                {
                    ViewBag.mensaje = "¡Lo sentimos! Debe agregar una imagen!";
                    return RedirectToAction("EditarMiAvatar");
                }

                //se guardar la extension del archivo temporalmente
                string extension = Path.GetExtension(Path.GetFileName(inputArchivo.FileName));

                /*comprobar que la carpeta donde se guardan las cosas este creada*/
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserImages");

                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                //Se crea la ruta donde se guarda el archivo y se guarda el archivo
                string ruta = Path.Combine(directorio, usuario.idUsuario + extension);

                // Eliminar la imagen existente si ya existe
                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }


                inputArchivo.SaveAs(ruta);

                //actualizar ruta en la base de datos
                usuario.rutaImg = "/UserImages/" + usuario.idUsuario + extension;

                UM.ActualizarImgUsuario(usuario);

                @Session["rutaImg"] = usuario.rutaImg;

                return RedirectToAction("EditarMiAvatar");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult EliminarAvatar(long q)
        {
            try
            {
                var usuarioDb = UM.ConsultaUsuariosID(q); // Método para obtener el usuario por ID
                string rutaImg = usuarioDb?.rutaImg;

                if (!string.IsNullOrEmpty(rutaImg))
                {
                    // Obtener la ruta completa del archivo
                    string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserImages");
                    string ruta = Path.Combine(directorio, Path.GetFileName(rutaImg));

                    // Eliminar la imagen existente si ya existe
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }

                // Llamada al método para eliminar la referencia del avatar en la base de datos
                UM.EliminarAvatar(usuarioDb);

                // Actualizar la sesión para reflejar que no hay avatar
                Session["rutaImg"] = null;
                return RedirectToAction("EditarMiAvatar");
            }
            catch (Exception e)
            {
                return View(e);
            }

        }

        //---- INICIO: Usuarios por Departamento
        [HttpGet]
        public ActionResult ListaUsuariosDep(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            var datos = UM.ListaUsuarioDep(q);

            return View(datos);
        }


    }//fin de clase
}//fin de namespace