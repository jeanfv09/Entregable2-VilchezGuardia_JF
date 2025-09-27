using System.ComponentModel.DataAnnotations;

namespace Entregable2_VilchezGuardia_JF.Models
{
    public class Visita
    {
        public int Id { get; set; }

        [Required]
        public int InmuebleId { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public string Estado { get; set; } // Solicitada, Confirmada, Cancelada

        public string Notas { get; set; }
    }
}
