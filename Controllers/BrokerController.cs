using Microsoft.AspNetCore.Mvc;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;

namespace Entregable2_VilchezGuardia_JF.Controllers
{
    [Authorize(Roles = "Broker")]
    public class BrokerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public BrokerController(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Listado de Inmuebles
        public IActionResult Index()
        {
            var inmuebles = _context.Inmuebles.ToList();
            return View(inmuebles);
        }

        // Crear
        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inmueble);
                await _context.SaveChangesAsync();

                // Invalidar cache de catálogo
                await _cache.RemoveAsync("Catalogo_");

                return RedirectToAction("Index");
            }
            return View(inmueble);
        }

        // Editar
        public IActionResult Editar(int id)
        {
            var inmueble = _context.Inmuebles.FirstOrDefault(i => i.Id == id);
            if (inmueble == null) return NotFound();
            return View(inmueble);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _context.Update(inmueble);
                await _context.SaveChangesAsync();

                // Invalidar cache de catálogo
                await _cache.RemoveAsync("Catalogo_");

                return RedirectToAction("Index");
            }
            return View(inmueble);
        }

        // Activar/Desactivar
        public async Task<IActionResult> ToggleActivo(int id)
        {
            var inmueble = _context.Inmuebles.FirstOrDefault(i => i.Id == id);
            if (inmueble != null)
            {
                inmueble.Activo = !inmueble.Activo;
                _context.Update(inmueble);
                await _context.SaveChangesAsync();

                await _cache.RemoveAsync("Catalogo_");
            }
            return RedirectToAction("Index");
        }
    }
}
