using Microsoft.EntityFrameworkCore;
using pdfood.api.Models.Files;
using pdfood.api.Models.Products;

namespace pdfood.api.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ImageUpload> ImageUploads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e => e.HasKey(k => k.Id));

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Image)
                .WithMany()
                .HasForeignKey(p => p.ImageId);

            modelBuilder.Entity<ImageUpload>(e => e.HasKey(k => k.Id));
        }
    }
}
