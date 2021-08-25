using System;
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
    public class CentroDeCostosController : Controller
    {
        private readonly ErpDbContext _context;

        public CentroDeCostosController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: CentroDeCostos
        public IActionResult Index()
        {
            var erpDbContext = _context.CentroDeCostos.Include(c => c.Gerencia);
            return View(erpDbContext.ToList());
        }

        // GET: CentroDeCostos/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = _context.CentroDeCostos
                .Include(c => c.Gerencia)
                .FirstOrDefault(m => m.Id == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        // GET: CentroDeCostos/Create
        public IActionResult Create()
        {
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            return View();
        }

        // POST: CentroDeCostos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CentroDeCosto centroDeCosto)
        {
            if (ModelState.IsValid)
            {
                centroDeCosto.Id = Guid.NewGuid();
                _context.Add(centroDeCosto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // GET: CentroDeCostos/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = _context.CentroDeCostos.Find(id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, CentroDeCosto centroDeCosto)
        {
            if (id != centroDeCosto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centroDeCosto);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroDeCostoExists(centroDeCosto.Id))
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
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // GET: CentroDeCostos/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = _context.CentroDeCostos
                .Include(c => c.Gerencia)
                .FirstOrDefault(m => m.Id == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var centroDeCosto = _context.CentroDeCostos.Find(id);
            _context.CentroDeCostos.Remove(centroDeCosto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CentroDeCostoExists(Guid id)
        {
            return _context.CentroDeCostos.Any(e => e.Id == id);
        }
    }
}
