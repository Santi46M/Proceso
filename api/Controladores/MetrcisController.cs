using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proceso.Infraestructura.Persistencia;
using api.Interfaces;
using api.Aplicacion.DTOs;
using Proceso.Aplicacion.Mappers;

namespace Proceso.Controladores;

[ApiController]
[Route("metrics")]
public class MetricsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPythonRunner _runner;

    public MetricsController(AppDbContext db, IPythonRunner runner)
    {
        _db = db;
        _runner = runner;
    }

    [HttpPost("collect")]
    public async Task<IActionResult> Collect()
    {
        try
        {
            var result = await _runner.RunAsync(new
            {
                tasks = new[]
                {
                    new { id = "cpu", type = "cpu_usage" },
                    new { id = "memory", type = "memory_usage" },
                    new { id = "disk", type = "disk_usage" }
                }
            });

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var pythonResponse = JsonSerializer.Deserialize<PythonTaskResultDto>(result, options);

            if (pythonResponse == null || pythonResponse.Results.Count == 0)
            {
                return StatusCode(500, new
                {
                    message = "No se pudo interpretar la respuesta del worker."
                });
            }

            var metric = MetricMapper.FromPython(pythonResponse.Results);

            await _db.Metrics.AddAsync(metric);
            await _db.SaveChangesAsync();

            const int maxMetrics = 100;

            var totalCount = await _db.Metrics.CountAsync();

            if (totalCount > maxMetrics)
            {
                var oldMetrics = await _db.Metrics
                    .OrderBy(m => m.CreatedAt)
                    .Take(totalCount - maxMetrics)
                    .ToListAsync();

                _db.Metrics.RemoveRange(oldMetrics);
                await _db.SaveChangesAsync();
            }

            return Ok(new
            {
                saved = true,
                metricId = metric.Id,
                results = pythonResponse.Results
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                detail = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var latest = await _db.Metrics
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefaultAsync();

        if (latest == null)
        {
            return NotFound(new
            {
                message = "No hay métricas guardadas."
            });
        }

        return Ok(latest);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int take = 20)
    {
        if (take <= 0) take = 20;
        if (take > 500) take = 500;

        var metrics = await _db.Metrics
            .OrderByDescending(m => m.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(metrics);
    }
}