using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class DistritoController : Controller
    {
        #region Create
        public ActionResult RegistrarDistrito()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                DistritoViewModel entity = new DistritoViewModel();
                entity.cantones = this.ObtenerCantones();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarDistrito(DistritoViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/distrito/", entity);
                response.EnsureSuccessStatusCode();
                DistritoViewModel entityViewModel = response.Content.ReadAsAsync<DistritoViewModel>().Result;
                return RedirectToAction("ConsultarUnDistrito", new { id = entityViewModel.IdDistrito });
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
        private List<CantonViewModel> ObtenerCantones()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/canton");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<CantonViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CantonViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private CantonViewModel ObtenerCanton(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/canton/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                CantonViewModel distritoViewModel =
                    responseMessage.Content.ReadAsAsync<CantonViewModel>().Result;
                return distritoViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoDistrito()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/distrito");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<DistritoViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<DistritoViewModel>>(content);
                    foreach (DistritoViewModel item in entityViewModel)
                    {
                        item.canton = this.ObtenerCanton(item.IdCanton);
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
        public ActionResult ConsultarUnDistrito(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/distrito/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DistritoViewModel entityViewModel =
                        response.Content.ReadAsAsync<DistritoViewModel>().Result;
                    entityViewModel.canton = this.ObtenerCanton(entityViewModel.IdCanton);
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
        public ActionResult ActualizarDistrito(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/distrito/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DistritoViewModel entityViewModel = response.Content.ReadAsAsync<DistritoViewModel>().Result;
                    entityViewModel.cantones = this.ObtenerCantones();
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
        public ActionResult ActualizarDistrito(DistritoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/distrito/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnDistrito", new { id = entity.IdDistrito });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarDistrito(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/distrito/" + id.ToString());
                response.EnsureSuccessStatusCode();
                DistritoViewModel entityViewModel = response.Content.ReadAsAsync<DistritoViewModel>().Result;
                entityViewModel.canton = this.ObtenerCanton(entityViewModel.IdCanton);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarDistrito(DistritoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/distrito/" + entity.IdDistrito.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoDistrito", new { id = entity.IdDistrito });
        }
        #endregion
    }
}
