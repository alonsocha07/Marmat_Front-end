using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class AreaComunController : Controller
    {
        #region Create
        public ActionResult RegistrarAreaComun()
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
        public ActionResult RegistrarAreaComun(AreacomunViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/AreaComun/", entity);
                response.EnsureSuccessStatusCode();
                AreacomunViewModel entityViewModel = response.Content.ReadAsAsync<AreacomunViewModel>().Result;
                return RedirectToAction("ConsultarUnAreaComun", new { id = entityViewModel.IdAreacomun });
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
        public ActionResult MantenimientoAreaComun()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/AreaComun");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<AreacomunViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<AreacomunViewModel>>(content);

                    ViewBag.Title = "All Area Comun";
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
        public ActionResult ConsultarUnAreaComun(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/AreaComun/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    AreacomunViewModel entityViewModel =
                        response.Content.ReadAsAsync<AreacomunViewModel>().Result;
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
        public ActionResult ActualizarAreaComun(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/AreaComun/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    AreacomunViewModel entityViewModel = response.Content.ReadAsAsync<AreacomunViewModel>().Result;
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
        public ActionResult ActualizarAreaComun(AreacomunViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/AreaComun/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnAreaComun", new { id = entity.IdAreacomun });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarAreaComun(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/AreaComun/" + id.ToString());
                response.EnsureSuccessStatusCode();
                AreacomunViewModel entityViewModel = response.Content.ReadAsAsync<AreacomunViewModel>().Result;
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        public ActionResult EliminarAreaComun(AreacomunViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/AreaComun/" + entity.IdAreacomun.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoAreaComun", new { id = entity.IdAreacomun });
        }
        #endregion
    }
}
