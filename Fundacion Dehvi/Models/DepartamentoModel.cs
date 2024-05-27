using Fundacion_Dehvi.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace Fundacion_Dehvi.Models
{
    public class DepartamentoModel
    {
        // conexión al proyecto Api
        public string urlApi = ConfigurationManager.AppSettings["urlApi"]; 

        //------------ Registrar Departamento ------------
        public int CrearDepartamento (DepartamentoEnt depa)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "CrearDepartamento";
                JsonContent cont = JsonContent.Create(depa);
                var resp = client.PostAsync(url,cont).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        } // fin del crear departamento

        //------------ Lista de Departamentos ------------
        public List<DepartamentoEnt> ListaDeparta()
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "ListaDeparta";
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<DepartamentoEnt>>().Result;
            }
        }// fin de la lista de departamentos


        //------------ Perfil de Departamento ------------
        public DepartamentoEnt PerfilDepa(long q)
        {
            using (var client = new HttpClient())
            {

                var url = urlApi + "PerfilDepa?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<DepartamentoEnt>().Result;

            }
        }//fin perfil departamento

        //------------ Actualizar Departamento ------------
        public int ActualizarDepa(DepartamentoEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "ActualizarDepa";
                JsonContent contenido = JsonContent.Create(entidad);
                var resp = client.PutAsync(url, contenido).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        }// fin actualizar

        //------------ Desactivar Departamento ------------
        public int EstadoDepartamento(DepartamentoEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = urlApi + "EstadoDepartamento";
                JsonContent contenido = JsonContent.Create(entidad);
                var resp = client.PutAsync(url, contenido).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        } // fin de desactivar

    }
}