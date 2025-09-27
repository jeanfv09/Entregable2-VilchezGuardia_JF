using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Entregable2_VilchezGuardia_JF.Models;

namespace Entregable2_VilchezGuardia_JF.Controllers
{
    [Authorize]
    public class VisitasController : Controller
    {
        private readonly AppDbContext _context;

        public VisitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Visitas/Agendar/5
        public IActionResult Agendar(int inmuebleId)
        {
            var visita = new Visita { InmuebleId = inmuebleId, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddHours(1) };
            return View(visita);
        }

        // POST: Visitas/Agendar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agendar(Visita visita)
        {
            if (visita.FechaInicio >= visita.FechaFin)
                ModelState.AddModelError("", "La fecha de inicio debe ser menor que la fecha de fin.");

            if (visita.FechaInicio.Hour < 8 || visita.FechaFin.Hour > 19)
                ModelState.AddModelError("", "Las visitas deben estar entre las 08:00 y 19:00.");

            // Revisar solapamiento
            bool solapada = await _context.Visitas.AnyAsync(v =>
                v.InmuebleId == visita.InmuebleId &&
                ((visita.FechaInicio >= v.FechaInicio && visita.FechaInicio < v.FechaFin) ||
                 (visita.FechaFin > v.FechaInicio && visita.FechaFin <= v.FechaFin)));

            if (solapada)
                ModelState.AddModelError("", "Ya existe una visita en ese intervalo para este inmueble.");

            if (ModelState.IsValid)
            {
                visita.UsuarioId = User.Identity.Name; // guarda usuario logueado
                visita.Estado = "Solicitada";
                _context.Add(visita);
                await _context.SaveChangesAsync();
                TempData["msg"] = "Visita agendada con éxito.";
                return RedirectToAction("Detalle", "Inmuebles", new { id = visita.InmuebleId });
            }

            return View(visita);
        }
    }
}
