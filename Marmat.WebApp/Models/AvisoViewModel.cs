namespace Marmat.WebApp.Models
{
    public class AvisoViewModel
    {
        public int IdAviso { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = null!;
        public UsuarioViewModel usuario { get; set; }
        public List<UsuarioViewModel> usuarios { get; set; }
    }
}
