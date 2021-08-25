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
    public class EmpresasController : Controller
    {
        private readonly ErpDbContext _context;

        public EmpresasController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public IActionResult Index()
        {
            var erpDbContext = _context.Empresas.Include(e => e.Logo);
            return View(erpDbContext.ToList());
        }

        // GET: Empresas/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = _context.Empresas
                .Include(e => e.Logo)
                .FirstOrDefault(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            ViewData["LogoId"] = new SelectList(_context.Imagenes, "Id", "Nombre");
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.Id = Guid.NewGuid();
                _context.Add(empresa);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LogoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empresa.LogoId);
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = _context.Empresas.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ViewData["LogoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empresa.LogoId);
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            ViewData["LogoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empresa.LogoId);
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = _context.Empresas
                .Include(e => e.Logo)
                .FirstOrDefault(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var empresa = _context.Empresas.Find(id);
            _context.Empresas.Remove(empresa);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(Guid id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
