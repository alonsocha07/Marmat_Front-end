using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class EstadoController : Controller
    {
        #region Create
        public ActionResult RegistrarEstado()
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
        public ActionResult RegistrarEstado(EstadoViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/estado/", entity);
                response.EnsureSuccessStatusCode();
                EstadoViewModel entityViewModel = response.Content.ReadAsAsync<EstadoViewModel>().Result;
                return RedirectToAction("ConsultarUnEstado", new { id = entityViewModel.IdEstado });
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
        public ActionResult MantenimientoEstado()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/estado");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<EstadoViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<EstadoViewModel>>(content);

                    ViewBag.Title = "All Estado";
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
        public ActionResult ConsultarUnEstado(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/estado/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    EstadoViewModel entityViewModel =
                        response.Content.ReadAsAsync<EstadoViewModel>().Result;
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
        public ActionResult ActualizarEstado(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/estado/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    EstadoViewModel entityViewModel = response.Content.ReadAsAsync<EstadoViewModel>().Result;
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
        public ActionResult ActualizarEstado(EstadoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/estado/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnEstado", new { id = entity.IdEstado });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarEstado(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/estado/" + id.ToString());
                response.EnsureSuccessStatusCode();
                EstadoViewModel entityViewModel = response.Content.ReadAsAsync<EstadoViewModel>().Result;
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarEstado(EstadoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/estado/" + entity.IdEstado.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoEstado", new { id = entity.IdEstado });
        }
        #endregion
    }
}
