using Microsoft.AspNetCore.Mvc;

namespace Marmat.WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        public ActionResult Administrador()
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
    }
}
