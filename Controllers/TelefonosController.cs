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
using System.Security.Claims;

namespace BENT1C.Grupo5.Controllers
{
    [Authorize]
    public class TelefonosController : Controller
    {
        private readonly ErpDbContext _context;

        public TelefonosController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Telefonos
        public IActionResult Index(Guid? empleadoID)
        {
            Guid pasarId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rolUser = this.User.FindFirstValue(ClaimTypes.Role);
            if (rolUser == "Empleado")
            {
                empleadoID = pasarId;
            }
            var erpDbContext = _context.Telefonos.Include(t => t.Empleado)
                .Include(t => t.TipoTelefono)
                .Where(e => (!empleadoID.HasValue || empleadoID == e.EmpleadoId));
            return View(erpDbContext.ToList());
        }

        // GET: Telefonos/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = _context.Telefonos
                .Include(t => t.Empleado)
                .Include(t => t.TipoTelefono)
                .FirstOrDefault(m => m.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // GET: Telefonos/Create
        public IActionResult Create(Guid? empleadoID)
        {
            Guid pasarId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rolUser = this.User.FindFirstValue(ClaimTypes.Role);
            if (rolUser == "Empleado")
            {
                empleadoID = pasarId;
                ViewData["EmpleadoId"] = new SelectList(_context.Empleados.Where(e => (!empleadoID.HasValue || empleadoID == e.Id)), "Id", "NombreCompleto");
                ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre");
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "NombreCompleto");
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre");
            return View();
        }

        // POST: Telefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh, Empleado")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                telefono.Id = Guid.NewGuid();
                _context.Add(telefono);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            return View(telefono);
        }

        // GET: Telefonos/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = _context.Telefonos.Find(id);
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Telefono telefono)
        {
            if (id != telefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telefono);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonoExists(telefono.Id))
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            return View(telefono);
        }

        // GET: Telefonos/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = _context.Telefonos
                .Include(t => t.Empleado)
                .Include(t => t.TipoTelefono)
                .FirstOrDefault(m => m.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador, Rrhh")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var telefono =_context.Telefonos.Find(id);
            _context.Telefonos.Remove(telefono);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(Guid id)
        {
            return _context.Telefonos.Any(e => e.Id == id);
        }
    }
}
