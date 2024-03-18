using Marmat.WebApp.Models;
using System;
using System.Collections.Generic;

namespace Marmat.DML
{
    public partial class ReservaViewModel
    {
        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public int? IdAreacomun { get; set; }

        public List<AreacomunViewModel> areascomunes { get; set; }
        public AreacomunViewModel areacomun { get; set; }
    }
}
