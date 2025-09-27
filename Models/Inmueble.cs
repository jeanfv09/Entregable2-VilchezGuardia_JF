using System.ComponentModel.DataAnnotations;

namespace Entregable2_VilchezGuardia_JF.Models
{
    public class Inmueble
    {
        public int Id { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Imagen { get; set; }

        [Required]
        public string Tipo { get; set; } // Departamento, Casa, Oficina, Local

        [Required]
        public string Ciudad { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Range(0, 50)]
        public int Dormitorios { get; set; }

        [Range(0, 50)]
        public int Banos { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Metros cuadrados debe ser mayor a 0")]
        public int MetrosCuadrados { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Precio debe ser mayor a 0")]
        public double Precio { get; set; }

        public bool Activo { get; set; }
    }
}
