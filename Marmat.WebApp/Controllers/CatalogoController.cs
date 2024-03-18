using FrontEnd.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Marmat.WebApp.Controllers
{
    public class CatalogoController : Controller
    {
        #region Catalogo
        public ActionResult Catalogo()
        {

            try
            {
                ServiceRepository Repository = new ServiceRepository();
                HttpResponseMessage responseMessage = Repository.GetResponse("api/catalogo");
                responseMessage.EnsureSuccessStatusCode();
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                List<CatalogoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CatalogoViewModel>>(content);

               

                ViewBag.Title = "All Condominio";
               
                return View(entityViewModel);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult CatalogoUsuario()
        {

            try
            {
                if (HttpContext.Session.GetInt32("Rol") != null)
                {

                    ServiceRepository Repository = new ServiceRepository();
                    HttpResponseMessage responseMessage = Repository.GetResponse("api/catalogo");
                    responseMessage.EnsureSuccessStatusCode();
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    List<CatalogoViewModel> entityViewModel =
                        JsonConvert.DeserializeObject<List<CatalogoViewModel>>(content);



                    ViewBag.Title = "All Condominio";

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

        #region Busqueda Condominio
        public ActionResult CondominioEspecifico(int id)
        {

            try
            {
                //Se busca la informacion del condominio
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/catalogo/" + id.ToString());
                response.EnsureSuccessStatusCode();
                CatalogoViewModel entityViewModel =
                    response.Content.ReadAsAsync<CatalogoViewModel>().Result;

                //Se buscan las imagenes en la carpeta que coincida con el id como string y se pasa a int
                string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                int cantidad = dirs.Length;

                //Se buscan los Departamentos relacionados al condominio

                HttpResponseMessage responseDepartamento = serviceObj.GetResponse("api/Departamento/GetByIdCondominio?idCondominio=" + id.ToString());
                responseDepartamento.EnsureSuccessStatusCode();
                
                var content = responseDepartamento.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> ListaDeparta =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);

                //Se pasan datos por ViewBag
                ViewBag.CantImg = cantidad;
                ViewBag.ListaDep = ListaDeparta;


                return View(entityViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CondominioEspecificoUsuario(int id)
        {

            try
            {
                //Se busca la informacion del condominio
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/catalogo/" + id.ToString());
                response.EnsureSuccessStatusCode();
                CatalogoViewModel entityViewModel =
                    response.Content.ReadAsAsync<CatalogoViewModel>().Result;

                //Se buscan las imagenes en la carpeta que coincida con el id como string y se pasa a int
                string[] dirs = Directory.GetFiles("wwwroot/images/Condominio/Condominio" + id.ToString());
                int cantidad = dirs.Length;

                //Se buscan los Departamentos relacionados al condominio

                HttpResponseMessage responseDepartamento = serviceObj.GetResponse("api/departamento/GetByNameIdCondominio?idCondominio=" + id.ToString());
                responseDepartamento.EnsureSuccessStatusCode();

                var content = responseDepartamento.Content.ReadAsStringAsync().Result;
                List<DepartamentoViewModel> ListaDeparta =
                    JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(content);

                //Se pasan datos por ViewBag
                ViewBag.CantImg = cantidad;
                ViewBag.ListaDep = ListaDeparta;


                return View(entityViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Feedback Correo
        public ActionResult MandarCorreo(string nombre, string apellidos, string numero, string correo, string mensaje, string NombreCondominio)
        {
            try
            {
                CatalogoViewModel CatalogoVMAux = new CatalogoViewModel();

                CatalogoVMAux.nombre = nombre;
                CatalogoVMAux.apellidos = apellidos;
                CatalogoVMAux.numero = numero;
                CatalogoVMAux.correo = correo;
                CatalogoVMAux.mensaje = mensaje;
                CatalogoVMAux.NombreCondominio = NombreCondominio;

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Catalogo/", CatalogoVMAux);
                response.EnsureSuccessStatusCode();
                // CatalogoViewModel entityViewModel = response.Content.ReadAsAsync<CatalogoViewModel>().Result;
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

        #region Filtro Catalogo
        public ActionResult FiltrarNombre(string buscarstring)
        {

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/Catalogo/GetByNameCatalogo?name=" + buscarstring);
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                List<CatalogoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CatalogoViewModel>>(content);

                return View(entityViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult FiltrarProvincia(string buscarstring)
        {

            try
            {

                if (buscarstring == "0")
                {
                    return RedirectToAction("Catalogo", "Catalogo");
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("api/Catalogo/GetByProvinciaCatalogo?provinciaid=" + buscarstring);
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                List<CatalogoViewModel> entityViewModel =
                    JsonConvert.DeserializeObject<List<CatalogoViewModel>>(content);

                return View(entityViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

