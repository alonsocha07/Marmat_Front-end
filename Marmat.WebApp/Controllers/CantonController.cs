using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class CantonController : Controller
    {
        #region Create
        public ActionResult RegistrarCanton()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                CantonViewModel item = new CantonViewModel();
                item.provincias = this.ObtenerProvincias();
                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCanton(CantonViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/canton/", entity);
                response.EnsureSuccessStatusCode();
                CantonViewModel entityViewModel = response.Content.ReadAsAsync<CantonViewModel>().Result;
                return RedirectToAction("ConsultarUnCanton", new { id = entityViewModel.IdCanton });
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

        private List<ProvinciaViewModel> ObtenerProvincias()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/provincia");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<ProvinciaViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<ProvinciaViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private ProvinciaViewModel ObtenerProvincia(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/provincia/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                ProvinciaViewModel provinciaViewModel =
                    responseMessage.Content.ReadAsAsync<ProvinciaViewModel>().Result;
                //ViewBag.Title = "All Products";
                return provinciaViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoCanton()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/canton");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<CantonViewModel> entityViewModels =
                        JsonConvert.DeserializeObject<List<CantonViewModel>>(content);
                    foreach (CantonViewModel item in entityViewModels)
                    {
                        item.provincia = this.ObtenerProvincia(item.IdProvincia);
                    }
                    return View(entityViewModels);
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
        public ActionResult ConsultarUnCanton(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/canton/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    CantonViewModel entityViewModel =
                        response.Content.ReadAsAsync<CantonViewModel>().Result;
                    entityViewModel.provincia = this.ObtenerProvincia(entityViewModel.IdProvincia);
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
        public ActionResult ActualizarCanton(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/canton/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    CantonViewModel entityViewModel = response.Content.ReadAsAsync<CantonViewModel>().Result;
                    entityViewModel.provincias = this.ObtenerProvincias();
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
        public ActionResult ActualizarCanton(CantonViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/canton/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnCanton", new { id = entity.IdCanton });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarCanton(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/canton/" + id.ToString());
                response.EnsureSuccessStatusCode();
                CantonViewModel entityViewModel = response.Content.ReadAsAsync<CantonViewModel>().Result;
                entityViewModel.provincia = this.ObtenerProvincia(entityViewModel.IdProvincia);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarCanton(CantonViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/canton/" + entity.IdCanton.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoCanton", new { id = entity.IdCanton });
        }
        #endregion
    }
}
