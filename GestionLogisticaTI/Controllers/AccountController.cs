using GestionLogisticaTI.Data;
using GestionLogisticaTI.ViewModels;
using GestionLogisticaTI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.Controllers
{

    public class AccountController : Controller
    {
        // GET
        public ActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hash = HashHelper.GenerarHash(model.Contrasena);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Usuario_Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@correo", model.Correo);
                cmd.Parameters.AddWithValue("@contrasenaHash", hash);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Session["IdUsuario"] = reader["idUsuario"];
                    Session["Nombre"] = reader["nombre"].ToString();
                    Session["Rol"] = reader["Rol"].ToString();

                    return RedireccionarPorRol(reader["Rol"].ToString());
                }
                else
                {
                    ModelState.AddModelError("", "Credenciales incorrectas.");
                    return View(model);
                }
            }
        }

        private ActionResult RedireccionarPorRol(string rol)
        {
            switch (rol)
            {
                case "Administrador":
                    return RedirectToAction("Index", "Usuario");

                case "Ventas":
                    return RedirectToAction("Index", "Pedido");

                case "Logistica":
                    return RedirectToAction("Index", "Producto");

                case "Almacen":
                    return RedirectToAction("Index", "Despacho");

                case "JefeArea":
                    return RedirectToAction("Index", "Dashboard");

                default:
                    return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
