using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripción")]
        [StringLength(200)]
        public string Descripcion { get; set; }

        public int StockActual { get; set; }

        [Required(ErrorMessage = "Debe indicar stock mínimo")]
        [Range(1, 10000, ErrorMessage = "El stock mínimo debe ser mayor a 0")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "Debe indicar ubicación")]
        [StringLength(100)]
        public string Ubicacion { get; set; }

        public string Estado { get; set; }
    }
}