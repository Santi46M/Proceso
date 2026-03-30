using Microsoft.EntityFrameworkCore;
using Proceso.Domain.Entities;

namespace Proceso.Infraestructura.Persistencia;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Metric> Metrics => Set<Metric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Metric>(entity =>
        {
            entity.ToTable("metrics");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("now()");

            entity.Property(x => x.Host)
                  .HasColumnName("host")
                  .HasMaxLength(100)
                  .HasDefaultValue("local");

            entity.Property(x => x.CpuPercent)
                  .HasColumnName("cpu_percent")
                  .HasPrecision(5, 2);

            entity.Property(x => x.MemTotalMb).HasColumnName("mem_total_mb");
            entity.Property(x => x.MemUsedMb).HasColumnName("mem_used_mb");
            entity.Property(x => x.MemPercent)
                  .HasColumnName("mem_percent")
                  .HasPrecision(5, 2);

            entity.Property(x => x.DiskTotalGb).HasColumnName("disk_total_gb");
            entity.Property(x => x.DiskUsedGb).HasColumnName("disk_used_gb");
            entity.Property(x => x.DiskPercent)
                  .HasColumnName("disk_percent")
                  .HasPrecision(5, 2);

            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_metrics_created_at");
            entity.HasIndex(x => new { x.Host, x.CreatedAt }).HasDatabaseName("idx_metrics_host_created_at");
        });
    }
}