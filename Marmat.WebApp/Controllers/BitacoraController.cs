using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class BitacoraController : Controller
    {
        #region Create
        public ActionResult RegistrarBitacora()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                BitacoraViewModel entityViewModel = new BitacoraViewModel();
                entityViewModel.usuarios = this.ObtenerUsuarios();
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarBitacora(BitacoraViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/bitacora/", entity);
                response.EnsureSuccessStatusCode();
                BitacoraViewModel entityViewModel = response.Content.ReadAsAsync<BitacoraViewModel>().Result;
                return RedirectToAction("ConsultarUnaBitacora", new { id = entityViewModel.ID_BITACORA });
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
                HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario/"+id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                UsuarioViewModel usuarioViewModel =
                    responseMessage.Content.ReadAsAsync<UsuarioViewModel>().Result;
                //ViewBag.Title = "All Products";
                return usuarioViewModel;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult MantenimientoBitacora()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/bitacora");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<BitacoraViewModel> entity =
                        JsonConvert.DeserializeObject<List<BitacoraViewModel>>(content);
                    foreach (BitacoraViewModel item in entity)
                    {
                        item.usuario = this.ObtenerUsuario(item.ID_USUARIO);
                    }
                    ViewBag.Title = "All Categories";
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
        public ActionResult ConsultarUnaBitacora(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/bitacora/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    BitacoraViewModel entityViewModel =
                        response.Content.ReadAsAsync<BitacoraViewModel>().Result;
                    entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.ID_USUARIO);
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
        public ActionResult ActualizarBitacora(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/bitacora/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    BitacoraViewModel entityViewModel = response.Content.ReadAsAsync<BitacoraViewModel>().Result;
                    entityViewModel.usuarios = this.ObtenerUsuarios();
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
        public ActionResult ActualizarBitacora(BitacoraViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/bitacora/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnaBitacora", new { id = entity.ID_BITACORA });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarBitacora(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/bitacora/" + id.ToString());
                response.EnsureSuccessStatusCode();
                BitacoraViewModel entityViewModel = response.Content.ReadAsAsync<BitacoraViewModel>().Result;
                entityViewModel.usuario = this.ObtenerUsuario(entityViewModel.ID_USUARIO);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarBitacora(BitacoraViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/bitacora/" + entity.ID_BITACORA.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoBitacora", new { id = entity.ID_BITACORA });
        }
        #endregion
    }
}
