using Microsoft.EntityFrameworkCore;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<TaxRule> TaxRules { get; set; }
    public DbSet<PublicHoliday> PublicHolidays { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehiclePass> VehiclePasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.MaxDailyTax).HasPrecision(18, 2);
        });

        modelBuilder.Entity<TaxRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasOne(e => e.City)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PublicHoliday>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.HasIndex(e => e.Date);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type)
                .HasConversion<string>()
                .IsRequired();
            entity.HasDiscriminator<string>("VehicleType")
                .HasValue<Car>(VehicleType.Car.ToString())
                .HasValue<Motorbike>(VehicleType.Motorbike.ToString())
                .HasValue<Emergency>(VehicleType.Emergency.ToString())
                .HasValue<Bus>(VehicleType.Bus.ToString())
                .HasValue<Diplomat>(VehicleType.Diplomat.ToString())
                .HasValue<Foreign>(VehicleType.Foreign.ToString())
                .HasValue<Military>(VehicleType.Military.ToString());
        });

        modelBuilder.Entity<VehiclePass>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.City)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.VehicleId, e.PassTime });
        });
    }
}

