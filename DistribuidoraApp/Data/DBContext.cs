
    using DistribuidoraApp.Models;
    // AppDbContext.cs
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<TipoProducto> TiposProducto { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProductoProveedor> ProductoProveedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoProveedor>()
                .HasKey(pp => new { pp.ProductoId, pp.ProveedorId });
        }
    }
