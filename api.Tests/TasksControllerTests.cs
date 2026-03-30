using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class TasksControllerTests
{
    [Fact]
    public async Task GetCpu_ReturnsOkWithResults()
    {
        // Arrange: respuesta fake del runner (como si fuera .py)
        var fakeJson = JsonDocument.Parse("""
        {
          "results": [
            {
              "id": "1",
              "type": "cpu_usage",
              "status": "ok",
              "data": { "cpu_percent": 12.3 }
            }
          ]
        }
        """).RootElement;

        var runnerMock = new Mock<IPythonRunner>();
        runnerMock
            .Setup(r => r.RunAsync(It.IsAny<object>()))
            .ReturnsAsync(fakeJson);

        var controller = new TasksController(runnerMock.Object);

        // Act
        var result = await controller.GetCpu();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var json = Assert.IsType<JsonElement>(ok.Value);

        Assert.True(json.TryGetProperty("results", out var results));
        Assert.Equal(1, results.GetArrayLength());

        var first = results[0];
        Assert.Equal("cpu_usage", first.GetProperty("type").GetString());
        Assert.Equal("ok", first.GetProperty("status").GetString());
        Assert.Equal(12.3, first.GetProperty("data").GetProperty("cpu_percent").GetDouble(), 1);
    }
}
