using API_Dehvi.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Dehvi.Controllers
{
    public class ProcedimientoController : ApiController
    {
        //------------------- Crear Procedimientos -------------------
        [HttpPost]
        [Route("CrearProce")]
        public int CrearProce(ProcedimientosEnt procedimiento)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var p = new Procedimiento();

                    if (CodigoExistente(procedimiento.codProcedimiento) != true)
                    {
                        p.codProcedimiento = procedimiento.codProcedimiento;
                        p.idDepartamento = procedimiento.idDepartamento;
                        p.nombre = procedimiento.nombre;
                        p.descripcion = procedimiento.descripcion;
                        p.objetivo = procedimiento.objetivo;
                        p.estado = true;
                        p.idUsuarioCreador = procedimiento.idUsuario;
                        p.fechaCreacion = DateTime.Now;

                        con.Procedimiento.Add(p);
                        con.SaveChanges();

                        return 1; // se realiza exitosamente el registro
                    }
                    else
                    {
                        return 2;
                    }

                }
            }// fin del try
            catch (Exception)
            {
                return 500; // es un registro fallido

            } // fin del catch
        }// fin de crear procedimientos

        //------------------- Lista Procedimientos Total -------------------
        [HttpGet]
        [Route("ListaProces")]
        public List<ProcedimientosEnt> ListaProces()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from p in con.Procedimiento
                        join d in con.Departamento on p.idDepartamento equals d.idDepartamento
                        join u in con.Usuario on p.idUsuarioCreador equals u.idUsuario

                        orderby p.nombre descending
                        select new ProcedimientosEnt
                        {
                            idProcedimiento = p.idProcedimiento,
                            codProcedimiento = p.codProcedimiento,
                            nombre = p.nombre,
                            objetivo = p.objetivo,
                            descripcion = p.descripcion,
                            estado = p.estado,
                            fechaCreacion = p.fechaCreacion,

                            idDepartamento = p.idDepartamento,
                            nombreDeparta = d.nombre,

                            idUsuario = p.idUsuarioCreador,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }).ToList();
                    return data;
                }

            }// fin del try
            catch (Exception)
            {
                return null;
            }// fin del catch
        }// fin de la lista total de procedimientos

        //------------------- Lista Procedimientos por Sector -------------------
        [HttpGet]
        [Route("ListaProcesSector")]
        public List<ProcedimientosEnt> ListaProcesSector(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from p in con.Procedimiento
                        join d in con.Departamento on p.idDepartamento equals d.idDepartamento
                        join u in con.Usuario on p.idUsuarioCreador equals u.idUsuario
                        orderby p.nombre descending
                        where p.idDepartamento == q
                        select new ProcedimientosEnt
                        {
                            idProcedimiento = p.idProcedimiento,
                            codProcedimiento = p.codProcedimiento,
                            nombre = p.nombre,
                            objetivo = p.objetivo,
                            descripcion = p.descripcion,
                            estado = p.estado,
                            fechaCreacion = p.fechaCreacion,

                            idDepartamento = p.idDepartamento,
                            nombreDeparta = d.nombre,

                            idUsuario = p.idUsuarioCreador,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }).ToList();
                    return data;
                }

            }// fin del try
            catch (Exception)
            {
                return null;
            }// fin del catch
        }// fin de la lista total de procedimientos

        //------------------- Perfil Procedimiento -------------------
        [HttpGet]
        [Route("PerfilProce")]
        public Procedimiento PerfilProce(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())//conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var data = (from p in con.Procedimiento
                                where p.idProcedimiento == q
                                select p).FirstOrDefault();

                    if (data != null)
                    {
                        return data;//Departamento encontrado
                    }
                    else
                    {
                        return null;// No encontrado
                    }
                }

            }// fin del try
            catch
            {
                return null;// Error al consultar
            }//fin del catch
        }// fin del perfil procedimiento

        //------------------- Actualizar Procedimiento -------------------
        [HttpPut]
        [Route("ActualizarProce")]
        public int ActualizarProce(ProcedimientosEnt procedimiento)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (from p in con.Procedimiento
                                where p.idProcedimiento == procedimiento.idProcedimiento
                                select p).FirstOrDefault();

                    if (data != null)
                    {
                        data.nombre = procedimiento.nombre;
                        data.objetivo = procedimiento.objetivo;
                        data.descripcion = procedimiento.descripcion;
                        data.estado = procedimiento.estado;

                        con.SaveChanges(); // se guardan los nuevos datos

                        return 1; // se logra actualizar el departamento
                    }
                    return 2; // hay datos nulos
                }
            }//fin del try
            catch (Exception)
            {
                return 0; // error al actualizar el procedimiento
            }// fin del catch

        }// fin del actualizar procedimiento

        //------------------- Desactivar Procedimiento ------------------
        [HttpPut]
        [Route("EstadoProce")]
        public int EstadoProce(ProcedimientosEnt proce)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (from p in con.Procedimiento
                                where p.idProcedimiento == proce.idProcedimiento
                                select p).FirstOrDefault();

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

                    return 3; // procedimiento no encontrado
                }

            }// fin del try
            catch (Exception)
            {
                return 0; // sucede un error al actualizar
            }// fin del catch

        }// fin de actualizar estado procedimiento 

        //------------------- Codigo Procedimiento Existente ------------------
        [HttpGet]
        [Route("CodigoExistente")]
        public bool CodigoExistente(string codigo)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var data = (from p in con.Procedimiento
                                where p.codProcedimiento == codigo
                                select p).FirstOrDefault();

                    if (data != null)
                    {
                        return true;// ya existe procedimiento con ese codigo
                    }
                    else
                    {
                        return false;// no existe procedimiento con ese codigo
                    }
                }
            }// fin del try
            catch (Exception)
            {
                return false;
            }// fin del catch
        }// fin de buscar si el codigo es existente

        //------------------- Consulta de Procedimiento ------------------
        [HttpGet]
        [Route("ConsultaProce")]
        public Procedimiento ConsultaProce(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var data = (from p in con.Procedimiento
                                where p.idProcedimiento == q
                                select p).FirstOrDefault();
                    if (data != null)
                    {
                        return data; // Procedimiento encontrado
                    }
                    return null; // no encontrado
                }
            }// fin del try
            catch (Exception)
            {
                return null; // Error al consultar
            }// fin del catch
        }// fin de la consulta de la empresa

        //------------------- Buscar Procedimientos -------------------
        [HttpGet]
        [Route("BuscarProcedimientos")]
        public List<ProcedimientosEnt> BuscarProcedimientos(string term)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from p in con.Procedimiento
                        join d in con.Departamento on p.idDepartamento equals d.idDepartamento
                        join u in con.Usuario on p.idUsuarioCreador equals u.idUsuario
                        where p.nombre.Contains(term)
                        orderby p.nombre descending
                        select new ProcedimientosEnt
                        {
                            idProcedimiento = p.idProcedimiento,
                            codProcedimiento = p.codProcedimiento,
                            nombre = p.nombre,
                            objetivo = p.objetivo,
                            descripcion = p.descripcion,
                            estado = p.estado,
                            fechaCreacion = p.fechaCreacion,
                            idDepartamento = p.idDepartamento,
                            nombreDeparta = d.nombre,
                            idUsuario = p.idUsuarioCreador,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }).ToList();
                    return data;
                }
            }
            catch (Exception)
            {
                return null; // Error al realizar la búsqueda
            }
        }// fin de la busqueda de procedimiento
    }// fin de la clase
}// fin del namespace