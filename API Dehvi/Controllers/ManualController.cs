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
    public class ManualController : ApiController
    {
        /*------------------------------MANUALES--------------------------*/

        //---- INICIO: Crear un Manual -----
        [HttpPost]
        [Route("CrearManual")]
        public long CrearManual(Manual_Ent manual)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {

                    //--Se crea el manual
                    var Manual = new Manual
                    {
                        idProcedimiento = manual.idProcedimiento,
                        nombre = manual.nombre,
                        codReferencia = manual.codReferencia,
                        estado = true,
                        idUsuarioCreador = manual.idUsuarioCreador,
                        fechaCreacion = manual.fechaCreacion
                    };
                    con.Manual.Add(Manual);
                    con.SaveChanges();
                    return Manual.idManual;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        //--- INICIO:  Lista Manuales por Procedimiento ----
        [HttpGet]
        [Route("ListaManProcedimiento")]
        public List<Manual_Ent> ListaManualesProcedimiento(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var data = (
                        from m in con.Manual
                        join u in con.Usuario on m.idUsuarioCreador equals u.idUsuario
                        orderby m.estado descending
                        where m.idProcedimiento == q
                        select new Manual_Ent
                        {
                            idManual = m.idManual,
                            estado = m.estado,
                            codReferencia = m.codReferencia,
                            fechaCreacion = m.fechaCreacion,
                            idProcedimiento = m.idProcedimiento,
                            idUsuarioCreador = m.idUsuarioCreador,
                            nombre = m.nombre,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }
                        ).ToList();
                    return data;
                }

            }// fin del try
            catch (Exception)
            {
                return null;
            }// fin del catch
        }// fin de la lista total de procedimientos

        //--- INICIO: Consulta para un Manual en especifico
        [HttpGet]
        [Route("ConsultaManualId")]
        public Manual ConsultaManualEspecifico(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var data = (
                        from m in con.Manual
                        where m.idManual == q
                        select m
                        ).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //--- INICIO: Consulta General Manuales
        [HttpGet]
        [Route("ConsultaManuales")]
        public List<Manual_Ent> ConsultaManuales()
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from m in con.Manual
                        join p in con.Procedimiento on m.idProcedimiento equals p.idProcedimiento
                        join u in con.Usuario on m.idUsuarioCreador equals u.idUsuario
                        
                        orderby m.fechaCreacion descending
                        select new Manual_Ent
                        {
                            idManual = m.idManual,
                            estado = m.estado,
                            codReferencia = m.codReferencia,
                            fechaCreacion = m.fechaCreacion,
                            idProcedimiento = m.idProcedimiento,
                            idUsuarioCreador = m.idUsuarioCreador,
                            idDepartamento= p.idDepartamento,
                            nombre = m.nombre,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2

                        }).ToList();
                    return data;
                }

            }// fin del try
            catch (Exception)
            {
                return null;// error
            }// fin del catch

        }// fin de la lista completa de manuales

        //--- INICIO: Actualizar Manual
        [HttpPut]
        [Route("ActualizarManual")]
        public int ActualizarManual(Manual_Ent manual)
        {
            try
          {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from m in con.Manual
                        where m.idManual == manual.idManual
                        select m).FirstOrDefault();

                    if (data != null)
                    {
                        data.nombre = manual.nombre;
                        data.estado = manual.estado;

                        con.SaveChanges();

                        return 1;
                    }
                    return 2;
                }

            }// fin del try
            catch (Exception)
            {
                return 0;// error
            }// fin del catch

        }// fin de actualizar manuales 

        //--- INICIO: Actualizar Estado Manual
        [HttpPut]
        [Route("EstadoManual")]
        public int EstadoManual(Manual_Ent manual)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    var data = (
                        from m in con.Manual
                        where m.idManual == manual.idManual
                        select m).FirstOrDefault();

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

                    return 3; // manual no encontrado
                }
            }
            catch (Exception)
            {
                return 0;// error
            }// fin del catch

        }// fin de actualizar estado manuales

        /*-------------------------------DOC MANUAL-------------------------------*/

        //---- INICIO: CrearDocManual----

        [HttpPost]
        [Route("CrearDocManual")]
        public long RegistrarDocManual(DocManual_Ent docManual)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    var DocManual = new DocManual
                    {
                        idManual = docManual.idManual,
                        versionDoc = contarArchivosPorManual(docManual.idManual),
                        estado = true,
                        idUsuarioCreador = docManual.idUsuarioCreador,
                        ruta = null,
                        fechaCreacion = docManual.fechaCreacion
                    };
                    con.DocManual.Add(DocManual);
                    con.SaveChanges();
                    return DocManual.idDocManual;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //---- INICIO: Actualizar la ruta de un DocManual
        [HttpPut]
        [Route("ActualizarRutaDocManual")]
        public int RutaDocManual(DocManual_Ent docManual)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;

                    var doc = (from d in con.DocManual
                               where d.idDocManual == docManual.idDocManual
                               select d
                               ).FirstOrDefault();

                    doc.ruta = docManual.ruta;
                    con.SaveChanges();

                    return 1;
                }
            }
            catch (Exception)
            {
                return 500;
            }
        }

        //---- INICIO: Contador de los documentos que tiene un Manual
        // no es accesible por el api
        public long contarArchivosPorManual(long idManual)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var contador = (
                        from m in con.DocManual
                        where m.idManual == idManual
                        select m
                        ).Count();

                    if (contador != 0)
                    {
                        contador++;
                    }

                    return contador;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //----INICIO: Lista Documenos por Manual ---
        [HttpGet]
        [Route("ListaDocManualxManual")]
        public List<DocManual_Ent> ListaDocManualxManual(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var data = (
                        from dm in con.DocManual
                        join u in con.Usuario on dm.idUsuarioCreador equals u.idUsuario
                        where dm.idManual == q
                        orderby dm.fechaCreacion descending
                        select new DocManual_Ent
                        {
                            idDocManual = dm.idDocManual,
                            idManual = dm.idManual,
                            estado = dm.estado,
                            fechaCreacion = dm.fechaCreacion,
                            idUsuarioCreador = dm.idUsuarioCreador,
                            ruta = dm.ruta,
                            versionDoc = dm.versionDoc,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }
                        ).ToList();
                    return data;
                }

            }// fin del try
            catch (Exception)
            {
                return null;
            }// fin del catch
        }// fin de la lista total de procedimientos

        //----INICIO: LIsta Documentos por manual
        [HttpGet]
        [Route("ListaDocManualxManual2")]
        public List<DocManual_Ent> ListaDocManualxManual2(long q)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities()) //conexion a la base de datos
                {
                    con.Configuration.LazyLoadingEnabled = false;
                    var data = (
                        from dm in con.DocManual
                        join u in con.Usuario on dm.idUsuarioCreador equals u.idUsuario
                        where dm.idManual == q
                        orderby dm.fechaCreacion descending
                        select new DocManual_Ent
                        {
                            idDocManual = dm.idDocManual,
                            idManual = dm.idManual,
                            estado = dm.estado,
                            fechaCreacion = dm.fechaCreacion,
                            idUsuarioCreador = dm.idUsuarioCreador,
                            ruta = dm.ruta,
                            versionDoc = dm.versionDoc,
                            nombreUsuario = u.nombre + " " + u.apellido1 + " " + u.apellido2
                        }
                    ).Take(3).ToList(); // Limitar a los últimos 3 registros}
                    return data;
                }
            }
            catch (Exception) { return null; }

        }//fin Lista domanual 2 solo da los ultimos 3 registros

        //----INICIO: Desactivar todos los docManual menos el ultimo registrado
        [HttpPut]
        [Route("DesactivarDocManuales")]
        public int DesacDocManual(long q, long a)
        {
            try
            {
                using (var con = new BD_fundacionDehviEntities())
                {
                    //--se buscan los ultimos 3 
                    var data = (
                        from dm in con.DocManual
                        where dm.idManual == q
                        orderby dm.fechaCreacion descending
                        select dm
                    ).Take(3).ToList(); // Limitar a los últimos 3 registros

                    //--se desactivan todos excepto el ultimo creado
                    foreach (var d in data)
                    {
                        if (d != null)
                        {
                            if (d.idDocManual != a)
                            {
                                d.estado = false;
                            }
                        }
                    }
                    con.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



    }//fin class
}//fin namespace
