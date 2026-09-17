using Ejemplo.DTOs;

namespace Ejemplo.Domain;

public static class LocalidadCatalogo
{
    public static IReadOnlyCollection<LocalidadDTO> Todas { get; } = new[]
    {
        new LocalidadDTO(1, "Santa Fe", "Santa Fe"),
        new LocalidadDTO(2, "Rosario", "Santa Fe"),
        new LocalidadDTO(3, "Rafaela", "Santa Fe"),
        new LocalidadDTO(4, "Esperanza", "Santa Fe"),
        new LocalidadDTO(5, "Paraná", "Entre Ríos"),
        new LocalidadDTO(6, "Concordia", "Entre Ríos"),
        new LocalidadDTO(7, "Gualeguaychú", "Entre Ríos"),
        new LocalidadDTO(8, "Concepción del Uruguay", "Entre Ríos"),
        new LocalidadDTO(9, "La Paz", "Entre Ríos"),
        new LocalidadDTO(10, "Santa Elena", "Entre Ríos")
    };

    public static bool Existe(int id) => Todas.Any(localidad => localidad.Id == id);
}