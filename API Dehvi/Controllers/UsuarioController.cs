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
        private readonly Utililitarios_Ent util = new Utililitarios_Ent();

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
                        join d in con.Departamento on u.idDepartamento equals d.idDepartamento
                        select new Usuario_Ent
                        {
                            idUsuario = u.idUsuario,
                            cedulaFisica = u.cedulaFisica,
                            nombre = u.nombre,
                            apellido1 = u.apellido1,
                            apellido2 = u.apellido2,
                            correo = u.correo,
                            contrasena = u.contrasena,
                            idRol = u.idRol,
                            idDepartamento = u.idDepartamento,
                            nombreDepartamento = d.nombre,
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
                        usuario.contrasenaNueva = util.randomPassword();
                        //----INICIO: Creacion de usuario----
                        var user = new Usuario
                        {
                            cedulaFisica = usuario.cedulaFisica,
                            nombre = usuario.nombre,
                            apellido1 = usuario.apellido1,
                            apellido2 = usuario.apellido2,

                            correo = usuario.correo,
                            
                            contrasena = util.encrpytar(usuario.contrasenaNueva), //La contraseña nueva es totalmente nueva y ramdon

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
                        string urlHtml = AppDomain.CurrentDomain.BaseDirectory + "TemplatesCorreos\\NuevaCuenta.html";
                        string html = System.IO.File.ReadAllText(urlHtml);

                        html = html.Replace("@@nombre", usuario.nombre);
                        html = html.Replace("@@apellido1", usuario.apellido1);
                        html = html.Replace("@@apellido2", usuario.apellido2);
                        html = html.Replace("@@correo", usuario.correo);
                        html = html.Replace("@@contrasena", usuario.contrasenaNueva);
                        util.enviarCorreo(usuario.correo, "Nueva Cuenta DEHVI", html);
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
        [HttpPut]
        [Route("DesactivarUsuario")]
        public int DesactivarUsuario(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
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
        public int ActivarUsuario(Usuario_Ent usuario) 
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
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

        //--Inicio: ListaRoles
        [HttpGet]
        [Route("LRoles")]
        public List<System.Web.Mvc.SelectListItem> IListRoles()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var roles = (from r in con.Rol select r).ToList();

                    List<System.Web.Mvc.SelectListItem> listaRoles = new List<System.Web.Mvc.SelectListItem>();
                    var res = new List<System.Web.Mvc.SelectListItem>();

                    foreach (var role in roles)
                    {
                        res.Add(new System.Web.Mvc.SelectListItem
                        {
                            Value = role.idRol.ToString(),
                            Text = role.nombre
                        });
                    }
                    return res;

                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //---INICIO: Actualizar Usuario---
        [HttpPut]
        [Route("ActualizarUsuario")]
        public int ActualizarUsuario(Usuario_Ent usuario)
        {
            try
            {
                using(var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario 
                                where u.idUsuario == usuario.idUsuario
                                select u).FirstOrDefault();

                    user.nombre = usuario.nombre;
                    user.apellido1 = usuario.apellido1;
                    user.apellido2 = usuario.apellido2;
                    user.correo = usuario.correo;
                    user.idRol = usuario.idRol;
                    user.idDepartamento = usuario.idDepartamento;

                    con.SaveChanges();

                    return 1;
                }
            }
            catch (Exception)
            {
                return 500;
            }
        }


        /*-------------------- FIN ADMISITRACION DE USUARIOS --------------------*/

        /*-------------------- INICIO: USUARIOS COMUNES -------------------------*/

        //--- INICIO: USUARIOS COMUN -
        [HttpPost]
        [Route("InicioSesion")]
        public Usuario_Ent IniciarSesion(Usuario_Ent usuario)
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
                    
                    var user= (from u in con.Usuario
                               join d in con.Departamento on u.idDepartamento equals d.idDepartamento
                               where u.correo == usuario.correo && u.contrasena == usuario.contrasena && u.estado == true
                               select new Usuario_Ent
                               {
                                   idUsuario = u.idUsuario,
                                   cedulaFisica = u.cedulaFisica,
                                   correo = u.correo,
                                   nombre=u.nombre,
                                   apellido1 = u.apellido1,
                                   apellido2 = u.apellido2,
                                   idDepartamento = u.idDepartamento,
                                   idRol = u.idRol,
                                   nombreDepartamento= d.nombre,
                                   rutaImg = u.rutaImg,
                                   fechaCreacion = u.fechaCreacion
                               }).FirstOrDefault();

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
                                where u.correo == usuario.correo && u.cedulaFisica == usuario.cedulaFisica && u.estado == true
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

                        //INICIO: Envio de correo
                        string urlHtml = AppDomain.CurrentDomain.BaseDirectory + "TemplatesCorreos\\RecuperarContrasena.html";
                        string html = System.IO.File.ReadAllText(urlHtml);
                        html = html.Replace("@@nombre", user.nombre);
                        html = html.Replace("@@apellido1", user.apellido1);
                        html = html.Replace("@@apellido2", user.apellido2);
                        html = html.Replace("@@contrasena", nuevaContrasena);
                        util.enviarCorreo(user.correo, "Credenciales Recuperadas", html);
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

        //--- INICIO: Cambiar Contraseña
        [HttpPut]
        [Route("CambioContrasena")]
        public int CambiarContrasenaUsuario(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
                                select u).FirstOrDefault();

                    if (user.contrasena.Equals(usuario.contrasenaActual) && usuario.contrasenaNueva.Equals(usuario.contrasenaRepetida)) //pregunto si la contraseña del usuario buscado y la contraseña actual que viene desde el front son iguales
                    {//son iguales
                        user.contrasena = usuario.contrasenaNueva;//cambio la actual por la nueva
                        con.SaveChanges();

                        //INICIO: Envio de correo
                        string urlHtml = AppDomain.CurrentDomain.BaseDirectory + "TemplatesCorreos\\CambioContrasena.html";
                        string html = System.IO.File.ReadAllText(urlHtml);
                        html = html.Replace("@@nombre", user.nombre);
                        html = html.Replace("@@apellido1", user.apellido1);
                        html = html.Replace("@@apellido2", user.apellido2);
                        util.enviarCorreo(user.correo, "Credenciales Cambiadas", html);

                        return 1; //mensaje de exito
                    }

                    if (!usuario.contrasenaNueva.Equals(usuario.contrasenaRepetida))
                    {
                        return 2;
                    }

                    return 3;// las contraseñas actual no era la misma a la que esta registrada
                }
            }
            catch { return 500; }
        }

        [HttpPut]
        [Route("EditarMisDatos")]
        public int EditDatosPersonales(Usuario_Ent usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
                                select u).FirstOrDefault();

                    user.nombre = usuario.nombre;
                    user.apellido1 = usuario.apellido1;
                    user.apellido2 = usuario.apellido2;
                    user.correo = usuario.correo;

                    con.SaveChanges();

                    //INICIO: Envio de correo
                    string urlHtml = AppDomain.CurrentDomain.BaseDirectory + "TemplatesCorreos\\EditMisDatos.html";
                    string html = System.IO.File.ReadAllText(urlHtml);
                    html = html.Replace("@@nombre", user.nombre);
                    html = html.Replace("@@apellido1", user.apellido1);
                    html = html.Replace("@@apellido2", user.apellido2);
                    util.enviarCorreo(user.correo, "Datos Personales Actualizados", html);

                    return 1;
                }
            }
            catch
            {
                return 500;
            }
        }


        //--- INICIO: Avatar (imagen de perfil)
        [HttpPut]
        [Route("SubirAvatar")]
        public int SubirAvatar(Usuario usuario)
        {
            try
            {
                using(var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
                                select u).FirstOrDefault();

                    user.rutaImg = usuario.rutaImg;

                    con.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 500;
            }

        }

        [HttpPut]
        [Route("EliminarAvatar")]
        public int EliminarAvatar(Usuario usuario)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var user = (from u in con.Usuario
                                where u.idUsuario == usuario.idUsuario
                                select u).FirstOrDefault();

                    user.rutaImg = null;

                    con.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 500;
            }

        }

        /*-------------------- FIN USUARIOS COMUNES --------------------*/

    }//fin de la clase
    }//fin del namespace

