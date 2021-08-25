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
    public class GerenciasController : Controller
    {
        private readonly ErpDbContext _context;

        public GerenciasController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Gerencias
        public IActionResult Index()
        {
            var erpDbContext = _context.Gerencias.Include(g => g.Direccion).Include(g => g.Empresa).Include(g => g.Responsable);
            return View(erpDbContext.ToList());
        }

        // GET: Gerencias/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias
                .Include(g => g.Direccion)
                .Include(g => g.Empresa)
                .Include(g => g.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // GET: Gerencias/Create
        public IActionResult Create()
        {
            ViewData["DireccionId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre");
            ViewData["ResponsableId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            return View();
        }

        // POST: Gerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gerencia gerencia)
        {
            if (ModelState.IsValid)
            {
                gerencia.Id = Guid.NewGuid();
                _context.Add(gerencia);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DireccionId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.DireccionId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Empleados, "Id", "Apellido", gerencia.ResponsableId);
            return View(gerencia);
        }

        // GET: Gerencias/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias.Find(id);
            if (gerencia == null)
            {
                return NotFound();
            }
            ViewData["DireccionId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.DireccionId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Empleados, "Id", "Apellido", gerencia.ResponsableId);
            return View(gerencia);
        }

        // POST: Gerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Gerencia gerencia)
        {
            if (id != gerencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerencia);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenciaExists(gerencia.Id))
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
            ViewData["DireccionId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.DireccionId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Empleados, "Id", "Apellido", gerencia.ResponsableId);
            return View(gerencia);
        }

        // GET: Gerencias/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias
                .Include(g => g.Direccion)
                .Include(g => g.Empresa)
                .Include(g => g.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // POST: Gerencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var gerencia = _context.Gerencias.Find(id);
            _context.Gerencias.Remove(gerencia);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        private bool GerenciaExists(Guid id)
        {
            return _context.Gerencias.Any(e => e.Id == id);
        }
    }
}
