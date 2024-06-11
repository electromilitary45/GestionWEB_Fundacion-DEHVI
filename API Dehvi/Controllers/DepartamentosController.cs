using API_Dehvi.Areas.HelpPage.ModelDescriptions;
using API_Dehvi.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Dehvi.Controllers
{
    public class DepartamentosController : ApiController
    {

        //------------------- Crear Departamento -------------------
        [HttpPost]
        [Route("CrearDepartamento")]
        public int CrearDepartamento(DepartamentoEnt departamento)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var d = new Departamento(); // se crea un nuevo departamento

                    d.nombre = departamento.nombre;
                    d.estado = true;

                    con.Departamento.Add(d);

                    con.SaveChanges();

                    return 1; // se realiza exitosamente el registro
                }
            }
            catch (Exception)
            {
                return 0; // es un registro fallido
            }
        } // fin del crear departamento

        //------------------- Lista de Departamentos -------------------
        [HttpGet]
        [Route("ListaDeparta")]
        public List<DepartamentoEnt> ListaDepartas()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from d in con.Departamento
                        orderby d.estado descending
                        select new DepartamentoEnt
                        {
                            idDepartamento = d.idDepartamento,
                            nombre = d.nombre,
                            estado = d.estado
                        }).ToList();
                    return data; // se muestran todos los departamentos
                }
            }
            catch (Exception)
            {
                return null; // error
            }

        } // fin de la lista de los departamentos

        //------------------- Lista de Departamentos Admin-------------------
        [HttpGet]
        [Route("ListaDepartasAdmin")]
        public List<DepartamentoEnt> ListaDepartasAdmin()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from d in con.Departamento
                        orderby d.estado descending
                        select new DepartamentoEnt
                        {
                            idDepartamento = d.idDepartamento,
                            nombre = d.nombre,
                            estado = d.estado
                        }).ToList();
                    return data; // se muestran todos los departamentos
                }
            }
            catch (Exception)
            {
                return null; // error
            }

        } // fin de la lista de los departamentos

        //------------------- Consulta Departamento -------------------
        [HttpGet]
        [Route("PerfilDepa")]
        public Departamento PerfilDepa(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())//conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var datos = (from x in con.Departamento
                                 where x.idDepartamento == q
                                 select x).FirstOrDefault();

                    if (datos != null)
                    {
                        return datos; //Departamento encontrado
                    }

                    return null; // No encontrado

                }

            }
            catch (Exception)
            {
                return null; // Error al consultar
            }
        } // fin perfil depa


        //------------------- Editar Departamento -------------------
        [HttpPut]
        [Route("ActualizarDepa")]
        public int ActualizarDepa(DepartamentoEnt departamento)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var data = (from d in con.Departamento
                                where d.idDepartamento == departamento.idDepartamento
                                select d).FirstOrDefault();
                    if (data != null)
                    {
                        data.nombre = departamento.nombre;
                        data.estado = departamento.estado;

                        con.SaveChanges(); // se guardan los nuevos datos

                        return 1; // se logra actualizar el departamento
                    }
                    return 2; // hay datos nulos
                }
            }
            catch (Exception)
            {
                return 0; // error al actualizar el departamento
            }

        }// fin del actualizar departamento 

        //------------------- Desactivar Departamento -------------------
        [HttpPut]
        [Route("EstadoDepartamento")]
        public int EstadoDeparta(DepartamentoEnt ent)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) // conexion a la base de datos
                {
                    var data = (from d in con.Departamento
                                where d.idDepartamento == ent.idDepartamento
                                select d).FirstOrDefault();

                    if (data != null)
                    {
                        if (data.estado == true)
                        {
                            data.estado = false; // se cambia el estado de activo a inactivo

                            con.SaveChanges();

                            return 1; // guarda el estado
                        }

                        data.estado = true; // se cambia el estado de inactivo a activo

                        con.SaveChanges();

                        return 2; // se guarda el estado
                    }

                    return 3; // departamento no encontrado
                }
            }
            catch (Exception)
            {
                return 0; // sucede un error al actualizar
            }

        }// fin de editar el estado

        //------------------- DropDown Departamento -------------------
        [HttpGet]
        [Route("DropDownDeparta")]
        public List<System.Web.Mvc.SelectListItem> DropDownDeparta()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (from d in con.Departamento
                                select d).ToList();

                    var result = new List<System.Web.Mvc.SelectListItem>();
                    foreach(var item in data) 
                    {
                        result.Add(new System.Web.Mvc.SelectListItem
                        {
                            Value = item.idDepartamento.ToString(),
                            Text = item.nombre
                        });
                    }
                    return result;
                }

            }// fin del try
            catch (Exception)
            {
                return null;
            }// fin del catch 

        }// fin del DropDown de departamentos

    } // fin de la clase
} // fin del namespace
