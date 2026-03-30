using System.Diagnostics;
using System.Text.Json;
using api.Interfaces;

namespace api.Aplicacion;

public class PythonRunner : IPythonRunner
{
    public async Task<string> RunAsync(object payload)
    {
        var json = JsonSerializer.Serialize(payload);

        var pythonExecutable = Environment.GetEnvironmentVariable("PYTHON_EXECUTABLE") ?? "python3";
        var workerPath = Environment.GetEnvironmentVariable("PYTHON_WORKER_PATH") ?? "../worker/main_stdin.py";

        var psi = new ProcessStartInfo
        {
            FileName = pythonExecutable,
            Arguments = workerPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi)
            ?? throw new Exception("No se pudo iniciar el worker");

        await process.StandardInput.WriteAsync(json);
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new Exception($"Error ejecutando el worker: {error}");

        if (string.IsNullOrWhiteSpace(output))
            throw new Exception("El worker no devolvió salida.");

        return output;
    }
}