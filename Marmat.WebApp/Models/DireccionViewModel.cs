namespace Marmat.WebApp.Models
{
    public class DireccionViewModel
    {
        public int IdDireccion { get; set; }
        public string NombreDireccion { get; set; } = null!;
        public int IdDistrito { get; set; }
        public List<DistritoViewModel> distritos { get; set; }
        public DistritoViewModel distrito { get; set; }
    }
}
