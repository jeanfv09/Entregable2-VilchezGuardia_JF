using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;

namespace Entregable2_VilchezGuardia_JF.Areas.Broker.Controllers
{
    [Area("Broker")]
    [Authorize(Roles="Broker")]
    public class BrokerController : Controller
    {
        private readonly AppDbContext _context;

        public BrokerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var inmuebles = await _context.Inmuebles.ToListAsync();
            return View(inmuebles);
        }

        // GET: Create
        public IActionResult Create() => View();

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inmueble inmueble)
        {
            if(ModelState.IsValid)
            {
                _context.Add(inmueble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inmueble);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble == null) return NotFound();
            return View(inmueble);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inmueble inmueble)
        {
            if(id != inmueble.Id) return NotFound();
            if(ModelState.IsValid)
            {
                _context.Update(inmueble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inmueble);
        }

        // Activar/Desactivar
        public async Task<IActionResult> ToggleActivo(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);
            if(inmueble != null)
            {
                inmueble.Activo = !inmueble.Activo;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

