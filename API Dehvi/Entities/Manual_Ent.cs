using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Dehvi.Entities
{
    public class Manual_Ent
    {
        //----- Atributos Tabla ------

        public long idManual { get; set; }
        public long idProcedimiento { get; set; }
        public long idUsuarioCreador { get; set; }
        public DateTime fechaCreacion { get; set; }

        public string nombre { get; set; }
        public string codReferencia { get ; set; }
        public bool estado { get; set; }

        //----- Atributos genericos
        public string nombreUsuario { get; set; }
        public long idDepartamento { get; set; }

    }
}