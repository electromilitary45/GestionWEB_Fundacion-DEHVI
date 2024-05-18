using API_Dehvi.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Dehvi.Controllers
{
    public class UsuarioController : ApiController
    {
        //---Instancias---

        //-------------------- ADMISITRACION DE USUARIOS --------------------

        //----Inicio: Obtener Usuarios----
        [HttpGet]
        [Route("Usuario/ListaUsuarios")]
        public List<Usuario_Ent> ListaUsuarios()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var data = (
                        from u in con.Usuario
                        select new Usuario_Ent
                        {
                            idUsuario = u.idUsuario,
                            nombre = u.nombre,
                            apellido1 = u.apellido1,
                            apellido2 = u.apellido2,
                            correo = u.correo,
                            contrasena = u.contrasena,
                            idRol = u.idRol,
                            estado = u.estado,
                            fechaCreacion = u.fechaCreacion,
                            rutaImg = u.rutaImg
                        }).ToList();

                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
