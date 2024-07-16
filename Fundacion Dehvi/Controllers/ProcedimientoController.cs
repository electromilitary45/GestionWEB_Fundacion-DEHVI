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
        DepartamentoModel d = new DepartamentoModel();
        ManualModel m = new ManualModel();

        //------------------- Crear Procedimiento -------------------
        [HttpGet]
        public ActionResult CrearProce()
        {

            if (Session["idusuario"] == null)
            {
                return RedirectToAction("InicioSesion, Login");
            }
            else
            {
                ViewBag.idDepartamento = d.LItemDepartamento();
                return View();
            }
        } // fin del crear procedimiento

        [HttpPost]
        public ActionResult CrearProce(ProcedimientosEnt proce)
        {
            try
            {
                ViewBag.idDepartamento = d.LItemDepartamento();
                proce.fechaCreacion = DateTime.Now.Date;
                var resp = p.CrearProce(proce);

                
                if (resp == 1)
                {
                    TempData["Mensaje"] = "El Procedimiento fue registrado exitosamente";
                    return RedirectToAction("ListaProces");

                }else if(resp == 2)
                {
                    TempData["Mensaje"] = "Ya existe un procedimiento con ese codigo!";
                    return RedirectToAction("ListaProces");
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
                    var datos2 = d.ListaDeparta();

                    var model = new Tuple<IEnumerable<ProcedimientosEnt>, IEnumerable<DepartamentoEnt>>(datos, datos2);
                    //Muestra los mensajes referentes a procedimientos agregados correctamente y al cambio de estado de estas cuando sea pertinente
                    ViewBag.Mensaje = TempData["Mensaje"];
                    return View(model);
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
                ViewBag.MensajeProc = Session["mensaje"].ToString();
            }

            return View(datos); // muestra el listado de procedimientos por sector
        } // fin del get de lista de procedimientos por sector

        //------------------- Perfil Procedimiento -------------------
        [HttpGet]
        public ActionResult PerfilProce(long q)
        {

            var datos = p.PerfilProce(q);
            var datos2 = m.ListaManProcedimiento(q);

            var model = new Tuple<ProcedimientosEnt, IEnumerable<Manual_Ent>>(datos, (IEnumerable<Manual_Ent>)datos2);


            if (Session["mensaje"] != null)
            {
                ViewBag.MensajeProc = Session["mensaje"].ToString();
            }

            return View(model); // muestra el perfil del procedimiento en especifico
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
                ViewBag.idDepartamento = d.LItemDepartamento();
                var datos = p.ConsultaProce(q);
                return View(datos);
            }
        } // fin del get actualizar procedimiento

        [HttpPost]
        public ActionResult ActualizarProce(ProcedimientosEnt entidad)
        {
            try
            {
                ViewBag.idDepartamento = d.LItemDepartamento();
                var resp = p.ActualizarProce(entidad);

                if (resp == 1)
                {
                    TempData["Mensaje"] = "El procedimiento fue actualizada exitosamente";
                    return RedirectToAction("ListaProces", "Procedimiento");
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