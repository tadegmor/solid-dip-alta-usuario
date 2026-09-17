using Ejemplo.Domain;
using Npgsql;

namespace Ejemplo.Repositories;

public sealed class PostgresUsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public PostgresUsuarioRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Postgres.");
    }

    public async Task AgregarAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            "INSERT INTO usuarios (id, nombre_usuario, email, nombre, apellido, password_hash, localidad_id, calle, numero, activo, fecha_alta) " +
            "VALUES (@id, @nombre_usuario, @email, @nombre, @apellido, @password_hash, @localidad_id, @calle, @numero, @activo, @fecha_alta)", connection);
        AgregarParametros(command, usuario);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM usuarios WHERE lower(email) = lower(@email))", connection);
        command.Parameters.AddWithValue("email", email);
        return (bool)(await command.ExecuteScalarAsync(cancellationToken))!;
    }

    public async Task<IReadOnlyCollection<Usuario>> ObtenerTodosAsync(CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand("SELECT id, nombre_usuario, email, nombre, apellido, password_hash, localidad_id, calle, numero, activo, fecha_alta FROM usuarios ORDER BY fecha_alta DESC", connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var usuarios = new List<Usuario>();
        while (await reader.ReadAsync(cancellationToken))
            usuarios.Add(new Usuario { Id = reader.GetGuid(0), NombreUsuario = reader.GetString(1), Email = reader.GetString(2), Nombre = reader.GetString(3), Apellido = reader.GetString(4), PasswordHash = reader.GetString(5), LocalidadId = reader.GetInt32(6), Calle = reader.GetString(7), Numero = reader.GetString(8), Activo = reader.GetBoolean(9), FechaAlta = reader.GetDateTime(10) });
        return usuarios;
    }

    private static void AgregarParametros(NpgsqlCommand command, Usuario usuario)
    {
        command.Parameters.AddWithValue("id", usuario.Id);
        command.Parameters.AddWithValue("nombre_usuario", usuario.NombreUsuario);
        command.Parameters.AddWithValue("email", usuario.Email);
        command.Parameters.AddWithValue("nombre", usuario.Nombre);
        command.Parameters.AddWithValue("apellido", usuario.Apellido);
        command.Parameters.AddWithValue("password_hash", usuario.PasswordHash);
        command.Parameters.AddWithValue("localidad_id", usuario.LocalidadId);
        command.Parameters.AddWithValue("calle", usuario.Calle);
        command.Parameters.AddWithValue("numero", usuario.Numero);
        command.Parameters.AddWithValue("activo", usuario.Activo);
        command.Parameters.AddWithValue("fecha_alta", usuario.FechaAlta);
    }
}