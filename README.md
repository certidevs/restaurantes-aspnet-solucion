# Restaurantes ASP.NET · solución paso a paso

Este repositorio contiene el proyecto Restaurantes tal y como queda **al terminar cada lección** del
curso. Cada lección tiene un tag: `leccion-03`, `leccion-04`, etc.

Si te has perdido en clase, descarga el ZIP del tag de la última lección que hayas completado, ábrelo en
VS Code y continúa desde ahí:

```
https://github.com/certidevs/restaurantes-aspnet-solucion/archive/refs/tags/leccion-NN.zip
```

Para ver solo lo que cambia en una lección, compara dos tags en GitHub:
`https://github.com/certidevs/restaurantes-aspnet-solucion/compare/leccion-03...leccion-04`

El proyecto de partida de todo el curso es [certidevs/restaurantes-aspnet](https://github.com/certidevs/restaurantes-aspnet).

---

## Proyecto base

Plantilla de clase para crear paso a paso una aplicación de gestión de restaurantes con
ASP.NET Core MVC. Incluye la base común de usuarios, login, perfiles, SQLite, Entity
Framework, Razor, Bootstrap, Docker y CI.

No incluye todavía `Restaurant`, `Dish`, `Review`, `Order` ni sus pantallas. Es
intencional: esas entidades se construirán en clase desde cero.

## Requisitos

- SDK de .NET 10. `global.json` acepta cualquier SDK de la versión 10.0.
- Visual Studio Code y C# Dev Kit. Es el IDE estándar del curso en Windows, macOS y
  Linux.
- Git. Docker es opcional y solo se usa al explicar despliegue.

No instales ASP.NET, C#, SQLite ni Entity Framework por separado: los aporta el SDK o
se restauran como dependencias del proyecto.

## Instalación inicial

Instala .NET 10 SDK, Visual Studio Code y C# Dev Kit una sola vez en el ordenador.

### Windows (PowerShell)

```powershell
winget install --id Microsoft.DotNet.SDK.10 --exact
winget install --id Microsoft.VisualStudioCode --exact
winget install --id Git.Git --exact
```

Cierra y abre una terminal nueva; después instala la extensión:

```powershell
code --install-extension ms-dotnettools.csdevkit
```

### macOS (Terminal, con Homebrew)

```bash
brew install dotnet
brew install --cask visual-studio-code
brew install git
code --install-extension ms-dotnettools.csdevkit
```

Si no utilizas Homebrew, instala el SDK de .NET 10 con el instalador oficial de
[macOS](https://dotnet.microsoft.com/download/dotnet/10.0) y añade C# Dev Kit desde
el panel Extensions de VS Code.

### Ubuntu 26.04 (Terminal)

```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-10.0 git
sudo snap install code --classic
code --install-extension ms-dotnettools.csdevkit
```

Para otra distribución Linux, sigue el instalador oficial de
[.NET para Linux](https://learn.microsoft.com/dotnet/core/install/linux) y, una vez
instalado VS Code, ejecuta el último comando.

Comprueba la instalación desde esta carpeta:

```bash
dotnet --version
```

Debe mostrar una versión que empiece por `10.0`.
Si el comando `code` no se reconoce, abre VS Code e instala C# Dev Kit desde
**Extensions**.

## Primer arranque

Abre la carpeta `restaurantes-aspnet` en VS Code, no la carpeta padre. La extensión
carga automáticamente `RestaurantesAspNet.sln`.

```bash
code .
dotnet restore RestaurantesAspNet.sln
dotnet tool restore
dotnet run --project RestaurantesAspNet.csproj --launch-profile http
```

Abre `http://localhost:5107`. El primer arranque crea `App_Data/restaurantes.db`,
aplica la migración de Identity y añade los usuarios demo:

- `admin` / `Admin123!`
- `user` / `User123!`

## Comandos de trabajo diario

Todos se ejecutan desde la raíz de este repositorio.

```bash
# Restaurar dependencias NuGet y herramientas locales tras clonar o actualizar
dotnet restore RestaurantesAspNet.sln
dotnet tool restore

# Ejecutar la aplicación
dotnet run --project RestaurantesAspNet.csproj --launch-profile http

# Ejecutar con recarga al guardar archivos
dotnet watch --project RestaurantesAspNet.csproj run --launch-profile http

# Compilar y ejecutar los tests
dotnet build RestaurantesAspNet.sln
dotnet test RestaurantesAspNet.sln
```

### Entity Framework y SQLite

`dotnet-ef` está fijado en `.config/dotnet-tools.json`, por lo que `dotnet tool
restore` lo deja disponible sin instalar nada de forma global.

```bash
# Después de cambiar una entidad: crear y aplicar una migración
dotnet ef migrations add NombreDescriptivo
dotnet ef database update

# Corregir la última migración solo antes de aplicarla a la base de datos
dotnet ef migrations remove
```

Las migraciones se aplican también al arrancar la aplicación en desarrollo. El comando
`database update` se incluye para aprender el flujo explícito que se usará en clase.

### Git, CI y Docker

```bash
git status
git add .
git commit -m "feat: describe el cambio"
git pull --rebase
git push
```

[GitHub Actions](.github/workflows/build-and-test.yml) ejecuta restauración,
compilación Release y tests en cada `push` y *pull request*.

Docker es opcional:

```bash
docker build -t restaurantes-aspnet:local .
docker run --rm -p 10000:10000 restaurantes-aspnet:local
```

El contenedor queda disponible en `http://localhost:10000`.

## IDEs

En VS Code, selecciona el perfil `http` y pulsa F5 para depurar. Las tareas
`Restaurantes: compilar` y `Restaurantes: ejecutar tests` están disponibles en
**Terminal → Run Task**. En Windows, Visual Studio 2026 también abre
`RestaurantesAspNet.sln`; consulta [la guía de IDEs](docs/IDE-SETUP.md).
