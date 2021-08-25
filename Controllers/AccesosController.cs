using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BENT1C.Grupo5.Database;
using BENT1C.Grupo5.Entidades;
using BENT1C.Grupo5.Extensions;
using BENT1C.Grupo5.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BENT1C.Grupo5.Controllers
{
    [AllowAnonymous]
    public class AccesosController : Controller
    {
        private readonly ErpDbContext _context;

        private const string _Return_Url = "ReturnUrl";
        public AccesosController(ErpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Ingresar(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            // Guardamos la url de retorno para que una vez concluído el login del 
            // usuario lo podamos redirigir a la página en la que se encontraba antes
            TempData[_Return_Url] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(string email, string password, Rol rol)
        {
            string returnUrl = TempData[_Return_Url] as string;
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                Usuario usuario = null;

                if (rol == Rol.Empleado)
                {
                    usuario = _context.Empleados.FirstOrDefault(usr => usr.Email == email);
                    var empleado = usuario as Empleado;
                    rol = empleado?.EmpleadoRrhh ?? false ? Rol.Rrhh : rol;
                }
                else
                {
                    usuario = _context.Administrador.FirstOrDefault(usr => usr.Email == email);
                }

                if (usuario != null)
                {
                    var passwordEncriptada = password.Encriptar();

                    if (usuario.Password.SequenceEqual(passwordEncriptada))
                    {
                        // Se crean las credenciales del usuario que serán incorporadas al contexto
                        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        // El lo que luego obtendré al acceder a User.Identity.Name
                        identity.AddClaim(new Claim(ClaimTypes.Email, email));

                        // Se utilizará para la autorización por roles
                        identity.AddClaim(new Claim(ClaimTypes.Role, rol.ToString()));

                        // Lo utilizaremos para acceder al Id del usuario que se encuentra en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                        // Lo utilizaremos cuando querramos mostrar el nombre del usuario logueado en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));

                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        // En este paso se hace el login del usuario al sistema
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                        // Guardo la fecha de último acceso que es ahora.
                        // usuario.FechaUltimoAcceso = DateTime.Now;
                        // _context.SaveChanges();

                        TempData["JustLoggedIn"] = true;

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }

            // Completo estos dos campos para poder retornar a la vista en caso de errores.
            ViewBag.Error = "Usuario o contraseña incorrectos";
            ViewBag.UserName = email;
            TempData[_Return_Url] = returnUrl;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Salir()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction(nameof(AccesosController.Ingresar), "Accesos");
        }
        [Authorize]
        [HttpGet]
        public IActionResult NoAutorizado()
        {
            return View();
        }



    }
}
