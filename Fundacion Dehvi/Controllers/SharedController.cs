using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundacion_Dehvi.Controllers
{
    public class SharedController : Controller
    {
        [HttpGet]
        public ActionResult AccesoNoAuthorizado() { 
            
            return View();
        }
    }
}