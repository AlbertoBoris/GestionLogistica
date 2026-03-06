using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.ViewModels
{
    public class ReporteLogisticoViewModel
    {
        public string TipoReporte { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public List<SelectListItem> TiposReporte { get; set; }

        public List<dynamic> Resultados { get; set; }
    }
}