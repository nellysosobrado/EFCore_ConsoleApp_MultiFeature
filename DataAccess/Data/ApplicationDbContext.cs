using Microsoft.EntityFrameworkCore;
using ClassLibrary.Models;

namespace ClassLibrary.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Calculator> Calculations { get; set; }
    public DbSet<Shape> Shapes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Calculator>()
            .Property(c => c.Result)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Shape>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Area)
                .HasPrecision(18, 2);

            entity.Property(s => s.Perimeter)
                .HasPrecision(18, 2);

            entity.Property(s => s.ParametersJson)
                .HasColumnName("Parameters")
                .HasColumnType("nvarchar(max)")
                .HasDefaultValue("{}");
        });
    }
}