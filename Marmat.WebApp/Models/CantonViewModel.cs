namespace Marmat.WebApp.Models
{
    public class CantonViewModel
    {
        public int IdCanton { get; set; }
        public string NombreCanton { get; set; } = null!;
        public int IdProvincia { get; set; }
        public List<ProvinciaViewModel> provincias { get; set; }
        public ProvinciaViewModel provincia { get; set; }
    }
}
