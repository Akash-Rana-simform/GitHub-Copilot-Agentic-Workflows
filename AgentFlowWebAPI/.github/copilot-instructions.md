# Copilot Instructions — AgentFlow

## Solution Overview

AgentFlow is a layered .NET 10 Web API solution.
The project uses **C# 14** with file-scoped namespaces and global usings.

## Project Structure

| Project | Layer | Responsibility |
|---|---|---|
| `AgentFlow_Domain` | Domain | Entities only. No framework references. |
| `AgentFlow_Application` | Application | Business logic, service classes, repository interfaces. |
| `AgentFlow_Infrastructure` | Infrastructure | Repository implementations, in-memory or external storage. |
| `AgentFlow` | Web API | Controllers, DI registration, HTTP pipeline. |

## Dependency Direction

```
AgentFlow (Web API)
  ├── AgentFlow_Application
  │     └── AgentFlow_Domain
  └── AgentFlow_Infrastructure
        ├── AgentFlow_Domain
        └── AgentFlow_Application
```

Outer layers depend on inner layers. Inner layers never reference outer layers.

## Coding Conventions

- **C# version**: 14.0 — use the latest language features where they improve clarity.
- **Target framework**: .NET 10.
- **Namespaces**: Always use file-scoped namespaces (`namespace Foo;`).
- **Global usings**: Shared `using` directives live in `GlobalUsings.cs` per project. Do not repeat them in individual files.
- **Nullability**: Nullable reference types are enabled across all projects. Use `?` and null-checks accordingly.
- **Implicit usings**: Enabled. Do not re-declare BCL namespaces already covered by the SDK.

## Layer Rules

### Domain (`AgentFlow_Domain`)
- Plain C# classes only — no EF Core attributes, no framework references.
- No service or repository logic inside entities.
- See `.github/instructions/domain.instructions.md` for business concepts, rules, and terminology.

### Application (`AgentFlow_Application`)
- Defines repository interfaces (`IOrderRepository`).
- Contains service classes (`OrderService`) with pure business logic.
- No database, HTTP, or framework dependencies.
- Logic must be unit-testable without mocking infrastructure.

### Infrastructure (`AgentFlow_Infrastructure`)
- Implements interfaces defined in Application.
- Allowed to use in-memory collections, EF Core, or external clients.
- Never referenced directly by Application or Domain.

### Web API (`AgentFlow`)
- Controllers are thin — delegate all logic to Application services.
- No business logic inside controllers.
- Request/response models (records) live alongside the controller that uses them.
- DI registration lives in `Program.cs`.

## Patterns & Constraints

- **No CQRS, no MediatR** — direct service calls only.
- **No unnecessary abstractions** — only introduce an interface when there is a concrete reason (e.g., testability, DI).
- **No repository pattern in controllers** — controllers call services, services call repositories.
- Keep code minimal, readable, and clean.
