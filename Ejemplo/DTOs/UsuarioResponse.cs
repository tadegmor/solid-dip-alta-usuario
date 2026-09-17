namespace Ejemplo.DTOs;

public sealed record UsuarioResponse(
    Guid Id,
    string NombreUsuario,
    string Email,
    string Nombre,
    string Apellido,
    int LocalidadId,
    string Calle,
    string Numero,
    bool Activo,
    DateTime FechaAlta);