#region Using
using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Newtonsoft.Json; 
#endregion

namespace Marmat.WebApp.Helpers
{
    public class UsuarioReporteHelper
    {
        #region Reporte Usuario
        public List<ReporteUsuario> GetDataReporteUsuario()
        {
            try
            {
                var usuarioViewModel = GetAllUsuarios();

                List<ReporteUsuario> lista = new List<ReporteUsuario>();

                var result = usuarioViewModel.Select(x => x.rol.NombreRol).Distinct();

                foreach (var value in result)
                {
                    int contador = 0;
                    foreach (var item in usuarioViewModel)
                    {
                        if (item.rol.NombreRol == value)
                        {
                            contador++;
                        }
                    }
                    lista.Add(new ReporteUsuario(value.ToString() + " Total= " + contador, contador));
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Usuarios
        public List<UsuarioViewModel> GetAllUsuarios()
        {
            UsuarioViewModel objeto = new UsuarioViewModel();
            objeto.roles = this.ObtenerRoles();

            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> usuarioViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                foreach (var item in usuarioViewModel)
                {
                    item.rol = this.ObtenerRol(item.IdRol);
                }
                return usuarioViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region Usuario Roles
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
    }
}
