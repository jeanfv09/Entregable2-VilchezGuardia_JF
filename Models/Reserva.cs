using System.ComponentModel.DataAnnotations;

namespace Entregable2_VilchezGuardia_JF.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public int InmuebleId { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        public DateTime FechaExpiracion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
