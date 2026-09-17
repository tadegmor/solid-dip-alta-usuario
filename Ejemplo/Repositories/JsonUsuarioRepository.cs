using System.Text.Json;
using Ejemplo.Domain;

namespace Ejemplo.Repositories;

public sealed class JsonUsuarioRepository : IUsuarioRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonUsuarioRepository(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var configuredPath = configuration["Persistence:JsonFilePath"] ?? "Data/usuarios.json";
        _filePath = Path.Combine(environment.ContentRootPath, configuredPath);
    }

    public async Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var usuarios = (await LeerAsync(cancellationToken)).ToList();
            usuarios.Add(usuario);
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(usuarios, JsonOptions), cancellationToken);
        }
        finally { _lock.Release(); }
    }

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try { return (await LeerAsync(cancellationToken)).Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)); }
        finally { _lock.Release(); }
    }

    public async Task<IReadOnlyCollection<Usuario>> ObtenerTodosAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try { return await LeerAsync(cancellationToken); }
        finally { _lock.Release(); }
    }

    private async Task<List<Usuario>> LeerAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath)) return new List<Usuario>();
        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<Usuario>>(stream, cancellationToken: cancellationToken) ?? new List<Usuario>();
    }
}