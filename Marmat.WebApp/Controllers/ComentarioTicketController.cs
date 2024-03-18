using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class ComentarioTicketController : Controller
    {
        #region Create
        public ActionResult RegistrarComentarioTicket()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ComentarioTicketViewModel item = new ComentarioTicketViewModel();
                item.tickets = this.ObtenerTickets();
                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarComentarioTicket(ComentarioTicketViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/ComentarioTicket/", entity);
                response.EnsureSuccessStatusCode();
                ComentarioTicketViewModel entityViewModel = response.Content.ReadAsAsync<ComentarioTicketViewModel>().Result;
                return RedirectToAction("ConsultarUnComentarioTicket", new { id = entityViewModel.IdComentarioTicket });
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

        private List<TicketViewModel> ObtenerTickets()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/ticket");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<TicketViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<TicketViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private TicketViewModel ObtenerTicket(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/ticket/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                TicketViewModel ticketViewModel =
                    responseMessage.Content.ReadAsAsync<TicketViewModel>().Result;
                //ViewBag.Title = "All Products";
                return ticketViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoComentarioTicket()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/ComentarioTicket");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<ComentarioTicketViewModel> entity =
                        JsonConvert.DeserializeObject<List<ComentarioTicketViewModel>>(content);
                    foreach (ComentarioTicketViewModel item in entity)
                    {
                        item.ticket = this.ObtenerTicket(item.IdTicket);
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
        public ActionResult ConsultarUnComentarioTicket(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/ComentarioTicket/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ComentarioTicketViewModel entityViewModel =
                        response.Content.ReadAsAsync<ComentarioTicketViewModel>().Result;
                    entityViewModel.ticket = this.ObtenerTicket(entityViewModel.IdTicket);
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
        public ActionResult ActualizarComentarioTicket(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/ComentarioTicket/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    ComentarioTicketViewModel entityViewModel = response.Content.ReadAsAsync<ComentarioTicketViewModel>().Result;
                    entityViewModel.tickets = this.ObtenerTickets();
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
        public ActionResult ActualizarComentarioTicket(ComentarioTicketViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/ComentarioTicket/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnComentarioTicket", new { id = entity.IdComentarioTicket });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarComentarioTicket(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/ComentarioTicket/" + id.ToString());
                response.EnsureSuccessStatusCode();
                ComentarioTicketViewModel entityViewModel = response.Content.ReadAsAsync<ComentarioTicketViewModel>().Result;
                entityViewModel.ticket = this.ObtenerTicket(entityViewModel.IdTicket);

                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarComentarioTicket(ComentarioTicketViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/ComentarioTicket/" + entity.IdComentarioTicket.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoComentarioTicket", new { id = entity.IdComentarioTicket });
        }
        #endregion
    }
}
