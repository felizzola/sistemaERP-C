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
    [Authorize]
    public class ImagenesController : Controller
    {
        private readonly ErpDbContext _context;

        public ImagenesController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Imagenes
        public IActionResult Index()
        {
            return View(_context.Imagenes.ToList());
        }

        // GET: Imagenes/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = _context.Imagenes
                .FirstOrDefault(m => m.Id == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // GET: Imagenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Imagenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh, Empleado")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                imagen.Id = Guid.NewGuid();
                _context.Add(imagen);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(imagen);
        }

        // GET: Imagenes/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = _context.Imagenes.Find(id);
            if (imagen == null)
            {
                return NotFound();
            }
            return View(imagen);
        }

        // POST: Imagenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Imagen imagen)
        {
            if (id != imagen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagen);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenExists(imagen.Id))
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
            return View(imagen);
        }

        // GET: Imagenes/Delete/5
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = _context.Imagenes
                .FirstOrDefault(m => m.Id == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // POST: Imagenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var imagen = _context.Imagenes.Find(id);
            _context.Imagenes.Remove(imagen);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenExists(Guid id)
        {
            return _context.Imagenes.Any(e => e.Id == id);
        }
    }
}
