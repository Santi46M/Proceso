using api.Aplicacion;

namespace api.Interfaces;

public interface IPythonRunner
{
    Task<String> RunAsync(object payload);
}
