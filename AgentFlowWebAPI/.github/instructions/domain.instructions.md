---
applyTo: "**"
---

# Project Domain Knowledge — AgentFlow

## What is AgentFlow?

AgentFlow is an **Order Management** Web API. It allows clients to create orders and apply
percentage-based discounts to them. The system is intentionally minimal and serves as a
clean-architecture reference for layered .NET applications.

## Business Concepts

### Order
An `Order` represents a commercial transaction initiated by a client.

| Field | Type | Description |
|---|---|---|
| `Id` | `Guid` | Unique identifier, generated on creation. |
| `TotalAmount` | `decimal` | Monetary value of the order. Reduced when a discount is applied. |
| `DiscountPercentage` | `double?` | Optional. The percentage discount that was last applied. `null` means no discount has been applied. |

### Discount
A discount is a percentage (0–100) applied to an existing order.

- Applying a discount **reduces `TotalAmount`** by the given percentage and records the
  percentage on `DiscountPercentage`.
- Formula: `TotalAmount = TotalAmount - (TotalAmount × discountPercentage / 100)`
- A discount can only be applied to an **existing** order. If the order is not found the
  operation returns nothing (`null`) and the API responds with `404 Not Found`.

## Features

| Feature | HTTP Method | Route | Description |
|---|---|---|---|
| Create order | `POST` | `/orders` | Creates a new order with a given total amount. |
| Apply discount | `POST` | `/orders/{id}/discount` | Applies a percentage discount to an existing order. |

## Business Rules

1. Every order is assigned a `Guid` identifier at creation time — clients do not supply it.
2. `TotalAmount` must be provided by the caller on creation; it represents the gross amount.
3. `DiscountPercentage` is optional on the entity — it is only populated after a discount is applied.
4. A discount is always applied to the **current** `TotalAmount`, not the original amount.
5. There is no concept of order status, cancellation, or payment in the current scope.

## Terminology Glossary

| Term | Meaning in this project |
|---|---|
| Order | A record of a purchase with an associated monetary amount. |
| Discount | A percentage reduction applied to an order's total amount. |
| Repository | A storage abstraction that persists and retrieves orders. |
| Service | An Application-layer class that contains all business logic for a feature area. |

## Out of Scope (current version)

- Order status lifecycle (pending, confirmed, shipped, etc.)
- Multiple discount rules or stacking discounts
- User / customer identity and authentication
- Persistent database storage (currently in-memory only)
