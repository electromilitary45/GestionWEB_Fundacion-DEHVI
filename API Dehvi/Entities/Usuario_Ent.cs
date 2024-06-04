using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Dehvi.Entities
{
    public class Usuario_Ent
    {
        //----- Atributos en Tabla -----
        public long idUsuario { get; set; }

        public string cedulaFisica { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }

        public string correo { get; set; }
        public string contrasena { get; set; }

        public long idDepartamento { get; set; }
        public byte idRol { get; set; }
        public bool estado { get; set; }

        public DateTime fechaCreacion { get; set; }
        public string rutaImg { get; set; }

        //-------- Atributos genericos-------
        public string contrasenaNueva { get; set; }




    }
}