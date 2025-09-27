using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Models;

namespace Entregable2_VilchezGuardia_JF.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Codigo único
            builder.Entity<Inmueble>()
                   .HasIndex(i => i.Codigo)
                   .IsUnique();

            // Semilla de 3–4 inmuebles
            builder.Entity<Inmueble>().HasData(
                new Inmueble { Id = 1, Codigo = "DEP001", Titulo = "Departamento céntrico", Tipo = "Departamento", Ciudad = "Lima", Direccion = "Av. Principal 123", Dormitorios = 2, Banos = 1, MetrosCuadrados = 70, Precio = 150000, Activo = true, Imagen = "dep1.jpg" },
                new Inmueble { Id = 2, Codigo = "CAS001", Titulo = "Casa familiar", Tipo = "Casa", Ciudad = "Arequipa", Direccion = "Calle Flores 456", Dormitorios = 3, Banos = 2, MetrosCuadrados = 120, Precio = 250000, Activo = true, Imagen = "casa1.jpg" },
                new Inmueble { Id = 3, Codigo = "OFI001", Titulo = "Oficina moderna", Tipo = "Oficina", Ciudad = "Lima", Direccion = "Av. Negocios 789", Dormitorios = 0, Banos = 1, MetrosCuadrados = 50, Precio = 100000, Activo = true, Imagen = "ofi1.jpg" }
            );
        }
    }
}
