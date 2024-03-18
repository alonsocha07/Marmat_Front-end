using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class AvisoController : Controller
    {
        #region Create
        public ActionResult RegistrarAviso()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                AvisoViewModel entityViewModel = new AvisoViewModel();
                entityViewModel.usuarios = this.ObtenerUsuarios();
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarAviso(AvisoViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/aviso/", entity);
                response.EnsureSuccessStatusCode();
                AvisoViewModel entityViewModel = response.Content.ReadAsAsync<AvisoViewModel>().Result;
                return RedirectToAction("ConsultarUnAviso", new { id = entityViewModel.IdAviso });
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
        private UsuarioViewModel ObtenerUsuario(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario/"+id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                UsuarioViewModel usuarioViewModel =
                    responseMessage.Content.ReadAsAsync<UsuarioViewModel>().Result;
                //ViewBag.Title = "All Products";
                return usuarioViewModel;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult MantenimientoAviso()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/aviso");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<AvisoViewModel> entity =
                        JsonConvert.DeserializeObject<List<AvisoViewModel>>(content);
                    foreach (AvisoViewModel item in entity)
                    {
                        item.usuario = this.ObtenerUsuario(item.IdUsuario);
                    }
                    return View(entity);
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
        public ActionResult ConsultarUnAviso(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/aviso/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    AvisoViewModel entityViewModel =
                        response.Content.ReadAsAsync<AvisoViewModel>().Result;
                    entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdUsuario);
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
        public ActionResult ActualizarAviso(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/aviso/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    AvisoViewModel entityViewModel = response.Content.ReadAsAsync<AvisoViewModel>().Result;
                    entityViewModel.usuarios = this.ObtenerUsuarios();
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
        public ActionResult ActualizarAviso(AvisoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/aviso/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnAviso", new { id = entity.IdAviso });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarAviso(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/aviso/" + id.ToString());
                response.EnsureSuccessStatusCode();
                AvisoViewModel entityViewModel = response.Content.ReadAsAsync<AvisoViewModel>().Result;
                entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdAviso);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarAviso(AvisoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/aviso/" + entity.IdAviso.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoAviso", new { id = entity.IdAviso });
        }
        #endregion
    }
}
