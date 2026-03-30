using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using api.Aplicacion.DTOs;
using Proceso.Aplicacion.Mappers;
using Proceso.Infraestructura.Persistencia;
using api.Interfaces;

namespace Proceso.Controladores;

[ApiController]
[Route("tasks")]
public class TasksController : ControllerBase
{
    private readonly IPythonRunner _runner;
    private readonly AppDbContext _db;

    public TasksController(IPythonRunner runner, AppDbContext db)
    {
        _runner = runner;
        _db = db;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
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

        return Ok(new
        {
            saved = true,
            metricId = metric.Id,
            results = pythonResponse.Results
        });
    }

    [HttpGet("cpu")]
    public async Task<IActionResult> GetCpu()
    {
        var result = await _runner.RunAsync(new
        {
            tasks = new[]
            {
                new { id = "1", type = "cpu_usage" }
            }
        });

        return Content(result, "application/json");
    }

    [HttpGet("memory")]
    public async Task<IActionResult> GetMemory()
    {
        var result = await _runner.RunAsync(new
        {
            tasks = new[]
            {
                new { id = "2", type = "memory_usage" }
            }
        });

        return Content(result, "application/json");
    }

    [HttpGet("disk")]
    public async Task<IActionResult> GetDisk()
    {
        var result = await _runner.RunAsync(new
        {
            tasks = new[]
            {
                new { id = "3", type = "disk_usage" }
            }
        });

        return Content(result, "application/json");
    }
}