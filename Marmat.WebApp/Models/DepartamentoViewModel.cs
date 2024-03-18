namespace Marmat.WebApp.Models
{
    public class DepartamentoViewModel
    {
        public int IdDepartamento { get; set; }
        public int CantidadCuartos { get; set; }
        public int CantidadBanios { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Imagen { get; set; } = null!;
        public int? IdUsuario { get; set; }
        public int IdCondominio { get; set; }
        public List<UsuarioViewModel>? usuarios { get; set; }
        public UsuarioViewModel? usuario { get; set; }
        public List<CondominioViewModel> condominios { get; set; }
        public CondominioViewModel condominio { get; set; }
    }
}
