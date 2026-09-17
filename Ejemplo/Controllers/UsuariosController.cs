using Ejemplo.DTOs;
using Ejemplo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ejemplo.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController : ControllerBase
{
    private readonly AltaUsuarioUseCase _altaUsuario;

    public UsuariosController(AltaUsuarioUseCase altaUsuario)
    {
        _altaUsuario = altaUsuario;
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> RegistrarUsuario(
        UsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await _altaUsuario.EjecutarAsync(request, cancellationToken);
            return Created($"api/usuarios/{usuario.Id}", usuario);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UsuarioResponse>>> ObtenerUsuarios(
        CancellationToken cancellationToken)
    {
        return Ok(await _altaUsuario.ObtenerTodosAsync(cancellationToken));
    }
}