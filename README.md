# GitHub Copilot Agentic Workflows

A minimal **Order Management Web API** built with **.NET 10** following clean layered architecture principles.

---

## Tech Stack

| | |
|---|---|
| Runtime | .NET 10 |
| Language | C# 14 |
| API | ASP.NET Core Web API |
| Docs | OpenAPI (built-in) |
| Storage | In-memory (`ConcurrentDictionary`) |

---

## Architecture

The solution is split into four projects, each with a single responsibility.

```
AgentFlow              → Web API  (controllers, DI, HTTP pipeline)
AgentFlow_Application  → Business logic, service classes, repository interfaces
AgentFlow_Infrastructure → Repository implementations (in-memory)
AgentFlow_Domain       → Entities — no framework dependencies
```

### Dependency direction

```
AgentFlow (Web API)
  ├── AgentFlow_Application
  │     └── AgentFlow_Domain
  └── AgentFlow_Infrastructure
        ├── AgentFlow_Domain
        └── AgentFlow_Application
```

Outer layers depend on inner layers. Inner layers never reference outer layers.

---

## Project Structure

```
AgentFlowWebAPI/
├── AgentFlow/
│   ├── Controllers/
│   │   └── OrderController.cs
│   ├── GlobalUsings.cs
│   └── Program.cs
├── AgentFlow_Application/
│   ├── GlobalUsings.cs
│   ├── IOrderRepository.cs
│   └── OrderService.cs
├── AgentFlow_Domain/
│   └── Order.cs
├── AgentFlow_Infrastructure/
│   ├── GlobalUsings.cs
│   └── OrderRepository.cs
└── .github/
    ├── copilot-instructions.md
    └── instructions/
        └── domain.instructions.md
```

---

## API Endpoints

### Create Order

```http
POST /orders
Content-Type: application/json

{
  "totalAmount": 150.00
}
```

**Response `200 OK`**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "totalAmount": 150.00
}
```

**Response `400 Bad Request`** — when `totalAmount` is ≤ 0.

---

## Domain

### Order

| Property | Type | Description |
|---|---|---|
| `Id` | `Guid` | Unique identifier, assigned at creation. |
| `TotalAmount` | `decimal` | Monetary value of the order. |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone <repo-url>
cd AgentFlowWebAPI
dotnet run --project AgentFlow
```

### Explore the API

Once running, open the OpenAPI explorer in your browser:

```
https://localhost:{port}/openapi/v1.json
```

---

## Conventions

- File-scoped namespaces throughout (`namespace Foo;`).
- Shared `using` directives centralised in `GlobalUsings.cs` per project — not repeated in individual files.
- Nullable reference types enabled across all projects.
- No CQRS, no MediatR — direct service calls only.
- Controllers are thin and contain no business logic.
