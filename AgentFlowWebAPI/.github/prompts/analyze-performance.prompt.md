---
description: "Analyze a .NET service or repository class for performance issues including EF Core N+1 queries, missing async/await, synchronous blocking, excessive allocations, and unbounded queries."
mode: ask
---

Analyze the following class for performance issues: ${input:targetClass:Enter the class name to analyze (e.g. OrderService, OrderRepository)}

## What to Check

### EF Core Patterns
- N+1 query problems (missing `Include` or `ThenInclude`)
- Missing `.AsNoTracking()` on read-only queries
- Loading entire collections when only a count or subset is needed
- Calling `.ToList()` or `.ToArray()` before filtering instead of filtering at the query level
- Missing pagination on queries that could return unbounded result sets

### Async / Await
- Synchronous blocking on async code (`.Result`, `.Wait()`, `GetAwaiter().GetResult()`)
- Missing `await` on async calls
- Missing `ConfigureAwait(false)` in library code
- `async void` methods outside of event handlers
- Missing `CancellationToken` propagation

### Memory and Allocations
- Unnecessary object creation inside loops
- Large collection materialization that could be streamed
- String concatenation in loops instead of `StringBuilder`

### General
- Repeated calls to the repository for data that could be fetched once
- Business logic running outside a single unit of work when transactional consistency matters

## Output Format

### Findings
For each issue found:
- **Severity**: High / Medium / Low
- **Location**: File, class, and method name
- **Issue**: What the problem is
- **Why it matters**: Production impact
- **Recommendation**: Specific fix with code snippet

### Summary
- Total issues by severity
- Highest-risk finding
- Recommended fix order
