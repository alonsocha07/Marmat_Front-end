using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Marmat.WebApp.Models
{
    public class CondominioViewModel
    {
        public int IdCondominio { get; set; }
        public string NombreCondominio { get; set; } = null!;
        public int Vacantes { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Imagen { get; set; } = null!;
        public int IdDireccion { get; set; }


        public List<IFormFile> Img { get; set; } = null!;
        public IList<string> IList { get; set; } = null!;
       


        public List<DireccionViewModel> direcciones { get; set; }
        public DireccionViewModel direccion { get; set; }
    }
}
