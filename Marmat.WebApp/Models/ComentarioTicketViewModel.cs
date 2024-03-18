namespace Marmat.WebApp.Models
{
    public class ComentarioTicketViewModel
    {
        public int IdComentarioTicket { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = null!;
        public int IdTicket { get; set; }
        public List<TicketViewModel> tickets { get; set; }
        public TicketViewModel ticket { get; set; }
    }
}
