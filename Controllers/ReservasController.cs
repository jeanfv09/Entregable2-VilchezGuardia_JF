using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;

namespace Entregable2_VilchezGuardia_JF.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: Reservas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int inmuebleId)
        {
            // ¿Ya existe reserva activa?
            bool activa = await _context.Reservas.AnyAsync(r =>
                r.InmuebleId == inmuebleId && r.FechaExpiracion > DateTime.Now);

            if (activa)
            {
                TempData["msg"] = "Este inmueble ya tiene una reserva activa.";
                return RedirectToAction("Detalle", "Inmuebles", new { id = inmuebleId });
            }

            var reserva = new Reserva
            {
                InmuebleId = inmuebleId,
                UsuarioId = User.Identity.Name,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddHours(48)
            };

            _context.Add(reserva);
            await _context.SaveChangesAsync();
            TempData["msg"] = "Reserva creada por 48 horas.";
            return RedirectToAction("Detalle", "Inmuebles", new { id = inmuebleId });
        }
    }
}
