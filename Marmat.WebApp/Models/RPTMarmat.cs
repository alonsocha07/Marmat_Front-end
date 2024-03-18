using FrontEnd.Helpers;
using Newtonsoft.Json;

namespace Marmat.WebApp.Models
{
    #region Bitacora
    public class ReporteBitacora
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }

        public ReporteBitacora(string name, double y, bool sliced = false, bool selected = false)
        {
            this.name = name;
            this.y = y;
            this.sliced = sliced;
            this.selected = selected;
        }
    }
    #endregion

    #region Usuario
    public class ReporteUsuario
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }

        public ReporteUsuario()
        {

        }

        public ReporteUsuario(string name, double y, bool sliced = false, bool selected = false)
        {
            this.name = name;
            this.y = y;
            this.sliced = sliced;
            this.selected = selected;
        }
    }

    public class ReporteUsuarioRol
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }

        public ReporteUsuarioRol()
        {

        }

        public ReporteUsuarioRol(string name, double y, bool sliced = false, bool selected = false)
        {
            this.name = name;
            this.y = y;
            this.sliced = sliced;
            this.selected = selected;
        }
    }
    #endregion

}