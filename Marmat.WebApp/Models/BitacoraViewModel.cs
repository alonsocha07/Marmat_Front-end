namespace Marmat.WebApp.Models
{
    public class BitacoraViewModel
    {
        public int ID_BITACORA { get; set; }
        public string DESCRIPCION { get; set; }
        public DateTime FECHA { get; set; }

        public int ID_USUARIO { get; set; }
        public List<UsuarioViewModel> usuarios { get; set; }
        public UsuarioViewModel usuario { get; set; }

    }
}
