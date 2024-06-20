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




        /*----------------------------ADMINISTRACION---------------------------*/
        //--- INICIO: Perfil de Manual para un administrador
        [HttpGet]
        public ActionResult PerfilManualAdmin(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            var datos = MM.ConsultaManualId(q);
            var datos2 = MM.ListaDocManualxManual(q);

            var model = new Tuple<Manual_Ent, IEnumerable<DocManual_Ent>>(datos, datos2);

            return View(model);
        }

        /*----------------------------USUARIOS COMUNES---------------------------*/

        //---- INICIO: Agregar Manual
        [HttpGet]
        public ActionResult AgregarManual(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
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

        //---- INICIO:Perfil de manual para usuario
        [HttpGet]
        public ActionResult PerfilManual(long q, long a)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }

            ViewBag.idProcedimiento = a;

            var datos = MM.ConsultaManualId(q);
            var datos2 = MM.ListaDocManualxManual2(q);

            var model = new Tuple<Manual_Ent, IEnumerable<DocManual_Ent>>(datos, datos2);

            return View(model);
        }



    }//fin class
}//fin namespace