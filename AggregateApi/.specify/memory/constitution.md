# AggregateApi Constitution

## Core Principles (Minimal Requirements)

### I. Architecture: Backend (.NET 8)
- The backend MUST be a .NET 8 Web API project following a layered, testable design: Presentation (Controllers), Application (services), Domain (models/DTOs), Infrastructure (HTTP clients, caching). Use Dependency Injection and public interfaces for service contracts.

### II. API Contracts & Versioning
- All public HTTP endpoints MUST have OpenAPI (Swagger) definitions. API contracts use JSON and semantic versioning (MAJOR.MINOR.PATCH). Breaking changes require a MAJOR version bump and documented migration notes.

### III. Frontend: React Native
- The mobile frontend MUST be implemented with React Native (TypeScript recommended). Communication with the backend is via the defined JSON REST API. Shared DTOs or TypeScript types SHOULD be derived from OpenAPI where feasible.

### IV. Testing & CI
- Unit tests are REQUIRED for business logic; integration tests REQUIRED for key API flows. CI pipelines MUST run linting, tests, and build steps for both backend and frontend on every PR. Tests must pass before merging.

### V. Security & Configuration
- Secrets and credentials MUST be supplied via environment variables or secure stores (no hard-coded secrets). TLS required for network traffic in non-local environments. Authentication MUST use a documented mechanism (e.g., JWT/OAuth2).

### VI. Observability & Error Handling
- API responses MUST include consistent error payloads. Structured logging and basic metrics (request rate, error rate, latency) SHOULD be emitted by the backend.

## Development Workflow (Minimal)

1. Feature work occurs on short-lived branches; open PRs for review.
2. Every PR MUST include tests or an acceptance test plan for changed behavior.
3. Merge gated by passing CI, at least one approving reviewer, and successful OpenAPI validation when API surface changes.

## Governance

This document defines the minimal, non-negotiable engineering requirements for the AggregateApi project when implementing a .NET 8 backend and React Native frontend.

**Version**: 1.0.0 | **Ratified**: 2026-03-19 | **Last Amended**: 2026-03-19

<!-- End of minimal constitution -->
