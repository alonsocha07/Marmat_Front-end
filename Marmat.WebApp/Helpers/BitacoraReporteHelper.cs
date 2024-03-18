#region Using
using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
#endregion

namespace Marmat.WebApp.Helpers
{
    public class BitacoraReporteHelper
    {
        #region Bitacora

        #region Reporte Bitacora
        public List<ReporteBitacora> GetReporteBitacora()
        {
            try
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
                    lista.Add(new ReporteBitacora(value.ToString() + " Total= " + contador, contador));
                }
                return lista;
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

        #region Bitacora Usuario
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

        #region Bitacora Roles
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

        #endregion

    }
}
