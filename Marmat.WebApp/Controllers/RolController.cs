using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class RolController : Controller
    {
        #region Create
        public ActionResult RegistrarRol()
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
        public ActionResult RegistrarRol(RolViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/rol/", entity);
                response.EnsureSuccessStatusCode();
                RolViewModel entityViewModel = response.Content.ReadAsAsync<RolViewModel>().Result;
                return RedirectToAction("ConsultarUnRol", new { id = entityViewModel.IdRol });
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
        public ActionResult MantenimientoRol()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/rol");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<RolViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<RolViewModel>>(content);

                    ViewBag.Title = "All Rol";
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
        public ActionResult ConsultarUnRol(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/rol/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    RolViewModel entityViewModel =
                        response.Content.ReadAsAsync<RolViewModel>().Result;
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
        public ActionResult ActualizarRol(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/rol/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    RolViewModel entityViewModel = response.Content.ReadAsAsync<RolViewModel>().Result;
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
        public ActionResult ActualizarRol(RolViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/rol/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnRol", new { id = entity.IdRol });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarRol(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/rol/" + id.ToString());
                response.EnsureSuccessStatusCode();
                RolViewModel entityViewModel = response.Content.ReadAsAsync<RolViewModel>().Result;
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        public ActionResult EliminarRol(RolViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/rol/" + entity.IdRol.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoRol", new { id = entity.IdRol });
        }
        #endregion
    }
}
