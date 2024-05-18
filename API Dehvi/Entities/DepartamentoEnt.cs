using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Dehvi.Entities
{
    public class DepartamentoEnt
    {
        public long idDepartamento { get; set; }
        public string nombre { get; set; }
        public bool estado { get; set; }
    }
}