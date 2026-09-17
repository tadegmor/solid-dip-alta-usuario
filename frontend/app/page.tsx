"use client";

import { FormEvent, useState } from "react";

const apiUrl = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";
const localidades = [
  { id: 1, nombre: "Santa Fe", provincia: "Santa Fe" },
  { id: 2, nombre: "Rosario", provincia: "Santa Fe" },
  { id: 3, nombre: "Rafaela", provincia: "Santa Fe" },
  { id: 4, nombre: "Esperanza", provincia: "Santa Fe" },
  { id: 5, nombre: "Paraná", provincia: "Entre Ríos" },
  { id: 6, nombre: "Concordia", provincia: "Entre Ríos" },
  { id: 7, nombre: "Gualeguaychú", provincia: "Entre Ríos" },
  { id: 8, nombre: "Concepción del Uruguay", provincia: "Entre Ríos" },
  { id: 9, nombre: "La Paz", provincia: "Entre Ríos" },
  { id: 10, nombre: "Santa Elena", provincia: "Entre Ríos" }
];

export default function Home() {
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const formElement = event.currentTarget;
    setLoading(true);
    setMessage("");
    const form = new FormData(formElement);
    const body = Object.fromEntries(form.entries());
    const payload = {
      ...body,
      LocalidadId: Number(body.LocalidadId)
    };

    try {
      const response = await fetch(`${apiUrl}/api/usuarios`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });
      const result = await response.json();
      if (!response.ok) throw new Error(result.error ?? "No se pudo registrar el usuario.");
      formElement.reset();
      setMessage(`Usuario creado: ${result.nombreUsuario}`);
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "No se pudo registrar el usuario.");
    } finally {
      setLoading(false);
    }
  }

  return (
  <main>
    <section className="intro">
      <p className="eyebrow">Caso de uso · DIP</p>
      <h1>Dar de alta un usuario</h1>
      <p>El formulario habla con la API; el caso de uso no sabe si los datos terminan en JSON o PostgreSQL.</p>
    </section>

    <form onSubmit={submit} className="form">
      <div className="field-row full-width">
        <label>Nombre de usuario<input name="NombreUsuario" required /></label>
      </div>

      <div className="field-row">
        <label>Nombre<input name="Nombre" required /></label>
        <label>Apellido<input name="Apellido" required /></label>
      </div>

      <div className="field-row">
        <label>Email<input name="Email" type="email" required /></label>
        <label>Confirmar email<input name="ConfirmarEmail" type="email" required /></label>
      </div>

      <div className="field-row">
        <label>Contraseña<input name="Password" type="password" required /></label>
        <label>Confirmar contraseña<input name="ConfirmarPassword" type="password" required /></label>
      </div>

      <div className="field-row full-width">
        <label>
          Localidad
          <select name="LocalidadId" defaultValue="" required>
            <option value="" disabled>Seleccioná una localidad</option>
            {localidades.map((localidad) => (
              <option key={localidad.id} value={localidad.id}>
                {localidad.nombre}, {localidad.provincia}
              </option>
            ))}
          </select>
        </label>
      </div>

      <div className="field-row">
        <label>Calle<input name="Calle" required /></label>
        <label>Número<input name="Numero" required /></label>
      </div>

      <button disabled={loading}>{loading ? "Guardando..." : "Crear usuario"}</button>
      {message && <p role="status" className="message">{message}</p>}
    </form>
  </main>
);
}