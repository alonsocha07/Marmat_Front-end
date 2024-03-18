namespace Marmat.WebApp.Models
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public int NumeroTel { get; set; }
        public int IdRol { get; set; }
        public RolViewModel rol { get; set; }
        public List<RolViewModel> roles { get; set; }


        //Recaptcha
        public string token { get; set; } 
    }
}
