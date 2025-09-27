using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;

namespace Entregable2_VilchezGuardia_JF.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly AppDbContext _context;

        public InmueblesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Inmuebles/Catalogo
        public async Task<IActionResult> Catalogo(string ciudad, string tipo, double? precioMin, double? precioMax, int? dormitorios, int page = 1, int pageSize = 5)
        {
            var query = _context.Inmuebles.Where(i => i.Activo);

            // Filtros
            if (!string.IsNullOrEmpty(ciudad))
                query = query.Where(i => i.Ciudad.Contains(ciudad));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(i => i.Tipo == tipo);

            if (precioMin.HasValue)
                query = query.Where(i => i.Precio >= precioMin);

            if (precioMax.HasValue)
                query = query.Where(i => i.Precio <= precioMax);

            if (dormitorios.HasValue)
                query = query.Where(i => i.Dormitorios >= dormitorios);

            // Validación de rango
            if (precioMin.HasValue && precioMax.HasValue && precioMin > precioMax)
                ModelState.AddModelError("", "El precio mínimo no puede ser mayor que el máximo.");

            // Paginación
            var total = await query.CountAsync();
            var inmuebles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["Total"] = total;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(inmuebles);
        }

        // GET: /Inmuebles/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var inmueble = await _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id);
            if (inmueble == null) return NotFound();
            return View(inmueble);
        }
    }
}
