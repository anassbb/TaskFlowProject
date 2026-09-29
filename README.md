# TaskFlow

Outil de gestion de projets (style Jira/Trello) en **microservices**, projet de formation .NET 10 + Angular 22.

- Mémo v1, les fondations : [docs/TaskFlow_Memo_Technique_v1.html](docs/TaskFlow_Memo_Technique_v1.html)
- Mémo v2, les microservices et RabbitMQ : [docs/TaskFlow_Memo_Technique_v2.html](docs/TaskFlow_Memo_Technique_v2.html)

## Architecture

```
Angular ──► Gateway (YARP) ──► Projects API ──► projectsdb (Postgres)
                                    │
                                    └────────► RabbitMQ 4 ◄── (Tasks, Notifications, Reports : à venir)
```

## Stack

| Couche | Technologies |
|---|---|
| Orchestration locale | Aspire 13 (AppHost, dashboard, OpenTelemetry) |
| Gateway | YARP 2 + service discovery Aspire |
| Services | ASP.NET Core 10 Minimal APIs, Clean Architecture + Vertical Slices |
| Messagerie | RabbitMQ 4, Wolverine 6 (outbox, sagas) |
| Données | PostgreSQL 18, une base par service, EF Core 10 |
| Front | Angular 22 zoneless, signals, `httpResource`, Vitest, ESLint |
| Tests | xUnit v3, Shouldly, NetArchTest |
| CI | GitHub Actions, Dependabot |

## Structure

```
src/
  AppHost/TaskFlow.AppHost/         Orchestration Aspire (Postgres, RabbitMQ, services, gateway, front)
  BuildingBlocks/
    TaskFlow.ServiceDefaults/       OpenTelemetry, health checks, résilience
    TaskFlow.SharedKernel/          Entity, AggregateRoot, Result, Error, DomainEvent
  Gateway/TaskFlow.Gateway/         YARP : /api/projects/* -> projects-api
  Services/Projects/
    TaskFlow.Projects.Api/          Hôte HTTP du service
    TaskFlow.Projects.Domain/
    TaskFlow.Projects.Application/
    TaskFlow.Projects.Infrastructure/
    TaskFlow.Projects.Contracts/    Événements d'intégration publics
web/taskflow-ui/                    Application Angular (core / features / shared)
tests/                              Tests d'architecture et unitaires
```

Règles vérifiées par `tests/TaskFlow.ArchitectureTests` :
- dans un service : `Domain ← Application ← Infrastructure ← Api` ;
- entre services : on ne référence que le projet `*.Contracts` d'un autre service, et la Gateway n'en référence aucun.

## Prérequis

- .NET SDK 10.0.401+ (voir `global.json`)
- Node.js 22 LTS et Angular CLI 22
- Podman (machine démarrée) ou Docker
- Aspire CLI 13 (optionnel)

## Lancer l'application

```bash
dotnet run --project src/AppHost/TaskFlow.AppHost
```

Ou **F5** dans VS Code. Le lien du dashboard Aspire s'affiche dans la console. Il donne accès à tous les services,
à l'interface d'administration de RabbitMQ et au front Angular.

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

- **Podman** remplace Docker : `ASPIRE_CONTAINER_RUNTIME=podman` est défini dans `src/AppHost/TaskFlow.AppHost/Properties/launchSettings.json`.
- **Pas de `.exe` pour les projets applicatifs** (`UseAppHost=false`, voir `Directory.Build.targets`) : ils sont lancés via `dotnet X.dll`.
- **Proxy d'entreprise** : le registre npm doit être en `https://registry.npmjs.org/`.
