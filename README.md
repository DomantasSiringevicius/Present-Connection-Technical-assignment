# Room Reservation System

Technical assignment for the IDT internship at Present Connection.

## Tech stack

- **Back end:** ASP.NET Core 10 Web API, EF Core InMemory
- **Front end:** React, TypeScript, Vite, Material UI

## Back end structure (Clean Architecture)

```
api/
  api.slnx
  src/
    Domain/          entities, enums, business rules — no dependencies
    Application/     use cases, services, DTOs, interfaces  → Domain
    Infrastructure/  EF Core InMemory DbContext, repositories → Application
    Presentation/    ASP.NET Web API: controllers, DI setup, configuration → Application, Infrastructure
```

## Getting started

Requirements: .NET 10 SDK and Node.js 20+.

### Back end

```bash
cd api/src/Presentation
dotnet run
```

The API runs at http://localhost:5158. API docs are at http://localhost:5158/scalar.

### Front end

```bash
cd frontend
npm install
npm run dev
```

Open http://localhost:5173. Requests to `/api` are forwarded to the back end.
