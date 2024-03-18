using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class TicketController : Controller
    {
        #region Create
        public ActionResult RegistrarTicket()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                TicketViewModel ticket = new TicketViewModel();
                ticket.departamentos = this.ObtenerDepartamentos();
                ticket.usuarios = this.ObtenerUsuarios();
                ticket.estados = this.ObtenerEstados();
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarTicket(TicketViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/ticket/", entity);
                response.EnsureSuccessStatusCode();
                TicketViewModel entityViewModel = response.Content.ReadAsAsync<TicketViewModel>().Result;
                return RedirectToAction("ConsultarUnTicket", new { id = entityViewModel.IdTicket });
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
        private List<DepartamentoViewModel> ObtenerDepartamentos()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private DepartamentoViewModel ObtenerDepartamento(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/departamento/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                DepartamentoViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<DepartamentoViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private List<UsuarioViewModel> ObtenerUsuarios()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<UsuarioViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private UsuarioViewModel ObtenerUsuario(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                UsuarioViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<UsuarioViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private List<EstadoViewModel> ObtenerEstados()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/estado");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<EstadoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<EstadoViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private EstadoViewModel ObtenerEstado(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/estado/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                EstadoViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<EstadoViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult MantenimientoTicket()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/ticket");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<TicketViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<TicketViewModel>>(content);
                    foreach (TicketViewModel item in entityViewModel)
                    {
                        item.departamento = this.ObtenerDepartamento(item.IdDepartamento);
                        item.usuario = this.ObtenerUsuario(item.IdUsuario);
                        item.estado = this.ObtenerEstado(item.IdEstado);

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
        public ActionResult ConsultarUnTicket(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/ticket/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    TicketViewModel entityViewModel =
                        response.Content.ReadAsAsync<TicketViewModel>().Result;
                    entityViewModel.departamento = this.ObtenerDepartamento(entityViewModel.IdDepartamento);
                    entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdUsuario);
                    entityViewModel.estado = this.ObtenerEstado(entityViewModel.IdEstado);
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
        public ActionResult ActualizarTicket(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/ticket/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    TicketViewModel entityViewModel = response.Content.ReadAsAsync<TicketViewModel>().Result;
                    entityViewModel.departamentos = this.ObtenerDepartamentos();
                    entityViewModel.usuarios = this.ObtenerUsuarios();
                    entityViewModel.estados = this.ObtenerEstados();
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
        public ActionResult ActualizarTicket(TicketViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/ticket/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnTicket", new { id = entity.IdTicket });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarTicket(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/ticket/" + id.ToString());
                response.EnsureSuccessStatusCode();
                TicketViewModel entityViewModel = response.Content.ReadAsAsync<TicketViewModel>().Result;
                entityViewModel.departamento = this.ObtenerDepartamento(entityViewModel.IdDepartamento);
                entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.IdUsuario);
                entityViewModel.estado = this.ObtenerEstado(entityViewModel.IdEstado);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }           
        }

        [HttpPost]
        public ActionResult EliminarTicket(TicketViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/ticket/" + entity.IdTicket.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoTicket", new { id = entity.IdTicket });
        }
        #endregion
    }
}
