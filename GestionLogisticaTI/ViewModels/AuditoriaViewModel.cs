using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class AuditoriaViewModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string Ubicacion { get; set; }

        public int? StockFisico { get; set; }
    }
}