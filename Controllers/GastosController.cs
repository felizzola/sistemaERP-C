using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo5.Database;
using BENT1C.Grupo5.Entidades;
using Microsoft.AspNetCore.Authorization;
using BENT1C.Grupo5.Models;
using System.Security.Claims;

namespace BENT1C.Grupo5.Controllers
{
    //PUSE SOLO ACA EL AUTHORIZE PORQUE TANTO EMPLEADO COMO RRHH PUEDE REALIZAR CAMBIOS ETC, ESTA OK?
    [Authorize]
    public class GastosController : Controller
    {
        private readonly ErpDbContext _context;
        private readonly ErpDbContext _contextCC;

        public GastosController(ErpDbContext context, ErpDbContext context1)
        {
            _context = context;
            _contextCC = context1;

        }

        // GET: Gastos
        public IActionResult Index(Guid? empleadoID)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid pasarId = Guid.Parse(userId);
            var rolUser = this.User.FindFirstValue(ClaimTypes.Role);

            if (rolUser == "Empleado")
            {
                empleadoID = pasarId;
            }

            var erpDbContext = _context.Gastos.Include(g => g.CentroDeCosto).Include(g => g.Empleado)
                .Where(e => (!empleadoID.HasValue || empleadoID == e.EmpleadoId));

            return View(erpDbContext.ToList());
        }

        // GET: Gastos/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gastos/Create
        public IActionResult Create(Guid? empleadoID)
        {
            Guid pasarId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rolUser = this.User.FindFirstValue(ClaimTypes.Role);
            if (rolUser == "Empleado")
            {
                empleadoID = pasarId;
                ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre");
                ViewData["EmpleadoId"] = new SelectList(_context.Empleados.Where(e => (empleadoID == e.Id)), "Id", "NombreCompleto");
                return View();
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre");
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "NombreCompleto");
            return View();
        }


        //para cambiar las propiedades de un objeto (que le pasaste desde el html) se usa el asp-for, esto cambia
        // POST: Gastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh, Empleado")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gasto gasto, Guid? empleadoId)
        {
            //para validar, entra en el modelo, se fija todas las validation attributes, si las cumple, sigue, si no, el ModelState tira false

            if (ModelState.IsValid)
            {
                var centroDeCosto = _contextCC.CentroDeCostos.Find(gasto.CentroDeCostoId);
                if (centroDeCosto == null)
                {
                    TempData["Error"] = $"DEBE SELECCIONAR UN CENTRO DE COSTO VALIDO";
                    return RedirectToAction(nameof(Create), new { empleadoId = gasto.EmpleadoId });
                }
                if (gasto.Monto <= centroDeCosto.MontoMaximo)
                {
                    gasto.Id = Guid.NewGuid();
                    _context.Add(gasto);
                    centroDeCosto.MontoMaximo -= gasto.Monto;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index), new { empleadoId = gasto.EmpleadoId });
                }
                else
                {
                    TempData["Error"] = $"EL GASTO QUE INTENTA REALIZAR SUPERA EL SALDO DISPONIBLE";
                    return RedirectToAction(nameof(Create), new { empleadoId = gasto.EmpleadoId });
                }
            }
            return RedirectToAction(nameof(Index), new { empleadoId = gasto.EmpleadoId });
        }


        [Authorize(Roles = "Administrador")]
        // GET: Gastos/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos.Find(id);
            if (gasto == null)
            {
                return NotFound();
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Gasto gasto)
        {
            if (id != gasto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gasto);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { empleadoId = gasto.EmpleadoId });
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // GET: Gastos/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var gasto = _context.Gastos.Find(id);
            _context.Gastos.Remove(gasto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        [HttpGet]
        public IActionResult ListadoGastos()
        {
            Guid? empleadoId = null;
            Guid pasarId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rolUser = this.User.FindFirstValue(ClaimTypes.Role);
            if (rolUser == "Empleado")
            {
                empleadoId = pasarId;
            }
            var gastos = _context.Gastos
            .Include(g => g.Empleado)
            .ThenInclude(f => f.Foto)
            .Include(e => e.CentroDeCosto)
            .ThenInclude(g => g.Gerencia)
            .Where(g => (!empleadoId.HasValue || empleadoId == g.Empleado.Id) && (g.Empleado.EmpleadoActivo))
            .ToList().OrderByDescending(g => g.Fecha).ThenBy(e => e.Empleado.Nombre).ThenBy(e => e.Empleado.Apellido);



            return View(gastos);
        }

        [Authorize(Roles = "Administrador, Rrhh")]
        [HttpGet]
        public IActionResult ListadoGastosTotales()
        {
            var gastos = _context.Gastos
                .Include(e => e.CentroDeCosto)
                .ToList();

            ViewBag.GastosTotales = gastos
                .GroupBy(g => g.CentroDeCostoId)
                .Select(group => new CostoTotalViewModel
                {
                    CentroDeCostoId = group.Key,
                    Suma = group.Sum(gasto => gasto.Monto)
                })
                .ToList();

            var gerencias = _context.Gerencias
                .Include(gerencia => gerencia.CentroDeCostos)
                .ToList();

            return View(gerencias);
        }


        private bool GastoExists(Guid id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }
    }
}
