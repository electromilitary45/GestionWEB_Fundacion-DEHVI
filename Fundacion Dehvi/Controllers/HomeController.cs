using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class HomeController : Controller
    {
        //--- INSTANCIAS ---
        private readonly DepartamentoModel d = new DepartamentoModel();

        //--- INDEX SIN SESION ---
        [HttpGet]
        public ActionResult Index()
        {
            var datos = d.ListaDeparta();
            return View(datos);
        }
        //--- Navbar ---
        [ChildActionOnly]
        public PartialViewResult NavBar()
        {
            var departamentos = d.ListaDeparta();
            return PartialView("_NavBar", departamentos);
        }

    }
}