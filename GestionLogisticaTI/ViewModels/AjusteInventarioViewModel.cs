using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class AjusteInventarioViewModel
    {
        public string NumeroDocumento {  get; set; }

        [Required(ErrorMessage = "Debe ingresar motivo del ajuste")]
        [StringLength(200)]
        public string Motivo { get; set; }

        public List<ProductoViewModel> Productos { get; set; }

        public List<AjusteDetalleViewModel> Detalles { get; set; }
    }
}