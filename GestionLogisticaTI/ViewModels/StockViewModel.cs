using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class StockViewModel
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string Ubicacion { get; set; }
        public string EstadoStock { get; set; }

        public bool EsCritico
        {
            get { return EstadoStock == "Crítico"; }
        }
    }
}