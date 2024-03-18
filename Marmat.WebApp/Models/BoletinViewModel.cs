
using Newtonsoft.Json.Serialization;

namespace Marmat.WebApp.Models
{
    public class BoletinViewModel
    {
        public int IdBoletin { get; set; }
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }
        public string Comentario { get; set; } = null!;
    }
}
