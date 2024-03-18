using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class UsuarioHomeController : Controller
    {
        public ActionResult UsuarioHome()
        {
            if (HttpContext.Session.GetInt32("Rol") == 2)
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/boletin");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<BoletinViewModel> entity =
                    JsonConvert.DeserializeObject<List<BoletinViewModel>>(content);
                List<BoletinViewModel> listaFinal = new List<BoletinViewModel>();
                foreach (BoletinViewModel boletin in entity)
                {
                    if (boletin.FechaInicio <= DateTime.Today && boletin.FechaFinal >= DateTime.Today)
                    {
                        listaFinal.Add(boletin);
                    }
                }
                return View(listaFinal);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AvisoHome()
        {
            if (TempData["Message"]!=null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 2)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/aviso");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<AvisoViewModel> entity =
                        JsonConvert.DeserializeObject<List<AvisoViewModel>>(content);
                    List<AvisoViewModel> listaFinal = new List<AvisoViewModel>();
                    foreach (AvisoViewModel item in entity)
                    {
                        if (item.IdUsuario == HttpContext.Session.GetInt32("IdUsuario"))
                        {
                            listaFinal.Add(item);
                        }
                    }
                    return View(listaFinal);
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

        public ActionResult AvisoVisitas()
        {
            if (HttpContext.Session.GetInt32("Rol") == 2)
            {
                AvisoViewModel aviso = new AvisoViewModel();
                //Aquí en vez del 1 quemado, es necesario el ID del residente que tiene la sessión.
                aviso.IdUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;

                return View(aviso);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #region Feedback Correo
        public ActionResult MandarCorreoVisita(DateTime fecha, string comentario, int IdUsuario)
        {
            try
            {
                AvisoViewModel AvisoVisitasVMAux = new AvisoViewModel();

                AvisoVisitasVMAux.Fecha = fecha;
                AvisoVisitasVMAux.Comentario = comentario;
                AvisoVisitasVMAux.IdUsuario = IdUsuario;



                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Catalogo/Visitas/", AvisoVisitasVMAux);
                response.EnsureSuccessStatusCode();
                ServiceRepository serviceObj2 = new ServiceRepository();
                HttpResponseMessage response2 = serviceObj2.PostResponse("api/Aviso/", AvisoVisitasVMAux);
                response2.EnsureSuccessStatusCode();
                AvisoViewModel entityViewModel = response2.Content.ReadAsAsync<AvisoViewModel>().Result;
                return View("CorreoEnviado");
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
        [HttpGet]
        public ActionResult EliminarAviso(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 2)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/aviso/" + id.ToString());
                response.EnsureSuccessStatusCode();
                AvisoViewModel entityViewModel = response.Content.ReadAsAsync<AvisoViewModel>().Result;

                if (HttpContext.Session.GetInt32("IdUsuario") != entityViewModel.IdUsuario)
                {
                    TempData["Message"] = "Ese aviso no es suyo.";
                    return RedirectToAction("AvisoHome", "UsuarioHome");
                }
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarAviso(AvisoViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/aviso/" + entity.IdAviso.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("AvisoHome", new { id = entity.IdAviso });
        }


        #region Logout

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
