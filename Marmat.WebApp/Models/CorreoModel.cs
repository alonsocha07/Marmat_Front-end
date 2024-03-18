namespace Marmat.WebApp.Models
{
    public class CorreoModel
    {
        public string nombre { get; set; }
        public string apellidos { get; set; } = null!;
        public string numero {get; set; } = null!;
        public string correo { get; set; } = null!;
        public string mensaje { get; set; } = null!;
    }
}
