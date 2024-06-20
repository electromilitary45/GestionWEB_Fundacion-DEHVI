using Fundacion_Dehvi.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;

namespace Fundacion_Dehvi.Models
{
    public class ManualModel
    {
        //----- INSTANCIAS -----
        private readonly string urlAPI = ConfigurationManager.AppSettings["urlApi"];

        //----INICIO: Registro de nuevo manual---
        public long CrearManual(Manual_Ent manual)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "CrearManual";
                JsonContent cont = JsonContent.Create(manual);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<long>().Result;
            }
        }

        //----INICIO: Registro de un DocManual
        public long CrearDocManual(DocManual_Ent docManual)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "CrearDocManual";
                JsonContent cont = JsonContent.Create(docManual);
                var resp = client.PostAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<long>().Result;
            }
        }

        //----INICIO: Actualizar ruta doc
        public int ActRutaDocManual(DocManual_Ent docManual)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ActualizarRutaDocManual";
                JsonContent cont = JsonContent.Create(docManual);
                var resp = client.PutAsync(url, cont).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        }

        //----INICIO: Listat Manuales por procedimiento
        public List<Manual_Ent> ListaManProcedimiento(long q)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ListaManProcedimiento?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<Manual_Ent>>().Result;
            }
        }

        //----INICIO: Lista de DocManuales por Manuales
        public List<DocManual_Ent> ListaDocManualxManual(long q)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ListaDocManualxManual?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<DocManual_Ent>>().Result;
            }
        }

        //----INICIO: Lista de DocManuales por Manuales
        public List<DocManual_Ent> ListaDocManualxManual2(long q)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ListaDocManualxManual2?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<List<DocManual_Ent>>().Result;
            }
        }

        //----INICIO: Consulta Manual
        public Manual_Ent ConsultaManualId(long q)
        {
            using (var client = new HttpClient())
            {
                string url = urlAPI + "ConsultaManualId?q=" + q;
                var resp = client.GetAsync(url).Result;
                return resp.Content.ReadFromJsonAsync<Manual_Ent>().Result;
            }
        }

        //----INICIO: Desactivar DocManuales
        public int DesactivarDocManuales(long q, long a)
        {
            using (var client = new HttpClient())
            {
                string url = $"{urlAPI}DesactivarDocManuales?q={q}&a={a}";
                var resp = client.PutAsync(url, null).Result;
                return resp.Content.ReadFromJsonAsync<int>().Result;
            }
        }
    }//fin class
}//fin namespace