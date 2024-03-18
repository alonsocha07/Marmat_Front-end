#region Using
using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Newtonsoft.Json;
#endregion

namespace Marmat.WebApp.Helpers
{
    public class DepartamentoReporteHelper
    {
        #region Departamento
        public object[] GetDataReporteDepartamentoCuartos()
        {
            try
            {
                var departamentoViewModel = GetAllDepartamentos();
                int contador = 0;
                int contadorDos = 0;
                foreach (var item in departamentoViewModel)
                {
                    contador++;
                }

                object[] rptDepaCuartos = new object[contador];
                foreach (var item in departamentoViewModel)
                {
                    rptDepaCuartos[contadorDos] = new object[] { item.condominio.NombreCondominio, item.CantidadCuartos };
                    contadorDos++;
                }
                return rptDepaCuartos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object[] GetDataReporteDepartamentoBanios()
        {
            try
            {
                var departamentoViewModel = GetAllDepartamentos();
                int contador = 0;
                int contadorDos = 0;

                foreach (var item in departamentoViewModel)
                {
                    contador++;
                }

                object[] rptDepaBanios = new object[contador];
                foreach (var item in departamentoViewModel)
                {
                    rptDepaBanios[contadorDos] = new object[] { item.condominio.NombreCondominio, item.CantidadBanios };
                    contadorDos++;
                }
                return rptDepaBanios;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Departamentos
        public List<DepartamentoViewModel> GetAllDepartamentos()
        {
            DepartamentoViewModel objeto = new DepartamentoViewModel();
            objeto.condominios = this.GetAllCondominios();

            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> departamentoViewModel =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);
                foreach (var item in departamentoViewModel)
                {
                    item.condominio = this.ConsultarUnCondominio(item.IdCondominio);
                }
                return departamentoViewModel;
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

        public CondominioViewModel ConsultarUnCondominio(int id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/condominio/" + id.ToString());
                response.EnsureSuccessStatusCode();
                CondominioViewModel entityViewModel =
                    response.Content.ReadAsAsync<CondominioViewModel>().Result;
                return entityViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
