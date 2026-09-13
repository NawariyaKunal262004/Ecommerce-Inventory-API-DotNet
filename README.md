# Medical Billing & Inventory API

A .NET 10 REST API for pharmacy and medical-store operations. It covers medicine batches, FIFO inventory, billing, patients, suppliers, authentication, audit logging, and rule-based analytics.

## Highlights

- Layered architecture with `Medical.API`, `Medical.Application`, `Medical.Core`, and `Medical.Infra`
- CQRS with MediatR, FluentValidation, and pipeline behaviors
- JWT authentication with refresh tokens and role-based authorization
- EF Core SQL Server persistence with migrations and transaction-safe stock updates
- FIFO batch stock deduction, low-stock and expiry reporting, and billing analytics
- Swagger/OpenAPI in Development, health checks, rate limiting, security headers, and Serilog logging

## Project structure

| Project | Responsibility |
| --- | --- |
| `Medical.API` | HTTP controllers, middleware, Swagger, application startup |
| `Medical.Application` | Commands, queries, handlers, validators, and services |
| `Medical.Core` | Domain entities, settings, contracts, and repository interfaces |
| `Medical.Infra` | EF Core DbContext, repositories, Identity, and migrations |

## Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server LocalDB
- Optional: `dotnet-ef` for applying migrations

```bash
dotnet tool install --global dotnet-ef
```

## Local setup

1. Clone the repository and open a terminal in the repository root.
2. Copy `Medical.API/.env.example` to `Medical.API/.env`.
3. Replace the database, JWT, and optional admin-seed placeholders. The `.env` file is ignored by Git.
4. Restore, build, and apply the database migrations:

```bash
dotnet restore Medical.API/Medical.API.slnx
dotnet build Medical.API/Medical.API.slnx
dotnet ef database update --project Medical.API/Medical.Infra --startup-project Medical.API/Medical.API
```

5. Start the API:

```bash
dotnet run --project Medical.API/Medical.API
```

Swagger is available at `http://localhost:5099/swagger` in Development. The health endpoint is `GET /health`.

## Configuration and security

Secrets are loaded from the ignored `.env` file through environment variables. The API requires `DATABASE_CONNECTION_STRING` and `JWT_KEY`; it does not fall back to committed credentials or signing keys. For production, provide these values through the hosting platform's secret manager instead of creating a `.env` file on the server.

The optional `SEED_ADMIN_*` variables create the initial administrator on first startup. Disable this after provisioning by setting `SEED_ADMIN_ENABLED=false`.

## API documentation

See [API.md](Medical.API/API.md) for endpoint details and example requests. The API includes modules for authentication, users, medicines, batches, inventory transactions, bills, patients, suppliers, and analytics.

## Useful commands

```bash
dotnet build Medical.API/Medical.API.slnx
dotnet run --project Medical.API/Medical.API/Medical.API.csproj --environment Development
```

## License

This project is intended as a portfolio project and learning reference.