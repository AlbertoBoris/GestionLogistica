using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class MovimientoHistorialViewModel
    {
        public int IdMovimiento { get; set; }

        public DateTime Fecha { get; set; }

        public string TipoMovimiento { get; set; }

        public string NumeroDocumento { get; set; }

        public string Motivo { get; set; }

        public string Producto { get; set; }

        public int Cantidad { get; set; }

        public string Usuario { get; set; }
    }
}