using Fundacion_Dehvi.Entities;
using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class DepartamentoController : Controller
    {
        DepartamentoModel d = new DepartamentoModel();

        //------------------- Crear Departamento -------------------
        [HttpGet]
        public ActionResult CrearDepartamento()
        {
            if (Session["idusuario"] != null)
            {
                return RedirectToAction("InicioSesion, Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CrearDepartamento(DepartamentoEnt depa)
        {
            try
            {
                var resp = d.CrearDepartamento(depa);

                if (resp == 1)
                {
                    TempData["Mensaje"] = "El Departamento fue registrada exitosamente";
                    return RedirectToAction("ListaDeparta");

                }
                else
                {
                    ViewBag.mensaje = "¡Lo sentimos! Se produjo un error al crear departamento";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        }
        //------------------- Lista de Departamentos -------------------
        [HttpGet]
        public ActionResult ListaDeparta()
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    var datos = d.ListaDeparta();
                    //Muestra los mensajes referentes a empresas agregadas correctamente y al cambio de estado de estas cuando sea pertinente
                    ViewBag.Mensaje = TempData["Mensaje"];
                    return View(datos);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServer", "Error");
            }

        }

        //------------------- Perfil Departamento -------------------
        [HttpGet]
        public ActionResult PerfilDepa(long q)
        {

            var datos = d.PerfilDepa(q);
            if (Session["mensaje"] != null)
            {
                ViewBag.MensajeAcc = Session["mensaje"].ToString();
            }

            return View(datos); // muestra el perfil del accesorio en especifico
        }

        //------------------- Actualizar Departamento -------------------
        [HttpGet]
        public ActionResult ActualizarDepa(long q)
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("InicioSesion", "Login");
            }
            else
            {
                var datos = d.PerfilDepa(q);
                return View(datos);
            }
        }

        [HttpPost]
        public ActionResult ActualizarDepa(DepartamentoEnt entidad)
        {
            try
            {
                var resp = d.ActualizarDepa(entidad);

                if (resp == 1)
                {
                    TempData["Mensaje"] = "El departamento fue actualizada exitosamente";
                    return RedirectToAction("ListaDeparta", "Departamento");
                }
                else
                {
                    ViewBag.Mensaje = "¡Lo sentimos! No se pudo actualizar el departamento";
                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServer", "Error");
            }
        }
        //------------------- Estado Departamento -------------------
        [HttpGet]
        public ActionResult EstadoDepartamento(long q)
        {
            var entidad = new DepartamentoEnt();
            entidad.idDepartamento = q;

            var resp = d.EstadoDepartamento(entidad);

            switch (resp)
            {
                case 1:
                    TempData["Mensaje"] = "Se ha inactivado el departamento correctamente";
                    return RedirectToAction("ListaDeparta", "Departamento");
                case 2:
                    TempData["Mensaje"] = "Se ha activado el departamento correctamente";
                    return RedirectToAction("ListaDeparta", "Departamento");
                default:
                    TempData["Mensaje"] = "¡Lo sentimos! No se pudo actualizar el estado del departamento";
                    return RedirectToAction("ListaDeparta", "Departamento");
            }

        }

    }
}