using API_Dehvi.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
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
        [Route("ListaUsuarios")]
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
                            idDepartamento = u.idDepartamento,
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
        [Route("ConsultaUsuarioID")]
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
        [Route("ConsultaUsuarioCedula")]
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
        [Route("CrearUsuario")]
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

        //--Inicio: DesactivarUsuario---
        [HttpDelete]
        [Route("DesactivarUsuario")]
        public int DesactivarUsuario(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == q
                                select u).FirstOrDefault();

                    user.estado = false;
                    con.SaveChanges();
                    return 1; //Usuario desactivado correctamente
                }
            }
            catch (Exception)
            {
                return 0; //Error al desactivar el usuario
            }
        }

        //--Inicio: ActivarUsuario---
        [HttpPut]
        [Route("ActivarUsuario")]
        public int ActivarUsuario(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == q
                                select u).FirstOrDefault();

                    user.estado = true;
                    con.SaveChanges();
                    return 1; //Usuario activado correctamente
                }
            }
            catch (Exception)
            {
                return 0; //Error al activar el usuario
            }
        }


        /*-------------------- FIN ADMISITRACION DE USUARIOS --------------------*/


        //--- INICIO: USUARIOS COMUN -
        [HttpPost]
        [Route("InicioSesion")]
        public Usuario IniciarSesion(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    /*
                     * Busco el usuario por correo y contraseña
                     * Encrpto la contraseña para compararla con la de la base de datos
                     * Verifico que el usuario este activo
                    */
                    var user = (from u in con.Usuario
                                where u.correo == usuario.correo && u.contrasena == util.encrpytar(usuario.contrasena) && u.estado == true
                                select u).FirstOrDefault();

                    return user;
                }
            }
            catch (Exception)
            {
                return null; //error con la base de datos
            }
        }

        //--- INICIO: Recuperar Correo ---
        [HttpPost]
        [Route("RecuperacionCorreo")]
        public int RecuperarCorreoUsuario(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    /*
                     * Este metodo funciona para recuperar el correo de un usuario
                     * mediante la cedula fisica del mismo
                     * vericando que el usuario este activo
                     */
                    var user = (
                        from u in con.Usuario
                        where u.cedulaFisica == usuario.cedulaFisica && u.estado == true
                        select u).FirstOrDefault();


                    /*
                     * Si el usuario existe se envia un correo con la informacion
                     */
                    if (user != null && user.estado == true)
                    {
                        /*
                         * TODO: Aqui se enviar el correo con el correo del usuario encontrado
                         */

                        /*
                         * TODO: Crear Templeate de correo
                         *                         
                         */
                        return 1;
                    }
                    else if (user != null & user.estado == false)
                    {
                        return 2; //Usuario desactivado
                    }
                    else
                    {
                        return 3; //Usuario no encontrado
                    }

                }
            }
            catch (Exception)
            {
                return 500;
            }
        }

        //--- INICIO: Recuperar Contraseña ---
        [HttpPost]
        [Route("RecuperacionContrasena")]
        public int RecuperarContrasenaUsuario(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    /*
                     * Este metodo funciona para recuperar la contraseña de un usuario por medio del correo
                     * Verificando que el usuario este activo
                     */

                    var user = (from u in con.Usuario
                                where u.correo == usuario.correo && u.estado == true
                                select u).FirstOrDefault();

                    if (user != null)
                    {
                        /*
                         * Se genera una nueva contraseña para el usuario
                         */
                        var nuevaContrasena = util.randomPassword();
                        var contrasena = util.encrpytar(nuevaContrasena);

                        /*
                         * Se actualiza la contraseña del usuario
                         */
                        user.contrasena = contrasena;
                        con.SaveChanges();

                        //TODO: Enviar correo con la contraseña

                        return 1;
                    }
                    else
                    {
                        return 2; //Usuario no encontrado
                    }
                }
            }
            catch (Exception)
            {
                return 500;
            }
        }

    }//fin de la clase
}//fin del namespace

