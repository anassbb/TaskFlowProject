# TaskFlow

Outil de gestion de projets (style Jira/Trello), projet de formation .NET 10 + Angular 22.
Voir le mémo technique : [docs/TaskFlow_Memo_Technique_v1.html](docs/TaskFlow_Memo_Technique_v1.html).

## Stack

| Couche | Technologies |
|---|---|
| Orchestration locale | Aspire 13 (AppHost, dashboard, OpenTelemetry) |
| API | ASP.NET Core 10 Minimal APIs, ProblemDetails, OpenAPI + Scalar |
| Architecture | Modular monolith, Clean Architecture + Vertical Slices |
| Données | PostgreSQL 18 (conteneur Podman), EF Core 10 |
| Front | Angular 22 zoneless, signals, `httpResource`, Vitest, ESLint |
| Tests | xUnit v3, Shouldly, NetArchTest |
| CI | GitHub Actions, Dependabot |

## Structure

```
src/
  TaskFlow.AppHost/         Orchestration Aspire (Postgres + API + front)
  TaskFlow.ServiceDefaults/ OpenTelemetry, health checks, résilience
  TaskFlow.Api/             Hôte HTTP : Program.cs, gestion d'erreurs
  TaskFlow.SharedKernel/    Entity, AggregateRoot, Result, Error, DomainEvent
  Modules/Projects/         Module Projects : Domain / Application / Infrastructure
web/taskflow-ui/            Application Angular (core / features / shared)
tests/                      Tests d'architecture et unitaires
```

Règle de dépendance, vérifiée par `tests/TaskFlow.ArchitectureTests` :
`Domain ← Application ← Infrastructure ← Api`.

## Prérequis

- .NET SDK 10.0.401+ (voir `global.json`)
- Node.js 22 LTS et Angular CLI 22
- Podman (machine démarrée) ou Docker
- Aspire CLI 13 (optionnel)

## Lancer l'application

```bash
dotnet run --project src/TaskFlow.AppHost
```

Le lien du dashboard Aspire s'affiche dans la console. Il donne accès à Postgres, à l'API (`/scalar` pour tester les endpoints)
et au front Angular.

## Tests

```powershell
# Sur le poste de travail (la politique de sécurité bloque les .exe de test) :
./scripts/test.ps1

# Ailleurs (CI, machine perso) :
dotnet test --solution TaskFlow.slnx
```

Front :

```bash
cd web/taskflow-ui
npm run lint
npm test -- --watch=false
```

## Particularités du poste de travail

- **Podman** remplace Docker : `ASPIRE_CONTAINER_RUNTIME=podman` est défini dans `src/TaskFlow.AppHost/Properties/launchSettings.json`.
- **Pas de `.exe` pour les projets applicatifs** (`UseAppHost=false`, voir `Directory.Build.targets`) : ils sont lancés via `dotnet X.dll`.
- **Proxy d'entreprise** : le registre npm doit être en `https://registry.npmjs.org/`.
