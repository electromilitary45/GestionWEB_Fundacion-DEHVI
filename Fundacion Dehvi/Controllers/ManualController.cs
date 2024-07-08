using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class ManualController : Controller
    {
        //----INSTANCIA
        private readonly ManualModel MM = new ManualModel();
        private readonly DepartamentoModel DM = new DepartamentoModel();



        /*----------------------------ADMINISTRACION---------------------------*/
        //--- INICIO: Perfil de Manual para un administrador
        [HttpGet]
        public ActionResult PerfilManualAdmin(long q)
        {

            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            var idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1 && idRol != 2)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }

            var datos = MM.ConsultaManualId(q);
            var datos2 = MM.ListaDocManualxManual(q);
            var model = new Tuple<Manual_Ent, IEnumerable<DocManual_Ent>>(datos, datos2);

            return View(model);
        }

        [HttpGet]
        public ActionResult ActualizarEstadoManual(long q)
        {
            var entidad = new Manual_Ent();
            entidad.idManual = q;

            var resp = MM.EstadoManual(entidad);

            switch (resp)
            {
                case 1:
                    TempData["mensaje"] = "Se ha desactivado el Manual.";
                    return RedirectToAction("PerfilManualAdmin", "Manual", new { q });
                case 2:
                    TempData["mensaje"] = "Se ha activado el Manual.";
                    return RedirectToAction("PerfilManualAdmin", "Manual", new { q });
                default:
                    TempData["mensaje"] = "Manual no encontrado.";
                    return RedirectToAction("PerfilManualAdmin", "Manual", new { q });

            }


        }

        /*--- INICIO: NuevaVersion de DocManual */
        [HttpPost]
        public ActionResult NuevaVersionDocManual(HttpPostedFileBase inputDoc, long idProcedimiento, long idManual, long idUsuarioCreador)
        {
            try
            {
                if (inputDoc != null || idProcedimiento != 0 || idManual != 0 || idUsuarioCreador != 0)
                {
                    DocManual_Ent docManual = new DocManual_Ent
                    {
                        idManual = idManual,
                        idUsuarioCreador = idUsuarioCreador,
                        fechaCreacion = DateTime.Now
                    };

                    long respDocManual = MM.CrearDocManual(docManual);
                    //----INICIO
                    //Se guarda al extension del archivo temporalmente
                    string extension = Path.GetExtension(Path.GetFileName(inputDoc.FileName));

                    /*
                     * Se comprueba que la carpeta donde se guardan los archivos
                     * 
                     */
                    string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos/" + idProcedimiento + "/" + idManual);

                    if (!Directory.Exists(directorio))
                    {
                        Directory.CreateDirectory(directorio);
                    }

                    //se crea la ruta donde se guarda el archivo y se guarda el archivo
                    string ruta = Path.Combine(directorio, respDocManual + extension);
                    inputDoc.SaveAs(ruta);

                    var rutaDoc = "/Archivos/" + idProcedimiento + "/" + idManual + "/" + respDocManual + extension;
                    docManual.idDocManual = respDocManual;
                    docManual.ruta = rutaDoc;

                    //actualizar ruta en la base de datos
                    MM.ActRutaDocManual(docManual);
                    MM.DesactivarDocManuales(idManual, respDocManual);

                    TempData["mensaje"] = "Nueva version del manual agregada";
                    return RedirectToAction("PerfilManualAdmin", "Manual", new { q = idManual });

                }

                TempData["mensaje"] = "Debe rellenar los espacios requeridos";

                return RedirectToAction("PerfilManualAdmin", "Manual", new { q = idManual });
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        //--- INICIO: EDITAR MANUAL
        [HttpGet]
        public ActionResult EditarManual(long q)
        {
            
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            byte idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1 && idRol != 2)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }

            var datos = MM.ConsultaManualId(q);
            return View(datos);
        }
        [HttpPost]
        public ActionResult EditarManual(Manual_Ent manual)
        {
            try
            {
                if (manual.nombre == null || manual.codReferencia == null)
                {
                    TempData["mensaje"] = "Debe llenar los espacios requeridos";
                    return RedirectToAction("EditarManual", new { q = manual.idManual });
                }

                MM.ActualizarManual(manual);

                TempData["mensaje"] = "Manual actualizado con exito";
                return RedirectToAction("PerfilManualAdmin", new { q = manual.idManual });
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        //---- INICIO: Agregar Manual
        [HttpGet]
        public ActionResult AgregarManual(long q)
        {
            
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            var idRol = byte.Parse(Session["idRol"].ToString());
            if (idRol != 1 && idRol != 2)
            {
                return RedirectToAction("AccesoNoAuthorizado", "Shared");
            }

            //---idProcedimiento
            ViewBag.idProcedimiento = q;
            ViewBag.mensaje = TempData["Mensaje"];

            return View();

        }

        [HttpPost]
        public ActionResult AgregarManual(HttpPostedFileBase inputDocManual, Manual_Ent manual)
        {
            try
            {
                if (inputDocManual == null || manual.nombre == null || manual.codReferencia == null)
                {
                    TempData["Mensaje"] = "Debe llenar los espacios requeridos!";
                    return RedirectToAction("AgregarManual", new { q = manual.idProcedimiento });
                }

                //-----INICIO: Se registra el manual
                manual.fechaCreacion = DateTime.Now;
                long respManual = MM.CrearManual(manual);

                if (respManual == 0)
                {
                    TempData["Mensaje"] = "Error al crear un nuevo manual1!";
                    return RedirectToAction("AgregarManual", new { q = manual.idProcedimiento });
                }

                //----INICIO: Se registra el documento
                DocManual_Ent docManual = new DocManual_Ent
                {
                    idManual = respManual,
                    idUsuarioCreador = manual.idUsuarioCreador,
                    fechaCreacion = manual.fechaCreacion
                };
                long respDocManual = MM.CrearDocManual(docManual);

                if (respDocManual == 0)
                {
                    TempData["Mensaje"] = "Error al crear un nuevo manual2!";
                    return RedirectToAction("AgregarManual", new { q = manual.idProcedimiento });
                }

                //----INICIO
                //Se guarda al extension del archivo temporalmente
                string extension = Path.GetExtension(Path.GetFileName(inputDocManual.FileName));

                /*
                 * Se comprueba que la carpeta donde se guardan los archivos
                 * 
                 */
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos/" + manual.idProcedimiento + "/" + respManual);

                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                //se crea la ruta donde se guarda el archivo y se guarda el archivo
                string ruta = Path.Combine(directorio, respDocManual + extension);
                inputDocManual.SaveAs(ruta);

                var rutaDoc = "/Archivos/" + manual.idProcedimiento + "/" + respManual + "/" + respDocManual + extension;
                docManual.idDocManual = respDocManual;
                docManual.ruta = rutaDoc;

                //actualizar ruta en la base de datos
                MM.ActRutaDocManual(docManual);

                ////desactivar los docManuales desactualizados
                //MM.DesactivarDocManuales(respManual,respDocManual);

                return RedirectToAction("PerfilProce", "Procedimiento", new { q = manual.idProcedimiento });
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }


        /*---------------------------- USUARIOS Manuales ---------------------------*/

        //---- INICIO:Perfil de manual para usuario
        [HttpGet]
        public ActionResult PerfilManual(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }



            var datos = MM.ConsultaManualId(q);
            var datos2 = MM.ListaDocManualxManual2(q);
            ViewBag.idProcedimiento = datos.idProcedimiento;
            var model = new Tuple<Manual_Ent, IEnumerable<DocManual_Ent>>(datos, datos2);
            ViewBag.mensaje = TempData["mensaje"];
            return View(model);
        }

        //------------------- Lista Manuales Completa -------------------
        [HttpGet]
        public ActionResult ConsultaManuales()
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    var datos = MM.ConsultaManuales();
                    var datos2 = DM.ListaDeparta();

                    var model = new Tuple<IEnumerable<Manual_Ent>, IEnumerable<DepartamentoEnt>>(datos, datos2);
                    //Muestra los mensajes referentes a manales agregados correctamente y al cambio de estado de estas cuando sea pertinente
                    ViewBag.Mensaje = TempData["Mensaje"];
                    return View(model);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        }// fin de la lista de los manuales


    }//fin class
}//fin namespace