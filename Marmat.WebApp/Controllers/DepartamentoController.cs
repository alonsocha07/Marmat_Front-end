using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class DepartamentoController : Controller
    {
        #region Create
        public ActionResult RegistrarDepartamento()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                DepartamentoViewModel item = new DepartamentoViewModel();
                item.usuarios = this.ObtenerUsuarios();
                item.condominios = this.ObtenerCondominios();
                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarDepartamento(DepartamentoViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/departamento/", entity);
                response.EnsureSuccessStatusCode();
                DepartamentoViewModel entityViewModel = response.Content.ReadAsAsync<DepartamentoViewModel>().Result;
                return RedirectToAction("ConsultarUnDepartamento", new { id = entityViewModel.IdDepartamento });
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Read
        private List<UsuarioViewModel> ObtenerUsuarios()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private UsuarioViewModel ObtenerUsuario(int? id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                UsuarioViewModel usuarioViewModel =
                    responseMessage.Content.ReadAsAsync<UsuarioViewModel>().Result;
                return usuarioViewModel;
            }
            catch
            {
                return null;
            }
        }
        private List<CondominioViewModel> ObtenerCondominios()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/condominio");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<CondominioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CondominioViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private CondominioViewModel ObtenerCondominio(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/condominio/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                CondominioViewModel condominioViewModel =
                    responseMessage.Content.ReadAsAsync<CondominioViewModel>().Result;
                return condominioViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoDepartamento()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<DepartamentoViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);
                    foreach (DepartamentoViewModel item in entityViewModel)
                    {
                        if (item.IdUsuario != null)
                            item.usuario = this.ObtenerUsuario(item.IdUsuario);

                        item.condominio = this.ObtenerCondominio(item.IdCondominio);
                    }
                    return View(entityViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult ConsultarUnDepartamento(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/departamento/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DepartamentoViewModel entityViewModel =
                        response.Content.ReadAsAsync<DepartamentoViewModel>().Result;
                    if (entityViewModel.IdUsuario != null)
                        entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdUsuario);
                    entityViewModel.condominio = this.ObtenerCondominio(entityViewModel.IdCondominio);
                    return View(entityViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update
        public ActionResult ActualizarDepartamento(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/departamento/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DepartamentoViewModel entityViewModel = response.Content.ReadAsAsync<DepartamentoViewModel>().Result;
                    entityViewModel.usuarios = this.ObtenerUsuarios();
                    entityViewModel.condominios = this.ObtenerCondominios();
                    return View(entityViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActualizarDepartamento(DepartamentoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/departamento/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnDepartamento", new { id = entity.IdDepartamento });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarDepartamento(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/departamento/" + id.ToString());
                response.EnsureSuccessStatusCode();
                DepartamentoViewModel entityViewModel = response.Content.ReadAsAsync<DepartamentoViewModel>().Result;
                if (entityViewModel.IdUsuario != null)
                    entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdUsuario);
                entityViewModel.condominio = this.ObtenerCondominio(entityViewModel.IdCondominio);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarDepartamento(DepartamentoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/departamento/" + entity.IdDepartamento.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoDepartamento", new { id = entity.IdDepartamento });
        }
        #endregion
    }
}
