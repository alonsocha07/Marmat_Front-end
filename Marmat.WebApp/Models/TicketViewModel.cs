namespace Marmat.WebApp.Models
{
    public class TicketViewModel
    {
        public int IdTicket { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public int IdDepartamento { get; set; }
        public UsuarioViewModel usuario { get; set; }
        public List<UsuarioViewModel> usuarios { get; set; }
        public DepartamentoViewModel departamento { get; set; }
        public List<DepartamentoViewModel> departamentos { get; set; }
        public EstadoViewModel estado { get; set; }
        public List<EstadoViewModel> estados { get; set; }
    }
}
