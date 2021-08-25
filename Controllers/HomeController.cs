using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BENT1C.Grupo5.Models;
using BENT1C.Grupo5.Database;
using BENT1C.Grupo5.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo5.Controllers

{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ErpDbContext _context;


        public HomeController(ILogger<HomeController> logger, ErpDbContext context)
        {
            _logger = logger;
            _context = context;


        }

        //si View() no tiene nombre,retorna la vista que se llama igual que el metodo, por defecto.
        public IActionResult Index()
        {
            ViewBag.Gerencias = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Gerencias, nameof(Gerencia.Id), nameof(Gerencia.Nombre));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
