using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class ProcedimientoController : Controller
    {
        //------------------- Instancias -------------------
        ProcedimientoModel p = new ProcedimientoModel();

        //------------------- Crear Procedimiento -------------------
        [HttpGet]
        public ActionResult CrearProce()
        {
            if (Session["idusuario"] != null)
            {
                return RedirectToAction("InicioSesion, Login");
            }
            else
            {
                return View();
            }
        } // fin del crear procedimiento

        [HttpPost]
        public ActionResult CrearProce(ProcedimientosEnt proce)
        {
            try
            {
                var resp = p.CrearProce(proce);

                if (resp == 1)
                {
                    TempData["Mensaje"] = "El Procedimiento fue registrado exitosamente";
                    return RedirectToAction("ListaProcesSector");

                }
                else
                {
                    ViewBag.mensaje = "¡Lo sentimos! Se produjo un error al crear el procedimiento";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        }// fin del post de crear procedimiento

        //------------------- Lista Procedimientos Completa -------------------
        [HttpGet]
        public ActionResult ListaProces()
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    var datos = p.ListaProces();
                    //Muestra los mensajes referentes a procedimientos agregados correctamente y al cambio de estado de estas cuando sea pertinente
                    ViewBag.Mensaje = TempData["Mensaje"];
                    return View(datos);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        }// fin de la lista de los procedimientos

        //------------------- Lista Procedimientos por Sector -------------------
        [HttpGet]
        public ActionResult ListaProcesSector(long q)
        {

            var datos = p.ListaProcesSector(q);
            if (Session["mensaje"] != null)
            {
                ViewBag.MensajeAcc = Session["mensaje"].ToString();
            }

            return View(datos); // muestra el listado de procedimientos por sector
        } // fin del get de lista de procedimientos por sector

        //------------------- Perfil Procedimiento -------------------
        [HttpGet]
        public ActionResult PerfilProce(long q)
        {

            var datos = p.PerfilProce(q);
            if (Session["mensaje"] != null)
            {
                ViewBag.MensajeAcc = Session["mensaje"].ToString();
            }

            return View(datos); // muestra el perfil del procedimiento en especifico
        }// fin del get del perfil

        //------------------- Actualizar Procedimiento -------------------
        [HttpGet]
        public ActionResult ActualizarProce(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            else
            {
                var datos = p.PerfilProce(q);
                return View(datos);
            }
        } // fin del get actualizar procedimiento

        [HttpPost]
        public ActionResult ActualizarProce(ProcedimientosEnt entidad)
        {
            try
            {
                var resp = p.ActualizarProce(entidad);

                if (resp == 1)
                {
                    TempData["Mensaje"] = "El procedimiento fue actualizada exitosamente";
                    return RedirectToAction("PerfilProce", "Procedimiento");
                }
                else
                {
                    ViewBag.Mensaje = "¡Lo sentimos! No se pudo actualizar el procedimiento";
                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        } // fin del post de actualizar procedimiento

        //------------------- Desactualizar Procedimiento -------------------
        [HttpGet]
        public ActionResult EstadoProce(long q)
        {
            var entidad = new ProcedimientosEnt();
            entidad.idProcedimiento = q;

            var resp = p.EstadoProce(entidad);

            switch (resp)
            {
                case 1:
                    TempData["Mensaje"] = "Se ha inactivado el procedimiento correctamente";
                    return RedirectToAction("ListaProcesSector", "Procedimiento");
                case 2:
                    TempData["Mensaje"] = "Se ha activado el procedimiento correctamente";
                    return RedirectToAction("ListaProcesSector", "Procedimiento");
                default:
                    TempData["Mensaje"] = "¡Lo sentimos! No se pudo actualizar el estado del procedimiento";
                    return RedirectToAction("ListaProcesSector", "Procedimiento");
            }

        }// fin del actualizar estado procedimiento

    }// fin del controlador
}// fin del namespace