using FrontEnd.Helpers;
using Marmat.DML;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class ReservaController : Controller
    {
        #region Create
        public ActionResult RegistrarReserva()
        {
            if (HttpContext.Session.GetInt32("Rol") != null)
            {
                ReservaViewModel reserva = new ReservaViewModel();
                reserva.areascomunes = this.ObtenerAreasComunes();
                return View(reserva);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarReserva(ReservaViewModel entity)
        {
            try
            {
                if (!DateCheck(entity))
                {
                    if (entity.IdAreacomun == null)
                        entity.IdAreacomun = 1;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/Reservas/", entity);
                    response.EnsureSuccessStatusCode();
                    ReservaViewModel entityViewModel = response.Content.ReadAsAsync<ReservaViewModel>().Result;
                    return RedirectToAction("MantenimientoReservaCliente", new { id = entity.IdReserva });
                }
                else
                {
                    ReservaViewModel item = new ReservaViewModel();
                    item.areascomunes = this.ObtenerAreasComunes();
                    ViewBag.Message = "El area comun esta reservada ese dia";
                    return View(item);
                }
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
        private List<AreacomunViewModel> ObtenerAreasComunes()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/AreaComun");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<AreacomunViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<AreacomunViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private AreacomunViewModel ObtenerAreaComun(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/AreaComun/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                AreacomunViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<AreacomunViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        

        public ActionResult MantenimientoReserva()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") != null)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/Reservas");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<ReservaViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<ReservaViewModel>>(content);
                    foreach (ReservaViewModel item in entityViewModel)
                    {
                        item.areacomun = this.ObtenerAreaComun(item.IdAreacomun.Value);
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
        public ActionResult ConsultarUnaReserva(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/Reservas/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ReservaViewModel entityViewModel =
                        response.Content.ReadAsAsync<ReservaViewModel>().Result;
                    entityViewModel.areacomun = this.ObtenerAreaComun(entityViewModel.IdAreacomun.Value);
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
        public ActionResult ActualizarReserva(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/Reservas/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ReservaViewModel entityViewModel = response.Content.ReadAsAsync<ReservaViewModel>().Result;
                    entityViewModel.areascomunes = this.ObtenerAreasComunes();
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
        public ActionResult ActualizarReserva(ReservaViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/Reservas/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnaReserva", new { id = entity.IdReserva });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarReserva(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/Reservas/" + id.ToString());
                response.EnsureSuccessStatusCode();
                ReservaViewModel entityViewModel = response.Content.ReadAsAsync<ReservaViewModel>().Result;
                entityViewModel.areacomun = this.ObtenerAreaComun(entityViewModel.IdAreacomun.Value);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }           
        }

        [HttpPost]
        public ActionResult EliminarReserva(ReservaViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/Reservas/" + entity.IdReserva.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoReserva", new { id = entity.IdReserva });
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool DateCheck(ReservaViewModel entity)
        {

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/Reservas/DateCheck", entity);
            response.EnsureSuccessStatusCode();
            bool DateExist = response.Content.ReadAsAsync<bool>().Result;
            return DateExist;

        }


        public ActionResult MantenimientoReservaCliente()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") != null)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/ReservaCliente");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<ReservaClienteViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<ReservaClienteViewModel>>(content);

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
    }
}
