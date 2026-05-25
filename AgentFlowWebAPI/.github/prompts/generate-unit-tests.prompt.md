---
description: "Generate comprehensive unit tests for a .NET service class, covering business rules, happy paths, edge cases, and failure scenarios."
mode: ask
---

Generate unit tests for the following service class: ${input:serviceClass:Enter the service class name (e.g. OrderService)}

## Requirements

- Use xUnit as the test framework
- Use NSubstitute for mocking dependencies
- Cover all public methods
- Include the following test categories for each method:
  - Happy path (valid input, expected output)
  - Edge cases (boundary values, nulls, empty collections)
  - Failure scenarios (invalid input, not found, business rule violations)
- Follow the naming convention: `MethodName_Scenario_ExpectedBehavior`
- Use `[Fact]` for single-case tests and `[Theory]` with `[InlineData]` for parameterized tests
- Assert both the return value and any side effects (e.g. repository calls)
- Do not test infrastructure — mock all repository dependencies

## Context

Refer to `.github/instructions/domain.instructions.md` for business rules that must be validated in tests.
Refer to `.github/copilot-instructions.md` for project structure and layer conventions.

## Output Format

- Full compilable test class
- One test class per service class
- Place tests in a project named `AgentFlow.Tests` under a folder matching the service layer
- Include a brief comment above each test group explaining what is being covered
