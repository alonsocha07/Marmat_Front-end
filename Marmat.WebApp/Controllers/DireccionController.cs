using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class DireccionController : Controller
    {
        #region Create
        public ActionResult RegistrarDireccion()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                DireccionViewModel entity = new DireccionViewModel();
                entity.distritos = this.ObtenerDistritos();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarDireccion(DireccionViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/direccion/", entity);
                response.EnsureSuccessStatusCode();
                DireccionViewModel entityViewModel = response.Content.ReadAsAsync<DireccionViewModel>().Result;
                return RedirectToAction("ConsultarUnaDireccion", new { id = entityViewModel.IdDireccion });
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
        private List<DistritoViewModel> ObtenerDistritos()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/distrito");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DistritoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<DistritoViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private DistritoViewModel ObtenerDistrito(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/distrito/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                DistritoViewModel distritoViewModel =
                    responseMessage.Content.ReadAsAsync<DistritoViewModel>().Result;
                return distritoViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoDireccion()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/direccion");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<DireccionViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<DireccionViewModel>>(content);
                    foreach (DireccionViewModel item in entityViewModel)
                    {
                        item.distrito = this.ObtenerDistrito(item.IdDistrito);
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
        public ActionResult ConsultarUnaDireccion(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/direccion/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DireccionViewModel entityViewModel =
                        response.Content.ReadAsAsync<DireccionViewModel>().Result;
                    entityViewModel.distrito = this.ObtenerDistrito(entityViewModel.IdDistrito);
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
        public ActionResult ActualizarDireccion(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/direccion/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    DireccionViewModel entityViewModel = response.Content.ReadAsAsync<DireccionViewModel>().Result;
                    entityViewModel.distritos = this.ObtenerDistritos();
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
        public ActionResult ActualizarDireccion(DireccionViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/direccion/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnaDireccion", new { id = entity.IdDireccion });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarDireccion(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/direccion/" + id.ToString());
                response.EnsureSuccessStatusCode();
                DireccionViewModel entityViewModel = response.Content.ReadAsAsync<DireccionViewModel>().Result;
                entityViewModel.distrito = this.ObtenerDistrito(entityViewModel.IdDistrito);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarDireccion(DireccionViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/direccion/" + entity.IdDireccion.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoDireccion", new { id = entity.IdDireccion });
        }
        #endregion
    }
}
