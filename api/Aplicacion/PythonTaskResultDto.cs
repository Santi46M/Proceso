using System.Text.Json.Serialization;

namespace api.Aplicacion.DTOs;

public class PythonTaskResultDto
{
    [JsonPropertyName("results")]
    public List<PythonMetricResultDto> Results { get; set; } = new();
}

public class PythonMetricResultDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public PythonMetricDataDto Data { get; set; } = new();
}

public class PythonMetricDataDto
{
    [JsonPropertyName("cpu_percent")]
    public double? CpuPercent { get; set; }

    [JsonPropertyName("used_mb")]
    public double? UsedMb { get; set; }

    [JsonPropertyName("total_mb")]
    public double? TotalMb { get; set; }

    [JsonPropertyName("used_gb")]
    public double? UsedGb { get; set; }

    [JsonPropertyName("total_gb")]
    public double? TotalGb { get; set; }
}