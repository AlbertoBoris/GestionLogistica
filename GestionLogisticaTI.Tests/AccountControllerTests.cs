using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using GestionLogisticaTI.Controllers;
using GestionLogisticaTI.Helpers;
using GestionLogisticaTI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void Login_GET_RetornaVista()
        {
            var controller = new AccountController();

            var resultado = controller.Login() as ViewResult;

            Assert.IsNotNull(resultado, "Debe retornar una ViewResult");
        }

        [TestMethod]
        public void Login_POST_ModeloInvalido_RetornaVistaConErrores()
        {
            var controller = new AccountController();
            var model = new LoginViewModel
            {
                Correo = "",
                Contrasena = ""
            };
            controller.ModelState.AddModelError("Correo", "Debe ingresar correo");
            controller.ModelState.AddModelError("Contrasena", "Debe ingresar contraseña");

            var resultado = controller.Login(model) as ViewResult;

            Assert.IsNotNull(resultado, "Debe retornar la vista con errores");
            Assert.IsFalse(controller.ModelState.IsValid, "El modelo debe ser inválido");
        }

        [TestMethod]
        public void HashHelper_GenerarHash_ProduceHashConsistente()
        {
            string contrasena = "123456";

            string hash1 = HashHelper.GenerarHash(contrasena);
            string hash2 = HashHelper.GenerarHash(contrasena);

            Assert.AreEqual(hash1, hash2, "El mismo input debe producir siempre el mismo hash");
        }

        [TestMethod]
        public void HashHelper_GenerarHash_ProduceHashDiferenteParaInputDistinto()
        {
            string hash1 = HashHelper.GenerarHash("123456");
            string hash2 = HashHelper.GenerarHash("654321");

            Assert.AreNotEqual(hash1, hash2, "Inputs distintos deben producir hashes distintos");
        }

        [TestMethod]
        public void PasswordHelper_VerifyPassword_RetornaTrue_ConContrasenaCorrecta()
        {
            string contrasena = "123456";
            string hash = PasswordHelper.HashPassword(contrasena);

            bool resultado = PasswordHelper.VerifyPassword(contrasena, hash);

            Assert.IsTrue(resultado, "Debe verificar correctamente la contraseña");
        }

        [TestMethod]
        public void PasswordHelper_VerifyPassword_RetornaFalse_ConContrasenaIncorrecta()
        {
            string hash = PasswordHelper.HashPassword("123456");

            bool resultado = PasswordHelper.VerifyPassword("contraseñaWrong", hash);

            Assert.IsFalse(resultado, "Debe retornar false con contraseña incorrecta");
        }

        [TestMethod]
        public void Integracion_sp_Usuario_Login_RetornaDatos_ConCredencialesValidas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            string correo = "jefearea@empresa.com";
            string hash = HashHelper.GenerarHash("123456");
            bool retornoDatos = false;
            string rolObtenido = "";

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_Login", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contrasenaHash", hash);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        retornoDatos = true;
                        rolObtenido = reader["Rol"].ToString();
                    }
                }
            }

            Assert.IsTrue(retornoDatos, "El SP debe retornar datos con credenciales válidas");
            Assert.AreEqual("JefeArea", rolObtenido, "El rol debe ser JefeArea");
        }

        [TestMethod]
        public void Integracion_sp_Usuario_Login_NoRetornaDatos_ConCredencialesInvalidas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            string correo = "noexiste@empresa.com";
            string hash = HashHelper.GenerarHash("wrongpassword");
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_Login", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contrasenaHash", hash);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    retornoDatos = reader.Read();
                }
            }

            Assert.IsFalse(retornoDatos, "No debe retornar datos con credenciales inválidas");
        }
    }
}