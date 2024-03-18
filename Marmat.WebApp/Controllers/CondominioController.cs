#region Using
using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace Marmat.WebApp.Controllers
{
    public class CondominioController : Controller
    {
        #region Contructor
        private readonly IWebHostEnvironment _environment;
        public CondominioController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        #endregion

        #region Create
        public ActionResult RegistrarCondominio()
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {
                CondominioViewModel item = new CondominioViewModel();
                item.direcciones = this.ObtenerDirecciones();
                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCondominio(CondominioViewModel entity, List<IFormFile> upload)
        {
            try
            {
                if (entity.Imagen.Contains("<iframe src="))
                {
                    entity.Imagen = RecortarLinkMaps(entity.Imagen);
                }


                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/condominio/", entity);
                response.EnsureSuccessStatusCode();
                CondominioViewModel entityViewModel = response.Content.ReadAsAsync<CondominioViewModel>().Result;

                CrearCarpeta(entityViewModel.IdCondominio, upload);

                return RedirectToAction("ConsultarUnCondominio", new { id = entityViewModel.IdCondominio });
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
        private List<DireccionViewModel> ObtenerDirecciones()
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/direccion");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<DireccionViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<DireccionViewModel>>(content);
                return entityViewModel;
            }
            catch
            {
                return null;
            }
        }
        private DireccionViewModel ObtenerDireccion(int id)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/direccion/" + id.ToString());
                responseMessage.EnsureSuccessStatusCode();
                DireccionViewModel direccionViewModel =
                    responseMessage.Content.ReadAsAsync<DireccionViewModel>().Result;
                return direccionViewModel;
            }
            catch
            {
                return null;
            }
        }
        public ActionResult MantenimientoCondominio()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {

                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/condominio");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<CondominioViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<CondominioViewModel>>(content);
                    foreach (CondominioViewModel item in entityViewModel)
                    {
                        item.direccion = this.ObtenerDireccion(item.IdDireccion);
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
        public ActionResult ConsultarUnCondominio(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/condominio/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    CondominioViewModel entityViewModel =
                        response.Content.ReadAsAsync<CondominioViewModel>().Result;
                    entityViewModel.direccion = this.ObtenerDireccion(entityViewModel.IdDireccion);

                    string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                    int cantidad = dirs.Length;

                    ViewBag.CantImg = cantidad;

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
        public ActionResult ActualizarCondominio(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.GetResponse("api/condominio/" + id.ToString());
                    response.EnsureSuccessStatusCode();
                    CondominioViewModel entityViewModel = response.Content.ReadAsAsync<CondominioViewModel>().Result;
                    entityViewModel.direcciones = this.ObtenerDirecciones();
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
        public ActionResult ActualizarCondominio(CondominioViewModel entity)
        {
            if (entity.Imagen.Contains("<iframe src="))
            {
                entity.Imagen = RecortarLinkMaps(entity.Imagen);
            }
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/Condominio/", entity);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("ConsultarUnCondominio", new { id = entity.IdCondominio });
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult EliminarCondominio(int id)
        {
            if (HttpContext.Session.GetInt32("Rol") == 1)
            {

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/Condominio/" + id.ToString());
                response.EnsureSuccessStatusCode();
                CondominioViewModel entityViewModel = response.Content.ReadAsAsync<CondominioViewModel>().Result;
                entityViewModel.direccion = this.ObtenerDireccion(entityViewModel.IdDireccion);
                return View(entityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EliminarCondominio(CondominioViewModel entity)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/Condominio/" + entity.IdCondominio.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("MantenimientoCondominio", new { id = entity.IdCondominio });
        }
        #endregion

        #region Recorte Google Maps
        public string RecortarLinkMaps(string link)
        {
            try
            {
                char[] delimitador = { '"' };
                string[] linkRecortado = link.Split(delimitador);
                return linkRecortado[1];
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Carpeta para condominio
        public bool CrearCarpeta(int id, List<IFormFile> upload)
        {
            try
            {
                var WebRootPathAux = _environment.WebRootPath;
                WebRootPathAux += "\\images\\Condominio\\Condominio" + id;
                if (Directory.Exists(WebRootPathAux))
                {
                    return false;
                }
                else
                {
                    Directory.CreateDirectory(WebRootPathAux);
                    try
                    {
                        int contador = 1;
                        foreach (var archivo in upload)
                        {
                            string fileName = "Ejemplo" + contador.ToString() + ".png";
                            using (var fileStream = new FileStream(Path.Combine(WebRootPathAux, fileName), FileMode.Create))
                            {
                                archivo.CopyTo(fileStream);
                                contador += 1;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Imagenes Carousel
        public ActionResult SubirImagen(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                    int cantidad = dirs.Length;
                    ViewBag.CantImg = cantidad;
                    ViewBag.IdCondominio = id.ToString();
                    return View();
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
        public ActionResult SubirImagen(List<IFormFile> upload, int id)
        {
            try
            {
                string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                int cantidad = dirs.Length;
                ViewBag.CantImg = cantidad;
                var WebRootPathAux = _environment.WebRootPath;
                WebRootPathAux += "\\images\\Condominio\\Condominio" + id;
                int contador = cantidad + 1;
                foreach (var archivo in upload)
                {
                    string fileName = "Ejemplo" + contador.ToString() + ".png";
                    using (var fileStream = new FileStream(Path.Combine(WebRootPathAux, fileName), FileMode.Create))
                    {
                        archivo.CopyTo(fileStream);
                        contador += 1;
                    }
                }
                return RedirectToAction("ConsultarUnCondominio", new { id = id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EliminarImagen(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Rol") == 1)
                {
                    string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                    int cantidad = dirs.Length;

                    ViewBag.CantImg = cantidad;
                    ViewBag.IdCondominio = id.ToString();
                    return View();
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

        public ActionResult Eliminarimagenn(List<string> IList, int id)
        {
            try
            {
                string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                int cantidad = dirs.Length;
                for (int i = 1; i < cantidad + 1; i++)
                {
                    if (IList.Contains(i.ToString()))
                    {
                        string ruta = "wwwroot\\images\\Condominio\\Condominio" + id.ToString() + "\\Ejemplo" + i + ".png";
                        System.IO.File.Delete(ruta);
                    }
                }
                var files = Directory.EnumerateFiles("wwwroot\\images\\Condominio\\Condominio" + id.ToString());
                int contador = 1;
                foreach (var archivo in files)
                {
                    string rutaDestino = "wwwroot\\images\\Condominio\\Condominio" + id.ToString() + "\\Ejemplo" + contador.ToString() + ".png";
                    if (archivo != rutaDestino)
                    {
                        Directory.Move(archivo, rutaDestino);
                    }
                    contador += 1;
                }
                return RedirectToAction("ConsultarUnCondominio", new { id = id });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}