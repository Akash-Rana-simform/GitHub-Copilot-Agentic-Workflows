---
description: "Summarize the changes in a pull request or set of modified files, including what changed, which layers were affected, business impact, and any risks to existing behavior."
mode: ask
---

Summarize the following changes: ${input:changedFiles:Paste the list of changed files or describe the PR scope}

## What to Produce

### 1. Change Summary
A concise, plain-English description of what was changed and why, suitable for a PR description or release note.

### 2. Layers Affected
List which architectural layers were modified:
- Domain (`AgentFlow_Domain`)
- Application (`AgentFlow_Application`)
- Infrastructure (`AgentFlow_Infrastructure`)
- Web API (`AgentFlow`)

### 3. API Contract Impact
- Were any public endpoints added, modified, or removed?
- Were any request or response models changed?
- Are there any breaking changes to existing callers?

### 4. Business Rules Impact
Cross-reference `.github/instructions/domain.instructions.md` and identify:
- Which business rules are affected by these changes
- Whether any new rules were introduced
- Whether any existing rules could be violated by the change

### 5. Risk Assessment
- **High**: Could break existing behavior or violate a business rule
- **Medium**: Non-breaking but requires careful testing
- **Low**: Safe, isolated change with no side effects

### 6. Suggested PR Title
A concise PR title following the format: `[type]: short description`
Where type is one of: `feat`, `fix`, `refactor`, `test`, `docs`, `chore`

## Context

Refer to `.github/copilot-instructions.md` for architecture and layer conventions.
Refer to `.github/instructions/domain.instructions.md` for business rules.
