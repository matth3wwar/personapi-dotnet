# personapi-dotnet

Monolito ASP.NET Core con patrón **MVC** y **DAO** (repositorios) para el CRUD de `persona`, `profesion`, `telefono` y `estudios`.

- Stack: .NET 10, SQL Server 2022, Entity Framework Core, REST, Swagger
- Vistas MVC: `/Persona`, `/Profesion`, `/Telefono`, `/Estudio`
- API REST: `/api/personas`, `/api/profesiones`, `/api/telefonos`, `/api/estudios/{idProf}/{ccPer}`
- Swagger: `/swagger`

---

## Opción A: con Docker (un solo comando)

Requisito: [Docker Desktop](https://www.docker.com/products/docker-desktop/) abierto.

```bash
git clone <url-del-repo>
cd personapi-dotnet
docker compose up --build
```

Esto levanta SQL Server 2022, crea la base `persona_db` con `personapi_db.sql` y arranca la app.

- App: http://localhost:8080
- Swagger: http://localhost:8080/swagger

Para detener: `Ctrl + C` o `docker compose down`.
Para borrar también los datos: `docker compose down -v`.

> La contraseña de `sa` (`Persona_db2024!`) está en `docker-compose.yml` y es solo para desarrollo.
> Si ya tienes algo usando el puerto 1433 u 8080, cambia el puerto de la izquierda en `docker-compose.yml`.

---

## Opción B: local con Visual Studio

### Requisitos
- Visual Studio 2026 (o 2022 actualizado) con **Desarrollo ASP.NET y web** y el SDK de **.NET 10**
- SQL Server Express (instancia `SQLEXPRESS`)
- SQL Server Management Studio (opcional)

### Pasos
1. Clonar el repositorio:
   ```bash
   git clone <url-del-repo>
   ```
2. Crear la base de datos ejecutando `personapi_db.sql`:
   - En SSMS: abrir el archivo, conectarse a `localhost\SQLEXPRESS` y ejecutar (F5), o
   - Por consola:
     ```bash
     sqlcmd -S localhost\SQLEXPRESS -E -C -i personapi_db.sql
     ```
3. Si tu instancia de SQL Server tiene otro nombre, cambia la cadena `PersonaDb` en
   `personapi-dotnet/personapi-dotnet/appsettings.json`.
4. Abrir `personapi-dotnet/personapi-dotnet.slnx` en Visual Studio y ejecutar con **F5**.

También se puede correr sin Visual Studio:
```bash
cd personapi-dotnet/personapi-dotnet
dotnet run
```

### Error común
`Cannot open database "persona_db" requested by the login` → la base no existe todavía; ejecutar el paso 2.
