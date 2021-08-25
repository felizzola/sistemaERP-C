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

namespace BENT1C.Grupo5.Controllers
{
    [Authorize(Roles = "Administrador, Rrhh")]
    public class PosicionesController : Controller
    {
        private readonly ErpDbContext _context;

        public PosicionesController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Posiciones
        public IActionResult Index()
        {
            var erpDbContext = _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable);
            return View(erpDbContext.ToList());
        }

        // GET: Posiciones/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // GET: Posiciones/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion");
            return View();
        }

        // POST: Posiciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh, Empleado")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Posicion posicion)
        {
            if (ModelState.IsValid)
            {
                posicion.Id = Guid.NewGuid();
                _context.Add(posicion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.ResponsableId);
            return View(posicion);
        }

        // GET: Posiciones/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = _context.Posiciones.Find(id);
            if (posicion == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.ResponsableId);
            return View(posicion);
        }

        // POST: Posiciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id,Posicion posicion)
        {
            if (id != posicion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posicion);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosicionExists(posicion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.ResponsableId);
            return View(posicion);
        }

        // GET: Posiciones/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // POST: Posiciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var posicion = _context.Posiciones.Find(id);
            _context.Posiciones.Remove(posicion);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool PosicionExists(Guid id)
        {
            return _context.Posiciones.Any(e => e.Id == id);
        }
    }
}
