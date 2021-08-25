using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo5.Database;
using BENT1C.Grupo5.Entidades;
using BENT1C.Grupo5.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo5.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly ErpDbContext _context;

        public EmpleadosController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Empleados
        public IActionResult Index()
        {
            var erpDbContext = _context.Empleados.Include(e => e.Foto);
            return View(erpDbContext.ToList());
        }

        // GET: Empleados/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados
                .Include(e => e.Foto)
                .Include(emp => emp.Telefonos).ThenInclude(telefono => telefono.TipoTelefono)
                .FirstOrDefault(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            ViewData["FotoId"] = new SelectList(_context.Imagenes, "Id", "Nombre");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Empleado empleado, string pass)
        {
            if (ModelState.IsValid)
            {
                empleado.Id = Guid.NewGuid();
                empleado.Password = pass.Encriptar();
                _context.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FotoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empleado.FotoId);
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            ViewData["FotoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empleado.FotoId);
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Empleado empleado, string pass)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            // string pass = (string)empleado.Password.GetValue(0);
            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Empleado.Password), ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var empleadoDb = _context.Empleados.Find(id);

                    empleadoDb.Nombre = empleado.Nombre;
                    empleadoDb.FechaAlta = empleado.FechaAlta;
                    empleadoDb.Email = empleado.Email;
                    empleadoDb.Legajo = empleado.Legajo;
                    empleadoDb.Apellido = empleado.Apellido;
                    empleadoDb.Dni = empleado.Dni;
                    empleadoDb.ObraSocial = empleado.ObraSocial;
                    empleadoDb.EmpleadoActivo = empleado.EmpleadoActivo;
                    empleadoDb.FotoId = empleado.FotoId;


                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        empleadoDb.Password = pass.Encriptar();
                    }

                    //_context.Update(empleado);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            ViewData["FotoId"] = new SelectList(_context.Imagenes, "Id", "Nombre", empleado.FotoId);
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados
                .Include(e => e.Foto)
                .FirstOrDefault(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var empleado = _context.Empleados.Find(id);
            _context.Empleados.Remove(empleado);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Buscar(Guid? gerenciaId)
        {
            var empleado = _context.Empleados
                .Include(imagen => imagen.Foto)
                .Include(e => e.Posicion)
                .ThenInclude(p => p.Gerencia)
                .ThenInclude(g => g.Gerencias)
                .Where(e => (!gerenciaId.HasValue || gerenciaId == e.Posicion.GerenciaId) && (e.EmpleadoActivo))
                .OrderBy(e => e.Nombre).ThenBy(e => e.Apellido)
                .ToList();



            ViewBag.Gerencias = new SelectList(_context.Gerencias, nameof(Gerencia.Id), nameof(Gerencia.Nombre), gerenciaId);

            return View(empleado);
        }

        [Authorize(Roles = "Administrador, Rrhh")]
        [HttpGet]
        public IActionResult ListadoSueldos()
        {
            var empleado = _context.Empleados
                .Include(imagen => imagen.Foto)
                .Include(e => e.Posicion)
                .Where(e => (e.EmpleadoActivo))
                .ToList().OrderByDescending(e => e.Posicion.Sueldo).ThenBy(e => e.Nombre).ThenBy(e => e.Apellido);
            return View(empleado);
        }

        private bool EmpleadoExists(Guid id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
