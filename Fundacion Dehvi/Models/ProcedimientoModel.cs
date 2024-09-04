using Fundacion_Dehvi.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Web;

namespace Fundacion_Dehvi.Models
{
    public class ProcedimientoModel
    {
        // conexión al proyecto Api
        public string urlApi = ConfigurationManager.AppSettings["urlApi"];

        //------------ Crear Procedimiento ------------
        public int CrearProce(ProcedimientosEnt proce)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "CrearProce";
                JsonContent cont = JsonContent.Create(proce);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        } // fin del crear procedimiento

        //------------ Lista Procedimientos Completa ------------
        public List<ProcedimientosEnt> ListaProces()
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "ListaProces";
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<ProcedimientosEnt>>().Result;
            }
        }// fin de la lista de todos los procedimientos

        //------------ Lista Procedimientos Sector ------------
        public List<ProcedimientosEnt> ListaProcesSector(long q)
        {
            using (var client = new HttpClient())
            {
                var url = urlApi + "ListaProcesSector?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<ProcedimientosEnt>>().Result;
            }
        }//fin lista de procedimientos por sector

        //------------ Perfil Procedimiento ------------
        public ProcedimientosEnt PerfilProce(long q)
        {
            using (var client = new HttpClient())
            {
                var url = urlApi + "PerfilProce?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<ProcedimientosEnt>().Result;
            }
        }//fin perfil procedimiento

        //------------ Actualizar Procedimiento ------------
        public int ActualizarProce(ProcedimientosEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "ActualizarProce";
                JsonContent contenido = JsonContent.Create(entidad);
                var resp = client.PutAsync(url, contenido).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        }// fin actualizar

        //------------ Desactivar Procedimiento ------------
        public int EstadoProce(ProcedimientosEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "EstadoProce";
                JsonContent contenido = JsonContent.Create(entidad);
                var resp = client.PutAsync(url, contenido).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        } // fin de desactivar

        //------------ Consulta Procedimiento ------------
        public ProcedimientosEnt ConsultaProce(long q)
        {
            using (var client = new HttpClient())
            {

                var url = urlApi + "ConsultaProce?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<ProcedimientosEnt>().Result;

            }
        }//fin consulta procedimiento
         //------------ Buscar Procedimiento ------------
        public List<ProcedimientosEnt> BuscarProcedimientos(string term)
        {
            using (var client = new HttpClient())
            {
                var url = urlApi + "BuscarProcedimientos?term=" + term;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<ProcedimientosEnt>>().Result;
            }
        }


    }// fin de la clase
}// fin del namespace