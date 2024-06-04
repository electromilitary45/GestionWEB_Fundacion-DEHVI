using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fundacion_Dehvi.Entities
{
    public class ProcedimientosEnt
    {
        public long idProcedimiento { get; set; }
        public string codProcedimiento { get; set; }

        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string objetivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool estado { get; set; }


        //------------------------ FK Departamento
        public long idDepartamento { get; set; }
        public string nombreDeparta { get; set; }

        //------------------------ FK Usuarios
        public long idUsuario { get; set; }
        public string nombreUsuario { get; set; }
    }
}