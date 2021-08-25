using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo5.Database;
using BENT1C.Grupo5.Models;
using BENT1C.Grupo5.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo5.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradoresController : Controller
    {
        private readonly ErpDbContext _context;

        public AdministradoresController(ErpDbContext context)
        {
            //le pasamos nuestra base de datos como context para que asi nuestra variable tenga acceso a esta misma, basicamente esta trayendo una 
            //instancia de la base de datos, y despues pasandola para tener acceso y usarla.
            _context = context;
        }

        // GET: Administradores
        public IActionResult Index()
        {
            return View(_context.Administrador.ToList());
        }

        // GET: Administradores/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = _context.Administrador
                .FirstOrDefault(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: Administradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Administrador administrador, string pass)
        {
            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
            }
            if (ModelState.IsValid)
            {
                administrador.Id = Guid.NewGuid();
                administrador.Legajo = Guid.NewGuid();
                administrador.FechaAlta = DateTime.Now;
                administrador.Password = pass.Encriptar();
                _context.Add(administrador);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        // GET: Administradores/Edit/5
        public IActionResult Edit(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var administrador = _context.Administrador.Find(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Administrador administrador, string pass)
        {
            if (id != administrador.Id)
            {
                return NotFound();
            }
            //string pass = (string)administrador.Password.GetValue(0);

            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = _context.Administrador.Find(id);

                    admin.Nombre = administrador.Nombre;
                    admin.FechaAlta = administrador.FechaAlta;
                    admin.Email = administrador.Email;
                    admin.Legajo = administrador.Legajo;


                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        admin.Password = pass.Encriptar();
                    }

                    //_context.Update(administrador);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorExists(administrador.Id))
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
            return View(administrador);
        }


        private bool AdministradorExists(Guid id)
        {
            return _context.Administrador.Any(e => e.Id == id);
        }
    }
}
