using System.Security.Cryptography;
using System.Text;
using Ejemplo.Domain;
using Ejemplo.DTOs;
using Ejemplo.Repositories;

namespace Ejemplo.Services;

public sealed class AltaUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AltaUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponse> EjecutarAsync(UsuarioRequest request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmarPassword)
            throw new ArgumentException("La contraseña y su confirmación no coinciden.");
        if (request.Email != request.ConfirmarEmail)
            throw new ArgumentException("El email y su confirmación no coinciden.");
        if (!LocalidadCatalogo.Existe(request.LocalidadId))
            throw new ArgumentException("La localidad seleccionada no es válida.");
        if (await _usuarioRepository.ExisteEmailAsync(request.Email.Trim(), cancellationToken))
            throw new ArgumentException("El email ya está registrado.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            NombreUsuario = request.NombreUsuario.Trim(),
            Email = request.Email.Trim(),
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            PasswordHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.Password))),
            LocalidadId = request.LocalidadId,
            Calle = request.Calle.Trim(),
            Numero = request.Numero.Trim(),
            FechaAlta = DateTime.UtcNow
        };

        await _usuarioRepository.AgregarAsync(usuario, cancellationToken);
        return Mapear(usuario);
    }

    public async Task<IReadOnlyCollection<UsuarioResponse>> ObtenerTodosAsync(CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync(cancellationToken);
        return usuarios.Select(Mapear).ToArray();
    }

    private static UsuarioResponse Mapear(Usuario usuario) => new(
        usuario.Id, usuario.NombreUsuario, usuario.Email, usuario.Nombre,
        usuario.Apellido, usuario.LocalidadId, usuario.Calle, usuario.Numero,
        usuario.Activo, usuario.FechaAlta);
}