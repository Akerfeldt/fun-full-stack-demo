# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All projects live under `src/`. The solution is `src/FunDemo.slnx` (SLNX format, not `.sln`).

```powershell
dotnet build src/FunDemo.slnx                 # build everything
dotnet run --project src/FunDemo.AppHost      # run the whole app via Aspire (dashboard + all services)
dotnet test src/FunDemo.Tests                 # run tests
dotnet run --project src/FunDemo.DbUp         # create/migrate the local database (see Database below)
```

Run a single test:

```powershell
dotnet test src/FunDemo.Tests --filter "FullyQualifiedName~GetWebResourceRootReturns401"
```

`TreatWarningsAsErrors` is on for every project (`src/Directory.Build.props`), so any new warning fails the build.

NuGet uses **central package management** — add new packages as a versionless `<PackageReference Include="X" />` in the csproj plus a `<PackageVersion>` entry in `src/Directory.Packages.props`. Transitive pinning is enabled.

## Architecture

.NET 10 / Aspire demo of a small RPG-style API: a player has a name, race, class, and an X/Y location they can move around.

### Service composition

`FunDemo.AppHost/AppHost.cs` is the entry point for local development. It launches two projects — `identityserver` and `apiservice` — and wires a reference from the API to Identity Server. **Aspire injects the Identity Server URL into the API's configuration as `services:identityserver:https:0`**, which `ApiService/Program.cs` reads to configure the JWT bearer authority and the Swagger OAuth token URL. Running `FunDemo.ApiService` on its own leaves that null and auth will not work — always run through the AppHost.

`FunDemo.ServiceDefaults` is the shared Aspire extension project (OpenTelemetry, health checks, service discovery, resilience). `AddServiceDefaults()` / `MapDefaultEndpoints()` come from there; the AppHost's `WithHttpHealthCheck("/health")` calls depend on them.

### Authentication

Duende IdentityServer with everything configured in-memory in `IdentityServer/Config.cs` (clients, scopes, API resources) and `TestUsers`. There is no user store or database behind it.

- The API validates bearer tokens with audience `fun_api` and `MapInboundClaims = false`, so claims stay in raw JWT form — the code reads the `"sub"` claim directly (see `ApiService/Extensions/UserExtensions.cs`).
- The `sub` claim *is* the domain `UserId`. The client-credentials client `fun_user_1` hardcodes `sub` = `"1"`, which is how the Swagger UI flow gets an identity.
- Swagger UI is wired to the client-credentials flow, so you can authorize in-browser in Development.

### Domain / Infrastructure split

`FunDemo.Domain` is persistence-ignorant: `Player` is a plain aggregate with behavior (`GoUp`, `GoDown`, …), and identifiers/names wrap values via `ValueObject<T>` with validation in the constructor (`PlayerName` enforces 3–10 chars, `UserId` rejects blank).

`FunDemo.Infrastructure` keeps a **separate EF model**: `DbPlayer` in `FunContext.cs` (explicitly column-typed, mapped to table `Player`). `PlayerRepository` hand-maps between `Player` and `DbPlayer` in both directions — the domain type is never tracked by EF. Note the repository does **not** auto-save: `Create`/`UpdateLocation` only stage changes, and callers must call `SaveChanges()` explicitly.

### API controller hierarchy

Controllers inherit rather than repeat attributes:

`UserController` (`[ApiController]`, `[Authorize]`, no routes) → `PlayerController` (injects `IPlayerRepository`, exposes `GetPlayer()` resolving the current user's player) → `MeController` / `LocationController` (the actual endpoints).

Routes are declared as **absolute paths on each method** (`[HttpGet("api/me/location")]`) — there is no class-level `[Route]`. New endpoints under `/api/me` should extend `PlayerController` to inherit auth and player lookup.

DTOs and their enums (`CharacterDto`, `RaceDto`, `ClassDto`) are declared inline in `MeController.cs`; they mirror the domain enums by numeric value and are cast across.

### Database

**Not EF migrations.** `FunDemo.DbUp` is a standalone console app that runs the embedded `Resources/DbUp.sql` against SQL Server LocalDB, splitting batches on `GO\r\n`. The script is idempotent (`IF OBJECT_ID(...) IS NULL` guards), so re-running is safe. Schema changes go in that SQL file, and `DbPlayer` must be updated to match by hand.

DbUp is *not* part of the AppHost graph — run it manually before first use. Its connection strings are hardcoded constants in `FunDemo.DbUp/Constants.cs`, separate from the API's `ConnectionStrings:FunContext` in `appsettings.json`.

### Tests

`FunDemo.Tests` uses xUnit v3 with `Aspire.Hosting.Testing`. Tests boot the **entire distributed application** via `DistributedApplicationTestingBuilder<Projects.FunDemo_AppHost>`, then hit services through `app.CreateHttpClient("apiservice")` after waiting on `ResourceNotifications.WaitForResourceHealthyAsync`. These are slow integration tests (30s timeouts), not unit tests. Common usings (`Xunit`, `System.Net`, Aspire testing namespaces) are declared globally in the csproj.
