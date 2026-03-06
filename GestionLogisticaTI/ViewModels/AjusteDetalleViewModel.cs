using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class AjusteDetalleViewModel
    {
        [Required]
        public int IdProducto { get; set; }

        public string Producto { get; set; }

        [Required(ErrorMessage = "Debe ingresar cantidad de ajuste")]
        [Range(-1000, 1000, ErrorMessage = "Cantidad inválida")]
        public int CantidadAjuste { get; set; }
    }
}