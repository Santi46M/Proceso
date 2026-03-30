namespace api.Aplicacion.DTOs;

public class WorkerMetricsDto
{
    public string Host { get; set; } = string.Empty;
    public decimal CpuPercent { get; set; }
    public int MemUsedMb { get; set; }
    public int MemTotalMb { get; set; }
    public decimal MemPercent { get; set; }
    public int DiskUsedGb { get; set; }
    public int DiskTotalGb { get; set; }
    public decimal DiskPercent { get; set; }
}