namespace Ejemplo.Domain;
public class Direccion
{
    public String Calle { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public Localidad Localidad { get; set; } = null!;
}