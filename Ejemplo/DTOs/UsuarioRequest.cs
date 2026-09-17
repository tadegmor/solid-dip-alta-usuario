using System.ComponentModel.DataAnnotations;

namespace Ejemplo.DTOs;

public sealed class UsuarioRequest
{
    [Required] public string NombreUsuario { get; init; } = string.Empty;
    [Required, EmailAddress] public string Email { get; init; } = string.Empty;
    [Required, EmailAddress] public string ConfirmarEmail { get; init; } = string.Empty;
    [Required] public string Password { get; init; } = string.Empty;
    [Required] public string ConfirmarPassword { get; init; } = string.Empty;
    [Required] public string Nombre { get; init; } = string.Empty;
    [Required] public string Apellido { get; init; } = string.Empty;
    public int LocalidadId { get; init; }
    [Required] public string Calle { get; init; } = string.Empty;
    [Required] public string Numero { get; init; } = string.Empty;
}