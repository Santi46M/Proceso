using api.Aplicacion.DTOs;
using Proceso.Domain.Entities;

namespace Proceso.Aplicacion.Mappers;

public static class MetricMapper
{
    public static Metric FromPython(List<PythonMetricResultDto> results)
    {
        var metric = new Metric
        {
            Id = Guid.NewGuid(),
            Host = Environment.MachineName,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var result in results)
        {
            if (result.Status != "ok" || result.Data == null)
                continue;

            switch (result.Type)
            {
                case "cpu_usage":
                    metric.CpuPercent = decimal.Round((decimal)(result.Data.CpuPercent ?? 0), 2);
                    break;

                case "memory_usage":
                    metric.MemUsedMb = (int)(result.Data.UsedMb ?? 0);
                    metric.MemTotalMb = (int)(result.Data.TotalMb ?? 0);
                    metric.MemPercent = metric.MemTotalMb > 0
                        ? decimal.Round((decimal)metric.MemUsedMb / metric.MemTotalMb * 100, 2)
                        : 0;
                    break;

                case "disk_usage":
                    metric.DiskUsedGb = (int)(result.Data.UsedGb ?? 0);
                    metric.DiskTotalGb = (int)(result.Data.TotalGb ?? 0);
                    metric.DiskPercent = metric.DiskTotalGb > 0
                        ? decimal.Round((decimal)metric.DiskUsedGb / metric.DiskTotalGb * 100, 2)
                        : 0;
                    break;
            }
        }

        return metric;
    }
}