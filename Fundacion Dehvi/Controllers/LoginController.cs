using Fundacion_Dehvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class LoginController : Controller
    {
        DepartamentoModel d = new DepartamentoModel();
        //-------------------------------------------------------------------
        public ActionResult Index()
        {
            var datos = d.ListaDeparta();
            return View(datos);
        }
    }
}
