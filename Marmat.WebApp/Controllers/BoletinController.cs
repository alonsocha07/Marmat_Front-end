using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class BoletinController : Controller
    {
        #region Create
        public ActionResult RegistrarBoletin()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                BoletinViewModel item = new BoletinViewModel();

                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarBoletin(BoletinViewModel entity)
        {
            try
            {
                if (entity.FechaInicio > entity.FechaFinal)
                {
                    BoletinViewModel item = new BoletinViewModel();
                    ViewBag.Message = "La fecha de inicio no puede ser mayor a la fecha final.";
                    return View(item);
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/boletin/", entity);
                response.EnsureSuccessStatusCode();
                BoletinViewModel entityViewModel = response.Content.ReadAsAsync<BoletinViewModel>().Result;
                return RedirectToAction("ConsultarUnBoletin", new { id = entityViewModel.IdBoletin });
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Read

        public ActionResult MantenimientoBoletin()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/boletin");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<BoletinViewModel> entityViewModels =
                        JsonConvert.DeserializeObject<List<BoletinViewModel>>(content);

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
        public ActionResult ConsultarUnBoletin(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/boletin/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    BoletinViewModel entityViewModel =
                        response.Content.ReadAsAsync<BoletinViewModel>().Result;

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
        public ActionResult ActualizarBoletin(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {

                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/boletin/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    BoletinViewModel entityViewModel = response.Content.ReadAsAsync<BoletinViewModel>().Result;

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
        public ActionResult ActualizarBoletin(BoletinViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/boletin/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnBoletin", new { id = entity.IdBoletin });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarBoletin(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/boletin/" + id.ToString());
                response.EnsureSuccessStatusCode();
                BoletinViewModel entityViewModel = response.Content.ReadAsAsync<BoletinViewModel>().Result;

                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarBoletin(BoletinViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/boletin/" + entity.IdBoletin.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoBoletin", new { id = entity.IdBoletin });
        }
        #endregion
    }
}
