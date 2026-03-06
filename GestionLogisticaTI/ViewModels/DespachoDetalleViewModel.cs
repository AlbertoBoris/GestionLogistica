using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class DespachoDetalleViewModel
    {
        public int IdPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public string NumeroGuia { get; set; }

        public List<DetalleItem> Detalles { get; set; }
    }
}