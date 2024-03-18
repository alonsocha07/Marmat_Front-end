using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class ProvinciaController : Controller
    {
        #region Create
        public ActionResult RegistrarProvincia()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarProvincia(ProvinciaViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/provincia/", entity);
                response.EnsureSuccessStatusCode();
                ProvinciaViewModel entityViewModel = response.Content.ReadAsAsync<ProvinciaViewModel>().Result;
                return RedirectToAction("ConsultarUnaProvincia", new { id = entityViewModel.IdProvincia });
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
        public ActionResult MantenimientoProvincia()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/provincia");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<ProvinciaViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<ProvinciaViewModel>>(content);

                    ViewBag.Title = "All Provincia";
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

        #region Details
        [HttpGet]
        public ActionResult ConsultarUnaProvincia(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/provincia/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ProvinciaViewModel entityViewModel =
                        response.Content.ReadAsAsync<ProvinciaViewModel>().Result;
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

        #endregion

        #region Update
        public ActionResult ActualizarProvincia(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/provincia/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ProvinciaViewModel entityViewModel = response.Content.ReadAsAsync<ProvinciaViewModel>().Result;
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
        public ActionResult ActualizarProvincia(ProvinciaViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/provincia/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnaProvincia", new { id = entity.IdProvincia });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarProvincia(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/provincia/" + id.ToString());
                response.EnsureSuccessStatusCode();
                ProvinciaViewModel entityViewModel = response.Content.ReadAsAsync<ProvinciaViewModel>().Result;
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        public ActionResult EliminarProvincia(ProvinciaViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/provincia/" + entity.IdProvincia.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoProvincia", new { id = entity.IdProvincia });
        }
        #endregion
    }
}
