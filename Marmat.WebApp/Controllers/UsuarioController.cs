using FrontEnd.Helpers;
using Marmat.DML;
using Marmat.WebApp.Models;
using Marmat.WebApp.ReCaptcha;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class UsuarioController : Controller
    {


        #region Create
        public ActionResult RegistrarUsuario()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                UsuarioViewModel entity = new UsuarioViewModel();
                entity.roles = this.ObtenerRoles();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarUsuario(UsuarioViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/usuario/", entity);
                response.EnsureSuccessStatusCode();
                UsuarioViewModel entityViewModel = response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                return RedirectToAction("ConsultarUnUsuario", new { id = entityViewModel.IdUsuario });
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

        private List<RolViewModel> ObtenerRoles()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/rol");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<RolViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<RolViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private RolViewModel ObtenerRol(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/rol/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                RolViewModel entityViewModel =
                    responseMessage.Content.ReadAsAsync<RolViewModel>().Result;
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoUsuario()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/usuario");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<UsuarioViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                    foreach (UsuarioViewModel item in entityViewModel)
                    {
                        item.rol = this.ObtenerRol(item.IdRol);
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
        public ActionResult ConsultarUnUsuario(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/usuario/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    UsuarioViewModel entityViewModel =
                        response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                    entityViewModel.rol = this.ObtenerRol(entityViewModel.IdRol);
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
        public ActionResult ActualizarUsuario(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/usuario/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    UsuarioViewModel entityViewModel = response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                    entityViewModel.roles = this.ObtenerRoles();
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
        public ActionResult ActualizarUsuario(UsuarioViewModel entity)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("api/usuario/", entity);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("ConsultarUnUsuario", new { id = entity.IdUsuario });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarUsuario(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/usuario/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    UsuarioViewModel entityViewModel = response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                    entityViewModel.rol = this.ObtenerRol(entityViewModel.IdRol);
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
        public ActionResult EliminarUsuario(UsuarioViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/usuario/" + entity.IdUsuario.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoUsuario", new { id = entity.IdUsuario });
        }
        #endregion

        #region Login/Registrar
        public ActionResult LoginUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> LoginUsuario(UsuarioViewModel entity)
        {
            try
            {

                var captchaResult = await VerifyToken(entity.token);

                 if (!captchaResult)
                {
                    return View("ErrorReCaptcha");
                }

                entity.IdUsuario = 0;
                entity.NumeroTel = 0;
                entity.IdRol = 0;
                entity.Correo = "a";
                entity.Nombre = "a";
                entity.PrimerApellido = "a";
                entity.SegundoApellido = "a";
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/usuario/Login", entity);
                response.EnsureSuccessStatusCode();
                UsuarioViewModel userExist = response.Content.ReadAsAsync<UsuarioViewModel>().Result;

                if (userExist.IdUsuario != 0)
                {
                    HttpContext.Session.SetInt32("IdUsuario", userExist.IdUsuario);
                    HttpContext.Session.SetString("NombreUsuario", userExist.NombreUsuario);
                    HttpContext.Session.SetString("Correo", userExist.Correo);
                    HttpContext.Session.SetString("Nombre", userExist.Nombre);
                    HttpContext.Session.SetString("PrimerApellido", userExist.PrimerApellido);
                    HttpContext.Session.SetString("SegundoApellido", userExist.SegundoApellido);
                    HttpContext.Session.SetInt32("NumeroTel", userExist.NumeroTel);
                    HttpContext.Session.SetInt32("Rol", userExist.IdRol);

                    if (HttpContext.Session.GetInt32("Rol") == 1)
                    {
                        return RedirectToAction("Administrador", "Administrador");
                    }
                    else if (HttpContext.Session.GetInt32("Rol") == 2)
                    {
                        return RedirectToAction("UsuarioHome", "UsuarioHome");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    UsuarioViewModel item = new UsuarioViewModel();
                    ViewBag.Message = "Usuario o Contraseña incorrectos";
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

        public ActionResult RegistrarNuevoUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarNuevoUsuario(UsuarioViewModel entity)
        {
            var captchaResult = await VerifyToken(entity.token);

            if (!captchaResult)
            {
                return View("ErrorReCaptcha");
            }
            entity.IdUsuario = 0;
            entity.IdRol = 2;
            try
            {
                if (entity.Pass.Length < 8)
                {
                    UsuarioViewModel item = new UsuarioViewModel();
                    ViewBag.Message = "tiene que tener un min de 8 caracteres";
                    return View(item);
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/usuario/", entity);
                response.EnsureSuccessStatusCode();
                UsuarioViewModel entityViewModel = response.Content.ReadAsAsync<UsuarioViewModel>().Result;
                return RedirectToAction("LoginUsuario", "Usuario");
            }
            catch (HttpRequestException)
            {
                UsuarioViewModel item = new UsuarioViewModel();
                ViewBag.Message = "No pueden haber campos vacios";
                return View(item);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Cambio Pass

        public ActionResult CambioContraseña()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") != null)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/usuario/" + HttpContext.Session.GetInt32("IdUsuario").ToString());
                    response.EnsureSuccessStatusCode();
                    UsuarioViewModel entityViewModel = response.Content.ReadAsAsync<UsuarioViewModel>().Result;
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
        public ActionResult CambioContraseña(UsuarioViewModel entity)
        {
            entity.IdUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;
            entity.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            entity.Correo = HttpContext.Session.GetString("Correo");
            entity.Nombre = HttpContext.Session.GetString("Nombre");
            entity.PrimerApellido = HttpContext.Session.GetString("PrimerApellido");
            entity.SegundoApellido = HttpContext.Session.GetString("SegundoApellido");
            entity.NumeroTel = HttpContext.Session.GetInt32("NumeroTel").Value;
            entity.IdRol = HttpContext.Session.GetInt32("Rol").Value;

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("api/usuario/", entity);
                response.EnsureSuccessStatusCode();
                return View("CambioContraseñaExitoso");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret=6Ldpr4IjAAAAABn_NT7ACSMEygfMkRm7FPQDG1F2&response={token}";

                using (var client = new HttpClient())
                {

                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }

                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    return googleResult.Success && googleResult.score >= 0.5;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //ESTE METODO INTENTA LLAMAR AL CAPTCHA SINCRONO    
        /* public GoogleCaptchaResponse VerifyToken(string token)
         {
             try
             {
                 var url = $"https://www.google.com/recaptcha/api/siteverify?secret=6Ldpr4IjAAAAABn_NT7ACSMEygfMkRm7FPQDG1F2&response={token}";

                 using (var client = new HttpClient())
                 {

                     var httpResult =client.GetAsync(url);


                     var responseString = httpResult.Result;

                     var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                     return googleResult;
                 }
             }
             catch (Exception)
             {

                 throw;
             }
         }*/



    }
}
