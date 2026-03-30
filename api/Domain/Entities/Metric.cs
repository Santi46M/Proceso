using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proceso.Domain.Entities;

[Table("metrics")]
public class Metric
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    [MaxLength(100)]
    public string Host { get; set; } = "local";

    // ---------- CPU ----------
    [Required]
    [Range(0, 100)]
    public decimal CpuPercent { get; set; }

    // ---------- Memory ----------
    [Required]
    public int MemTotalMb { get; set; }

    [Required]
    public int MemUsedMb { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal MemPercent { get; set; }

    // ---------- Disk ----------
    [Required]
    public int DiskTotalGb { get; set; }

    [Required]
    public int DiskUsedGb { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal DiskPercent { get; set; }
}