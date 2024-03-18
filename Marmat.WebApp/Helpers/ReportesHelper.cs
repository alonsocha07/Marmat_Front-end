using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Helpers
{
    public class ReportesHelper
    {
        #region Bitacora

        #region Roles
        private List<RolViewModel> ObtenerRoles()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/rol");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<RolViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<RolViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private RolViewModel ObtenerRol(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/rol/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                RolViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<RolViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Usuario
        public List<UsuarioViewModel> GetAllUsuario()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                foreach (UsuarioViewModel item in entityViewModel)
                {
                    item.rol = this.ObtenerRol(item.IdRol);
                }
                return entityViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public UsuarioViewModel ConsultarUnUsuario(int id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/usuario/" + id.ToString());
                response.EnsureSuccessStatusCode();
                UsuarioViewModel entityViewModel =
                    response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                entityViewModel.rol = this.ObtenerRol(entityViewModel.IdRol);
                return entityViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Bitacora
        public List<BitacoraViewModel> BitacoraGetAll()
        {
            try
            {
                BitacoraViewModel objetoBitacora = new BitacoraViewModel();
                objetoBitacora.usuarios = this.GetAllUsuario();

                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/bitacora");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<BitacoraViewModel> bitacoraViewModels =
                    JsonConvert.DeserializeObject<List<BitacoraViewModel>>(content);
                foreach (var item in bitacoraViewModels)
                {
                    item.usuario = ConsultarUnUsuario(item.ID_USUARIO);
                }
                return bitacoraViewModels;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Reporte Bitacora
        public List<ReporteBitacora> DataPastelBitacora()
        {
            var bitacoraViewModels = BitacoraGetAll();

            var consulta = bitacoraViewModels.Select(x => x.usuario.Nombre).Distinct();

            List<ReporteBitacora> lista = new List<ReporteBitacora>();

            foreach (var value in consulta)
            {
                int contador = 0;
                foreach (var item in bitacoraViewModels)
                {
                    if (item.usuario.Nombre == value)
                    {
                        contador++;
                    }
                }
                lista.Add(new ReporteBitacora(value.ToString(), contador));
            }
            return lista;
        }
        #endregion

        #endregion

        #region Condominio
        public object[] GetDataReporteCondominio()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/condominio");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<CondominioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CondominioViewModel>>(content);

                int contador = 0;
                int contadorDos = 0;

                foreach (var item in entityViewModel)
                {
                    contador++;
                }

                object[] rptCondominio = new object[contador];
                foreach (var item in entityViewModel)
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

        #region Departamento
        public object[] GetDataReporteDepartamentoCuartos()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);

                int contador = 0;
                int contadorDos = 0;

                foreach (var item in entityViewModel)
                {
                    contador++;
                }

                object[] rptDepaCuartos = new object[contador];
                foreach (var item in entityViewModel)
                {
                    rptDepaCuartos[contadorDos] = new object[] { item.IdDepartamento, item.CantidadCuartos };
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
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);

                int contador = 0;
                int contadorDos = 0;

                foreach (var item in entityViewModel)
                {
                    contador++;
                }

                object[] rptDepaBanios = new object[contador];
                foreach (var item in entityViewModel)
                {
                    rptDepaBanios[contadorDos] = new object[] { item.IdDepartamento, item.CantidadBanios };
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


        #region Usuario
        public List<ReporteUsuario> GetDataReporteUsuario()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);

                List<ReporteUsuario> lista = new List<ReporteUsuario>();

                var result = entityViewModel.Select(x => x.IdUsuario);

                foreach (var value in result)
                {
                    int contador = 0;
                    foreach (var item in entityViewModel)
                    {
                        if (item.IdUsuario == value)
                        {
                            contador++;
                        }
                    }
                    lista.Add(new ReporteUsuario(value.ToString(), contador));
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ReporteUsuarioRol> GetDataReporteUsuarioRol()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);

                List<ReporteUsuarioRol> lista = new List<ReporteUsuarioRol>();

                var result = entityViewModel.Select(x => x.IdUsuario);

                foreach (var value in result)
                {
                    int contador = 0;
                    foreach (var item in entityViewModel)
                    {
                        if (item.IdRol == value)
                        {
                            contador++;
                        }
                    }
                    lista.Add(new ReporteUsuarioRol(value.ToString(), contador));
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
