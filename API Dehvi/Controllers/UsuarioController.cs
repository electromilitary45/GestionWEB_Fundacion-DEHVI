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
        Utililitarios_Ent util = new Utililitarios_Ent();

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

        //---Inicio: Obtener Usuario por ID---
        [HttpGet]
        [Route("Usuario/ConsultaUsuarioID")]
        public Usuario ConsultaUsuarioID(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var data = (from u in con.Usuario
                                where u.idUsuario == q
                                select u).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //---Inicio: ConsultaUsuarioCedula---
        [HttpGet]
        [Route("Usuario/ConsultaUsuarioCedula")]
        public Usuario ConsultaUsuarioCedula(string q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var data = (from u in con.Usuario
                                where u.cedulaFisica == q
                                select u).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //---Inicio: CrearUsuario---
        [HttpPost]
        [Route("Usuario/CrearUsuario")]
        public int RegistroUsuario(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    if (ConsultaUsuarioCedula(usuario.cedulaFisica) != null)
                    {
                        return 1; //Ya existe un usuario con esa cedula
                    }
                    else
                    {
                        //----INICIO: Creacion de usuario----
                        var user = new Usuario
                        {
                            cedulaFisica = usuario.cedulaFisica,
                            nombre = usuario.nombre,
                            apellido1 = usuario.apellido1,
                            apellido2 = usuario.apellido2,

                            correo = usuario.correo,
                            contrasena = util.encrpytar(usuario.cedulaFisica), //Contraseña por defecto es la cedula (encriptada)

                            idDepartamento = usuario.idDepartamento,
                            idRol = Convert.ToByte(usuario.idRol),
                            estado = true,
                            fechaCreacion = DateTime.Now,
                            rutaImg = null
                        };
                        con.Usuario.Add(user);
                        con.SaveChanges();
                        //----FIN: Creacion de usuario----

                        //----INICIO: Envio de correo----
                        //TODO
                        //----FIN: Envio de correo----

                        return 2; //Usuario creado correctamente
                    }
                }

            }
            catch (Exception)
            {
                return 0; //Error al crear el usuario
            }
        }



    }//fin de la clase
}//fin del namespace

