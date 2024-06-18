using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Dehvi.Entities
{
    public class DocManual_Ent
    {
        //------ Atributos de Tabla ------
        public long idDocManual { get; set; }
        public long idUsuarioCreador { get; set; }
        public long idManual { get; set; }
        public long versionDoc {  get; set; }
        public string ruta { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool estado { get; set; }

        //----- Atributos Genericos ------
        public string nombreUsuario { get; set; }

    }
}