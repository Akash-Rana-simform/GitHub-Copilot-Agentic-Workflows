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

---

## GitHub Copilot Agentic Workflows

This repository includes a complete set of custom Copilot agents and context files that demonstrate an agentic development workflow for .NET backend systems.

### Folder Structure

```
AgentFlowWebAPI/
└── .github/
    ├── copilot-instructions.md          ← Repository-wide architecture and coding rules
    ├── instructions/
    │   └── domain.instructions.md       ← Business rules and domain knowledge
    ├── prompts/
    │   ├── generate-unit-tests.prompt.md
    │   ├── analyze-performance.prompt.md
    │   └── summarize-pr-changes.prompt.md
    └── agents/
        ├── planning-agent.agent.md
        ├── implementation-agent.agent.md
        ├── code-review-agent.agent.md
        ├── pr-fix-agent.agent.md
        ├── functional-review-agent.agent.md
        └── sdlc-orchestrator.agent.md
```

### Context Files

| File | Purpose |
|---|---|
| `copilot-instructions.md` | Architecture overview, layer rules, and coding conventions applied to every Copilot interaction |
| `domain.instructions.md` | Business concepts, entity definitions, business rules, and terminology for the Order domain |

### Skills (Reusable Prompts)

Skills are parameterized, reusable workflows available in VS Code under **Copilot Chat → `/` command**.

---

#### Generate Unit Tests (`generate-unit-tests`)

| | |
|---|---|
| **File** | `.github/prompts/generate-unit-tests.prompt.md` |
| **Purpose** | Generate comprehensive xUnit tests for a service class, covering happy paths, edge cases, and business rule violations |
| **Parameter** | Service class name (e.g. `OrderService`) |

**Example usage:**
```
/generate-unit-tests OrderService
```

**What it produces:** Full compilable test class using xUnit + NSubstitute, with tests grouped by method and named `MethodName_Scenario_ExpectedBehavior`.

---

#### Analyze Performance (`analyze-performance`)

| | |
|---|---|
| **File** | `.github/prompts/analyze-performance.prompt.md` |
| **Purpose** | Analyze a service or repository class for EF Core N+1 queries, async misuse, synchronous blocking, and unbounded queries |
| **Parameter** | Class name to analyze (e.g. `OrderRepository`) |

**Example usage:**
```
/analyze-performance OrderRepository
```

**What it produces:** Severity-ranked findings (High / Medium / Low) with file/method location, impact description, and code fix for each issue.

---

#### Summarize PR Changes (`summarize-pr-changes`)

| | |
|---|---|
| **File** | `.github/prompts/summarize-pr-changes.prompt.md` |
| **Purpose** | Summarize changed files into a plain-English PR description including layers affected, API contract impact, business rules impact, and risk assessment |
| **Parameter** | List of changed files or PR scope description |

**Example usage:**
```
/summarize-pr-changes OrderService.cs, OrderController.cs, IOrderRepository.cs
```

**What it produces:** PR title suggestion, change summary, layers affected, API contract impact, business rule cross-reference, and risk rating.

### Custom Agents

All agents are available in VS Code under **Copilot Chat → Agent selector**.

---

#### Planning Agent (`planning-agent`)

| | |
|---|---|
| **File** | `.github/agents/planning-agent.agent.md` |
| **Purpose** | Transform requirements into an actionable, implementation-ready technical plan |
| **Tools** | `read`, `search`, `todo`, `edit` |
| **When to use** | Before any implementation begins — converts a user story or bug into tasks, API contracts, and AC traceability |

**Responsibilities:**
- Break down requirements into implementable tasks with execution order
- Define API endpoints, data models, and business logic components
- Identify edge cases, validation rules, and non-happy-path behavior
- Map each acceptance criterion to one or more concrete tasks
- Surface risks, assumptions, and open questions

**Example prompt:**
```
Add discount support to order processing.
```

---

#### Implementation Agent (`implementation-agent`)

| | |
|---|---|
| **File** | `.github/agents/implementation-agent.agent.md` |
| **Purpose** | Execute one task at a time from an approved plan and produce production-ready .NET code |
| **Tools** | `read`, `search`, `edit`, `execute`, `todo` |
| **When to use** | After a planning agent output has been reviewed and approved |

**Responsibilities:**
- Implement plan items with no pseudo code or incomplete methods
- Follow SOLID principles and layer separation (API → Services → Repository)
- Add input validation, structured logging, and exception handling
- Propagate `CancellationToken` through all async paths
- Preserve existing public API contracts unless the plan requires a change

**Example prompt:**
```
Execute Task 1 from the approved plan.
```

---

#### Code Review Agent (`code-review-agent`)

| | |
|---|---|
| **File** | `.github/agents/code-review-agent.agent.md` |
| **Purpose** | Perform deep technical review of implemented changes before they reach production |
| **Tools** | `read`, `search`, `edit`, `execute` |
| **When to use** | After implementation is complete — before merging or proceeding to functional validation |

**Responsibilities:**
- Validate .NET coding standards compliance
- Identify security vulnerabilities (OWASP-focused)
- Identify performance regressions (EF Core, async patterns, I/O)
- Assess API validation correctness and error response consistency
- Evaluate testability and maintainability

**Severity model:** Critical / Major / Minor  
**Output:** Issues list with file and line evidence, inline suggestions, and a final APPROVE or CHANGES REQUIRED verdict.

**Example prompt:**
```
Review the implemented changes for discount handling.
```

---

#### PR Fix Agent (`pr-fix-agent`)

| | |
|---|---|
| **File** | `.github/agents/pr-fix-agent.agent.md` |
| **Purpose** | Resolve code review comments accurately and safely, one comment at a time |
| **Tools** | `read`, `search`, `edit`, `execute`, `todo` |
| **When to use** | After receiving code review findings — to apply targeted fixes with traceability |

**Responsibilities:**
- Address each review comment in sequence — no comment is skipped
- Apply the smallest safe fix first
- Preserve architecture boundaries and existing API contracts
- Produce a traceable comment-to-fix mapping

**Example prompt:**
```
Fix the issues identified in the code review for discount validation.
```

---

#### Functional Review Agent (`functional-review-agent`)

| | |
|---|---|
| **File** | `.github/agents/functional-review-agent.agent.md` |
| **Purpose** | Validate whether the final implementation satisfies each acceptance criterion by tracing actual code logic |
| **Tools** | `read`, `search`, `execute` |
| **When to use** | After all code review findings have been resolved — as the final validation gate |

**Responsibilities:**
- Map each acceptance criterion to actual code paths
- Trace behavior from API endpoint to service layer to repository
- Verify edge case handling and business rule enforcement in code
- Issue an explicit PASS/FAIL verdict with evidence

**This agent does not write test cases.** It validates correctness from code logic only.

**Example prompt:**
```
Validate that the discount implementation satisfies all acceptance criteria.
```

---

#### SDLC Orchestrator (`sdlc-orchestrator`)

| | |
|---|---|
| **File** | `.github/agents/sdlc-orchestrator.agent.md` |
| **Purpose** | Route a high-level request through all five specialist agents in the correct order, enforcing quality gates at every stage |
| **Tools** | `agent`, `read`, `search`, `todo` |
| **Sub-agents** | `planning-agent`, `implementation-agent`, `code-review-agent`, `pr-fix-agent`, `functional-review-agent` |
| **When to use** | When you want the full SDLC workflow enforced automatically from a single prompt |

**Delegation topology:**
```
Planning Agent
      ↓
Implementation Agent
      ↓
Code Review Agent ←──┐
      ↓              │ loop until clean
PR Fix Agent ────────┘
      ↓
Functional Review Agent
      ↓
Final Delivery Report (PASS / FAIL)
```

**Example prompt:**
```
Add discount support to order processing.
```

---

### Choosing the Right Agent

| Task | Recommended Agent Path |
|---|---|
| New feature | Planning → Implementation → Code Review → PR Fix → Functional Review |
| Bug fix | Implementation → Code Review → PR Fix |
| Refactoring | Planning → Implementation → Code Review |
| Performance investigation | Planning → Code Review — never implement directly |
| Pull request review | Code Review |
| Fixing review comments | PR Fix |
| Validating a completed feature | Functional Review |
| Full SDLC automation | SDLC Orchestrator |

### Workflow Overview

```
User Prompt
     │
     ▼
Custom Agent (role + constraints)
     │
     ├──▶  copilot-instructions.md   (architecture and coding rules)
     ├──▶  domain.instructions.md    (business rules and domain knowledge)
     └──▶  Repository context        (files, structure, history)
                   │
                   ▼
           Structured Output
```
