using Microsoft.EntityFrameworkCore;
using ClassLibrary.Models;

namespace ClassLibrary.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Calculator> Calculations { get; set; }
    public DbSet<Shape> Shapes { get; set; }
    public DbSet<Game> Games { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Calculator>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FirstNumber)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(c => c.SecondNumber)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(c => c.Result)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(c => c.Operator)
                .HasConversion<string>()
                .IsRequired();


            entity.Property(c => c.CalculationDate)
                .IsRequired();

        });

        modelBuilder.Entity<Shape>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Area)
                .HasPrecision(18, 2);

            entity.Property(s => s.Perimeter)
                .HasPrecision(18, 2);

            entity.Property(s => s.Width).HasPrecision(18, 2);
            entity.Property(s => s.Height).HasPrecision(18, 2);
            entity.Property(s => s.Side).HasPrecision(18, 2);
            entity.Property(s => s.BaseLength).HasPrecision(18, 2);
            entity.Property(s => s.SideA).HasPrecision(18, 2);
            entity.Property(s => s.SideB).HasPrecision(18, 2);
            entity.Property(s => s.SideC).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Game>()
            .Property(g => g.GameDate)
            .HasDefaultValueSql("GETDATE()");


    }
}