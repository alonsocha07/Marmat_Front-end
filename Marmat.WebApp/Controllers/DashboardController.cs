using Marmat.WebApp.Helpers;
using Marmat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marmat.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Bitacora
        public IActionResult Bitacora()
        {
            return View();
        }

        public JsonResult GetReporteBitacora()
        {
            BitacoraReporteHelper serie = new BitacoraReporteHelper();
            return Json(serie.GetReporteBitacora());
        }
        #endregion

        #region Condominio
        public IActionResult Condominio()
        {
            return View();
        }

        public JsonResult ObtenerReporteCondominio()
        {
            CondominioReporteHelper serie = new CondominioReporteHelper();
            return Json(serie.GetDataReporteCondominio());
        }
        #endregion
        
        #region Departamento
        public IActionResult Departamento()
        {
            return View();
        }

        public JsonResult ObtenerReporteDepartamentoCuartos()
        {
            DepartamentoReporteHelper serie = new DepartamentoReporteHelper();
            return Json(serie.GetDataReporteDepartamentoCuartos());
        }
        public JsonResult ObtenerReporteDepartamentoBanios()
        {
            DepartamentoReporteHelper serie = new DepartamentoReporteHelper();
            return Json(serie.GetDataReporteDepartamentoBanios());
        }
        #endregion

        #region Usuario
        public IActionResult Usuario()
        {
            return View();
        }

        public JsonResult ObtenerReporteUsuario()
        {
            UsuarioReporteHelper serie = new UsuarioReporteHelper();
            return Json(serie.GetDataReporteUsuario());
        }
        #endregion
    }
}
