# AggregateApi Development Guidelines

Auto-generated from feature plans (002-response-dashboard, 001-modern-weather-landing). Last updated: 2026-03-24

**Project**: Aggregate API service aggregating weather, news, and social data for learning/demo purposes.  
**Constitution**: [docs/CONSTITUTION.md](../../docs/CONSTITUTION.md) - minimal, non-negotiable engineering requirements.

---

## Project Overview

### Architecture

**Backend (.NET 8)**:
- Layered: `Presentation/Controllers` → `Application/{Interfaces,Implementation}` → `Domain/{Models,DTOs}` → `Infrastructure/{HttpClients,Caching}`
- Key endpoint: `GET /api/aggregate` (Swagger docs at `/swagger/ui/index.html`)
- Dependencies: Moq, RichardSzalay.MockHttp, Swashbuckle (OpenAPI)
- Nullable reference types enabled; implicit usings enabled

**Frontend (React Native + Web)**:
- **Mobile/Universal**: `/frontend/modern-weather` - Expo + React Native
- **Web Dashboard** (new): Response Dashboard UI (feature 002-response-dashboard) using React 18.x
- React Query for server-side state, Context API for client state
- Jest + React Testing Library + Mock Service Worker (MSW) for testing

### Active Technologies

| Layer | Framework | Version | Purpose |
|-------|-----------|---------|---------|
| **Backend API** | .NET (C#) | 8.0 | REST API, Swagger/OpenAPI, dependency injection |
| **Frontend (Mobile)** | React Native / Expo | 50.x | Cross-platform UI, web export via Expo |
| **Frontend (Web Dashboard)** | React (TypeScript) | 18.x | Responsive dashboard UI for aggregated data |
| **Client State** | Context API | - | URL-synced parameter state |
| **Server State** | React Query | 5.x | Data fetching, caching, synchronization |
| **HTTP Client** | Axios | - | API communication |
| **Testing** | Jest + React Testing Library + MSW | - | Unit, integration tests with mock APIs |
| **Styling** | Tailwind CSS | 3.x | Mobile-first responsive design |

---

## Build & Test Commands

### Backend (.NET 8)

```bash
cd . # root directory contains AggregateApi.csproj

# Restore NuGet packages
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Run application
dotnet run

# Debug
dotnet run --launch-profile "https"
```

**Environment**: Swashbuckle Swagger UI at `http://localhost:5000/swagger/ui/index.html`

### Frontend - React Native (Expo)

```bash
cd frontend/modern-weather

# Install dependencies
npm install

# Start Expo dev server (mobile + web unified)
npm start

# Start web-only (development)
npm run start:web

# Run tests in watch mode
npm test -- --watch

# Run tests once
npm test

# Lint
npm run lint

# Format code
npm run format

# Build for web
npm run build:web
```

**Environment**: Expo dev server at `http://localhost:8081`, web at `http://localhost:19006`

---

## Code Style & Conventions

### C# (.NET Backend)

- **Nullability**: Strict null checks enabled (`<Nullable>enable</Nullable>`)
- **Naming**: PascalCase for types/methods/properties; camelCase for local variables
- **Comments**: XML documentation (`///`) for all public endpoints
- **Architecture**: Dependency Injection (constructor injection), interfaces for service contracts
- **Testing**: Unit + integration tests; use Moq for mocks, RichardSzalay.MockHttp for HTTP
- **API Design**: RESTful endpoints with OpenAPI/Swagger documentation via Swashbuckle

### TypeScript / JavaScript (Frontend)

- **Language**: TypeScript 5.x, strict mode enabled (`"strict": true` in tsconfig.json)
- **Components**: Functional components with hooks, React.memo for pure components
- **State Management**: React Query (server state) + Context API (client state) + React Router (URL state)
- **Testing**: Jest + React Testing Library; test behavior not implementation; use MSW for API mocking
- **Styling**: Tailwind CSS (mobile-first breakpoints: sm=640px, md=768px, lg=1024px)
- **Naming**: camelCase for functions/variables, PascalCase for components/types
- **File Structure**: Colocate components with their tests (`Component.tsx` + `Component.test.tsx`)

### Shared Conventions

- **Branch naming**: `feature/<short-desc>`, `fix/<short-desc>`, `chore/<short-desc>` (e.g., `feature/response-dashboard`)
- **Commits**: Clear, descriptive messages; reference issue numbers when applicable
- **PR process**: 1 approving review minimum; all CI checks must pass before merge
- **Code review focus**: Type safety, test coverage (80%+ target), adherence to architecture

---

## Project Structure

```
AggregateApi/                    # .NET 8 Web API
├── Program.cs
├── AggregateApi.csproj
├── AggregateApi.http             # REST client file
├── Presentation/
│   └── Controllers/
│       └── AggregateController.cs  # Main endpoint: GET /api/aggregate
├── Application/
│   ├── Interfaces/
│   │   ├── IAggregateService.cs
│   │   ├── IApiService.cs
│   │   ├── ICacheService.cs
│   │   └── IWeatherApiClient.cs
│   └── Implementation/
│       ├── AggregateService.cs
│       ├── ApiService.cs
│       └── CacheService.cs
├── Domain/
│   ├── AggregateResponse.cs      # Response DTO
│   └── RequestParamsDTO.cs
├── Handler/
│   └── MockHttpMessageHandler.cs
├── docs/
│   ├── CONSTITUTION.md           # Project governance & rules
│   ├── CONTRIBUTING.md           # Contribution guidelines
│   └── CODE_OF_CONDUCT.md

frontend/modern-weather/         # React Native (Expo) + Web
├── package.json
├── tsconfig.json
├── jest.config.js
├── App.tsx
├── app/
│   ├── components/               # All components here
│   │   ├── Dashboard/            # NEW: Response Dashboard components (002-response-dashboard)
│   │   └── Card.tsx              # Existing component
│   ├── services/                 # API clients, React Query setup
│   ├── hooks/                    # Custom hooks (useAggregateData, useQueryParameters)
│   ├── types/                    # TypeScript type definitions
│   └── utils/                    # Utilities (validators, formatters, toast)
├── tests/
│   ├── setup.ts                  # Jest setup + MSW server
│   ├── mocks/                    # Mock data + MSW handlers
│   └── fixtures/                 # Test fixtures

specs/                            # Feature spec documents
├── 001-modern-weather-landing/   # Feature 1: Initial weather landing
└── 002-response-dashboard/       # Feature 2: Response dashboard (ACTIVE)
    ├── spec.md                   # User stories, requirements, success criteria
    ├── plan.md                   # Tech stack, architecture, design decisions
    ├── research.md               # Tech research (React Query, Testing Library, etc.)
    ├── data-model.md             # Entity definitions, validation rules
    ├── quickstart.md             # Setup & development guide
    ├── contracts/
    │   └── api.contract.md       # REST API contract specification
    └── tasks.md                  # 74 implementation tasks organized by user story
```

---

## Key Development Workflows

### Starting Backend Development

1. **Restore dependencies**: `dotnet restore`
2. **Build**: `dotnet build`
3. **Run tests**: `dotnet test`
4. **Run app**: `dotnet run`
5. **View API docs**: Open browser to `http://localhost:5000/swagger/ui/index.html`

### Starting Frontend Development

1. **Install deps**: `cd frontend/modern-weather && npm install`
2. **Start dev server**: `npm start` or `npm run start:web`
3. **Watch tests**: `npm test -- --watch`
4. **Lint code**: `npm run lint` (fix issues with `npm run format`)

### Adding a Feature

1. Use `spec-kit` workflow:
   - Create feature branch: `git checkout -b feature/<name>`
   - Run: `./.specify/scripts/powershell/create-new-feature.ps1 <description>`
   - Generate spec, plan, tasks under `/specs/<###-name>/`
2. Implement from `/specs/<###-name>/tasks.md` in dependency order
3. Write tests as you code (TDD approach))
4. Submit PR with linked spec/tasks; ensure CI passes

### Testing Strategy

**Backend (.NET)**:
- Unit tests with Moq for service dependencies
- HttpClient mocking with RichardSzalay.MockHttp
- Run: `dotnet test`

**Frontend (React)**:
- Jest + React Testing Library for unit/integration tests
- Mock Service Worker (MSW) for HTTP request mocking
- Colocate tests with components: `Component.test.tsx`
- Run: `npm test` or `npm test -- --watch`
- Coverage target: 80%+ components, 90%+ utilities

---

## Common Pitfalls & How to Avoid

| Issue | Prevention |
|-------|-----------|
| **API contract mismatch** | Check [contracts/api.contract.md](../../specs/002-response-dashboard/contracts/api.contract.md); generate TypeScript types from OpenAPI |
| **Null reference exceptions** | Use strict null checks (`<Nullable>enable</Nullable>`); always validate optional data |
| **React stale state in hooks** | Use React Query cache keys correctly; depend on `useCallback` dependencies |
| **Flaky tests** | Use MSW for network mocking; avoid sleep/setTimeout in tests |
| **Component re-renders unnecessarily** | Use React.memo, useCallback, useMemo; check React DevTools Profiler |
| **Bundle size bloat** | Use `npm run build:web` and check output size; tree-shake unused dependencies |
| **TypeScript errors missed** | Run `npm run type-check` before commit; enable `strict: true` |
| **Forgotten tests** | All new components/services MUST have test files; target 80%+ coverage |
| **CORS errors in frontend** | Verify backend `/api/aggregate` accepts requests from frontend origin; check browser DevTools |
| **Performance degradation** | Use Lighthouse and Chrome DevTools; target LCP <2.5s, TTI <3s |

---

## Documentation & Reference

- **Architecture**: [docs/CONSTITUTION.md](../../docs/CONSTITUTION.md) - project governance, minimal requirements
- **Contributing**: [docs/CONTRIBUTING.md](../../docs/CONTRIBUTING.md) - branch naming, PR process, code style
- **Feature Plans**: [specs/002-response-dashboard/](../../specs/002-response-dashboard/) - feature spec, design decisions, implementation tasks
- **API Contracts**: [specs/002-response-dashboard/contracts/api.contract.md](../../specs/002-response-dashboard/contracts/api.contract.md) - REST endpoint specs
- **Setup Guide**: [specs/002-response-dashboard/quickstart.md](../../specs/002-response-dashboard/quickstart.md) - dev environment setup
- **Active Technologies**: See "Active Technologies" section above

---

## Feature Status

### Active Development

- **Feature 002**: Response Dashboard UI (React 18.x web dashboard)
  - Status: Planning phase complete (spec, plan, research, tasks generated)
  - Tech: React Query, Jest, React Testing Library, MSW, Tailwind CSS
  - Tasks: 74 implementation tasks in [tasks.md](../../specs/002-response-dashboard/tasks.md)
  - MVP: Phases 1-4 (4-6 days) - Dashboard + Parameters + Tests

### Existing Features

- **Feature 001**: Modern Weather Landing (React Native / Expo)
  - Status: In progress
  - Tech: React Native, Expo, React Navigation

---

## Quick Tips

- **Fast feedback loop**: Use `npm test -- --watch` while developing components
- **Type safety first**: Run `npm run type-check` before testing; catch errors early
- **Debug React Query**: Install React Query DevTools extension (`@tanstack/react-query-devtools`)
- **Debug MSW**: Check browser Network tab; MSW intercepts requests transparently
- **Swagger docs**: Always check `/swagger/ui/index.html` to verify API contract
- **Performance**: Use Chrome DevTools → Performance → Record to profile bundle/render times

---

## Auto-Generated Technology Stack

**From Feature Plans** (last updated 2026-03-24):

- TypeScript 5.x (React 18.x): React 18.x, React Router, TypeScript, Axios, React Query/SWR
- Browser LocalStorage: Caching, optional: Redux/Zustand
- React Native / Expo: Expo, React Navigation, Jest + React Native Testing Library
- Testing: Jest, @testing-library/react, MSW
- Styling: Tailwind CSS
- Backend: .NET 8, C#, Swashbuckle/OpenAPI

<!-- MANUAL ADDITIONS START -->
<!-- Enhanced with: comprehensive build commands, architecture overview, code style guidelines, project structure, development workflows, common pitfalls, documentation references -->
<!-- MANUAL ADDITIONS END -->
