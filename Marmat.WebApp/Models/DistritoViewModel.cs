namespace Marmat.WebApp.Models
{
    public class DistritoViewModel
    {
        public int IdDistrito { get; set; }
        public string NombreDistrito { get; set; } = null!;
        public int IdCanton { get; set; }
        public CantonViewModel canton { get; set; }
        public List<CantonViewModel> cantones { get; set; }
    }
}
