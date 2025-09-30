using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Entregable2_VilchezGuardia_JF.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public CatalogoController(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: /Catalogo
        public async Task<IActionResult> Index(string ciudad, string tipo, double? precioMin, double? precioMax, int? dormitorios)
        {
            // Guardar filtros en sesión
            HttpContext.Session.SetString("UltimaCiudad", ciudad ?? "");
            HttpContext.Session.SetString("UltimoTipo", tipo ?? "");
            HttpContext.Session.SetString("UltimoPrecioMin", precioMin?.ToString() ?? "");
            HttpContext.Session.SetString("UltimoPrecioMax", precioMax?.ToString() ?? "");
            HttpContext.Session.SetString("UltimosDormitorios", dormitorios?.ToString() ?? "");

            // Clave de caché por filtros
            string cacheKey = $"Catalogo_{ciudad}_{tipo}_{precioMin}_{precioMax}_{dormitorios}";
            string catalogoJson = await _cache.GetStringAsync(cacheKey);

            List<Inmueble> inmuebles;
            if (!string.IsNullOrEmpty(catalogoJson))
            {
                // Obtener del cache
                inmuebles = JsonSerializer.Deserialize<List<Inmueble>>(catalogoJson);
            }
            else
            {
                // Traer de DB
                inmuebles = await _context.Inmuebles
                    .Where(i => i.Activo)
                    .Where(i => string.IsNullOrEmpty(ciudad) || i.Ciudad.Contains(ciudad))
                    .Where(i => string.IsNullOrEmpty(tipo) || i.Tipo == tipo)
                    .Where(i => !precioMin.HasValue || i.Precio >= precioMin)
                    .Where(i => !precioMax.HasValue || i.Precio <= precioMax)
                    .Where(i => !dormitorios.HasValue || i.Dormitorios >= dormitorios)
                    .ToListAsync();

                // Guardar en Redis por 60s
                catalogoJson = JsonSerializer.Serialize(inmuebles);
                await _cache.SetStringAsync(cacheKey, catalogoJson, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                });
            }

            return View(inmuebles);
        }

        // GET: /Catalogo/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var inmueble = await _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id);
            if (inmueble != null)
            {
                // Guardar último Inmueble visitado en sesión
                HttpContext.Session.SetInt32("UltimoInmuebleId", inmueble.Id);
                HttpContext.Session.SetString("UltimoTitulo", inmueble.Titulo);
            }
            return View(inmueble);
        }
    }
}
