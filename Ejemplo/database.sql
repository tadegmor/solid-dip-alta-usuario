CREATE TABLE IF NOT EXISTS usuarios (
    id uuid PRIMARY KEY,
    nombre_usuario text NOT NULL,
    email text NOT NULL UNIQUE,
    nombre text NOT NULL,
    apellido text NOT NULL,
    password_hash text NOT NULL,
    localidad_id integer NOT NULL,
    calle text NOT NULL,
    numero text NOT NULL,
    activo boolean NOT NULL DEFAULT false,
    fecha_alta timestamptz NOT NULL
);