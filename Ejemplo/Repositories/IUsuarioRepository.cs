using Ejemplo.Domain;

namespace Ejemplo.Repositories;

public interface IUsuarioRepository
{
    Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken);
    Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Usuario>> ObtenerTodosAsync(CancellationToken cancellationToken);
}