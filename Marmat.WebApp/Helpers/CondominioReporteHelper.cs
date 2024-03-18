#region Using
using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
#endregion

namespace Marmat.WebApp.Helpers
{
    public class CondominioReporteHelper
    {
        #region Condominio
        public object[] GetDataReporteCondominio()
        {
            try
            {
                var condominioViewModel = GetAllCondominios();
                int contador = 0;
                int contadorDos = 0;
                foreach (var item in condominioViewModel)
                {
                    contador++;
                }
                object[] rptCondominio = new object[contador];
                foreach (var item in condominioViewModel)
                {
                    rptCondominio[contadorDos] = new object[] { item.NombreCondominio, item.Vacantes };
                    contadorDos++;
                }
                return rptCondominio;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Condominios
        public List<CondominioViewModel> GetAllCondominios()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/condominio");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<CondominioViewModel> condominioViewModel =
                    JsonConvert.DeserializeObject<List<CondominioViewModel>>(content);
                return condominioViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion
    }
}
