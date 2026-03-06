using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class PedidoDespachoViewModel
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public int CantidadItems { get; set; }
    }
}