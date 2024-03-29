﻿using BlogDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using XSystem.Security.Cryptography;

namespace BlogDapper.Areas.Front.Controllers
{
    [Authorize]
    [Area("Front")]
    public class AccesosController : Controller
    {
        private readonly IDbConnection _bd;
        public AccesosController(IConfiguration configuration)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Acceso()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin(Usuario user)
        {
            if (ModelState.IsValid) 
            {
                var sql = "SELECT * FROM Usuario WHERE Login=@Login AND Password=@Password";

                //encriptar password a md5, antes de enviar la pregunta
                var Password = obtenerMD5(user.Password);

                var validar = _bd.Query<Usuario>(sql, new
                {
                    user.Login,
                    Password
                });

                //parte de creacion de la coockie
                if (validar.Count() == 1)
                {
                    //creacion de claims
                    var claims = new List<Claim>
                    {
                        //guardar el login del usuario
                        new Claim(ClaimTypes.Name, user.Login)
                    };

                    //Login - corresponde con el campo "Login" de la base de datos
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    //autenticar
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity));

                    //llevar al inicio - dar acceso una vez autenticado
                    return RedirectToAction("Index", "Inicio");
                }
                else
                {
                    TempData["mensajeConfirmacion"] = "Datos de acceso incorrectos";
                    return RedirectToAction("Acceso", "Accesos");
                }
            }
            else
            {
                TempData["mensajeConfirmacion"] = "Algunos de los campos obligatorios estan vacíos";
                return RedirectToAction("Acceso", "Accesos");
            }

        }
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserRegistro(Usuario user)
        {
            if (ModelState.IsValid)
            {
                var sql = "SELECT * FROM Usuario WHERE Login=@Login";

                var existeUsuario = _bd.Query<Usuario>(sql, new
                {
                    user.Login
                });

                //validar si ya existe usuario en bd
                if (existeUsuario.Count() > 0)
                {
                    TempData["mensajeConfirmacion"] = "El usuario ya existe";
                    return RedirectToAction("Registro", "Accesos");
                }
                // si el usuario no existe - se puede crear
                else
                {
                    //encriptar password a md5, antes de enviar
                    var Password = obtenerMD5(user.Password);

                    var User_ID = Guid.NewGuid();

                    var ingresarUsuarioSQL = "INSERT INTO Usuario(User_ID, Login,Password) VALUES (@User_ID, @Login, @Password)";

                    _bd.Execute(ingresarUsuarioSQL, new
                    {
                        User_ID,
                        user.Login,
                        Password,
                    });

                    //creacion de claims
                    var claims = new List<Claim>
                    {
                        //guardar el login del usuario
                        new Claim(ClaimTypes.Name, user.Login)
                    };

                    //Login - corresponde con el campo "Login" de la base de datos
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");
                    //autenticar
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    //llevar al inicio - dar acceso una vez autenticado
                    return RedirectToAction("Index", "Inicio");
                 
                }
            }
            else
            {
                TempData["mensajeConfirmacion"] = "Algunos de los campos obligatorios estan vacíos";
                return RedirectToAction("Acceso", "Accesos");
            }

        }

        //para salir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salir()
        {
            //para cerrar la sesión -  destruye la cookie con el "token" de vida
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Inicio");
        }


        //Método para encriptar contraseña con MD5 se usa tanto en el acceso como en el registro
        public static string obtenerMD5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;

        }

    }
}
