using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;

namespace Entregable2_VilchezGuardia_JF.Areas.Broker.Controllers
{
    [Area("Broker")]
    [Authorize(Roles="Broker")]
    public class AgendaController : Controller
    {
        private readonly AppDbContext _context;
        public AgendaController(AppDbContext context) => _context = context;

        public async Task<IActionResult> VisitasHoy()
        {
            var hoy = DateTime.Today;
            var visitas = await _context.Visitas
                .Include(v => v.InmuebleId)
                .Where(v => v.FechaInicio.Date == hoy)
                .ToListAsync();
            return View(visitas);
        }

        public async Task<IActionResult> Confirmar(int id)
        {
            var visita = await _context.Visitas.FindAsync(id);
            if(visita != null)
            {
                visita.Estado = "Confirmada";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(VisitasHoy));
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            var visita = await _context.Visitas.FindAsync(id);
            if(visita != null)
            {
                visita.Estado = "Cancelada";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(VisitasHoy));
        }

        public async Task<IActionResult> ReservasActivas()
        {
            var reservas = await _context.Reservas
                .Include(r => r.InmuebleId)
                .Where(r => r.FechaExpiracion > DateTime.Now)
                .ToListAsync();
            return View(reservas);
        }

        public async Task<IActionResult> LiberarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if(reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ReservasActivas));
        }
    }
}
