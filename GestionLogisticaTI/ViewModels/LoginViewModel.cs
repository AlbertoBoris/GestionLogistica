using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar correo")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe ingresar contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}