using Ejemplo.Domain;
using Ejemplo.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ejemplo.Controllers;

[ApiController]
[Route("api/localidades")]
public sealed class LocalidadesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<LocalidadDTO>> ObtenerLocalidades()
    {
        return Ok(LocalidadCatalogo.Todas);
    }
}