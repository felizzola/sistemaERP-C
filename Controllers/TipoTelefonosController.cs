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
    public class TipoTelefonosController : Controller
    {
        private readonly ErpDbContext _context;

        public TipoTelefonosController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: TipoTelefonos
        public IActionResult Index()
        {
            return View(_context.TipoTelefonos.ToList());
        }

        // GET: TipoTelefonos/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono =  _context.TipoTelefonos
                .FirstOrDefault(m => m.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTelefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoTelefono tipoTelefono)
        {
            if (ModelState.IsValid)
            {
                tipoTelefono.Id = Guid.NewGuid();
                _context.Add(tipoTelefono);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = _context.TipoTelefonos.Find(id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }
            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, TipoTelefono tipoTelefono)
        {
            if (id != tipoTelefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoTelefono);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoTelefonoExists(tipoTelefono.Id))
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
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = _context.TipoTelefonos
                .FirstOrDefault(m => m.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var tipoTelefono = _context.TipoTelefonos.Find(id);
            _context.TipoTelefonos.Remove(tipoTelefono);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTelefonoExists(Guid id)
        {
            return _context.TipoTelefonos.Any(e => e.Id == id);
        }
    }
}
