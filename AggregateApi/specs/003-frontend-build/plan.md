# Implementation Plan: React Frontend for .NET 8 Backend

**Branch**: `003-frontend-build` | **Date**: March 26, 2026 | **Spec**: [spec.md](spec.md)  
**Input**: Feature specification from `specs/003-frontend-build/spec.md`

## Summary

Build a responsive, accessible React 18 frontend dashboard that consumes the .NET 8 AggregateController API. The application displays aggregated weather, news, and Twitter data with full CRUD operations for saved query parameters. Critical features include offline support with intelligent caching, keyboard accessibility (WCAG 2.1 Level AA), URL-synced parameters for bookmarking/sharing, and modern browser support (Chrome 90+, Firefox 88+, Safari 14+).

**MVP Scope**: Core dashboard (US1-US3, P1 features) with offline caching and accessibility built-in from day one.
**Extended Scope**: Data presentation polish (US4, P2) and responsive mobile refinements (US5, P2) in follow-up phases.

## Technical Context

**Language/Version**: TypeScript 5.x with React 18.x (modern ES2020+ features)  
**Frontend Framework**: React 18 with functional components and hooks  
**Build Tool**: Vite (fast dev server, optimized production builds)  
**Styling**: Tailwind CSS (utility-first, responsive by default, WCAG-compliant color utilities)  
**Routing**: React Router v6 (client-side routing, URL state sync, lazy-loaded routes)  
**State Management**: React Context API + useReducer for UI state; React Router for URL/navigation state  
**API Communication**: Axios with centralized `services/api/` folder (typed API methods, request/response interceptors, error handling)  
**Data Caching**: Browser localStorage (saved queries) + IndexedDB or localStorage (API response caching with TTL)  
**Testing**: Jest + React Testing Library (unit/component tests) + MSW (API mocking for tests)  
**Target Platform**: Web browsers (modern Chrome, Firefox, Safari, Edge)  
**Project Type**: Single Page Application (SPA) with offline-first caching layer  
**Performance Goals**: <5 seconds full data fetch (SC-001), 60fps interactions (SC-008), <500ms navigation (SC-006)  
**Constraints**: WCAG 2.1 Level AA accessibility non-negotiable (SC-011); 24-hour cache expiration (FR-021); 30-day hard cache delete (FR-023); 5MB localStorage assumption (SC-007)  
**Scale/Scope**: 1 main dashboard page + 1 saved queries management page; ~10-15 React components; 3 API endpoints consumed; 80%+ test coverage across components and utilities

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

**External Dependencies Check**: ✅ PASS
- All dependencies (React 18, Vite, TailwindCSS, React Router v6, Axios) are stable, widely-used, and actively maintained
- React 18 is LTS-aligned; Vite is production-ready (1.0+ stable)
- No exotic or proprietary technologies

**Accessibility as First-Class Feature**: ✅ PASS
- WCAG 2.1 Level AA is defined as a hard requirement (FR-015, FR-016, FR-017, SC-011)
- Tailwind CSS includes built-in accessible color utilities
- React Router v6 works well with keyboard navigation patterns
- Constitution aligned with modern web standards

**Architecture Simplicity**: ✅ PASS
- Context API chosen over Redux/Zustand (simpler for MVP scope, < 2000 LOC expected)
- Single-SPA not needed (single frontend, no micro-frontend complexity)
- Monolithic component structure sufficient for anticipated 10-15 component count
- Vite + Tailwind reduces build configuration burden

**Offline-First Design**: ✅ PASS
- Caching requirements (24-hour TTL, 30-day hard delete, manual clear button) are defined upfront
- localStorage for saved queries is fully specified
- API response caching strategy documented
- Edge case handling for storage quota exceeded is in scope

**Testing Coverage Target**: ✅ PASS
- 80%+ test coverage with Jest + React Testing Library is explicitly required (SC-009)
- MSW for API mocking ensures no backend dependency in test suite
- Component-level and utility-level testing split is clear

---

**Gate Status**: ✅ **PASS** — All constitution checks clear. Architecture is simple, aligned with modern React patterns, accessibility-first, and achieves feature scope without overengineering.

## Project Structure

### Documentation (this feature)

```text
specs/003-frontend-build/
├── spec.md              # Feature specification (completed)
├── plan.md              # This file (Phase 0-1 planning)
├── research.md          # Phase 0 output (tech decision rationale)
├── data-model.md        # Phase 1 output (React component tree + API contracts)
├── quickstart.md        # Phase 1 output (setup + dev workflow)
├── contracts/           # Phase 1 output
│   └── api.contract.md  # REST API request/response specifications
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
frontend/modern-weather/
├── public/
│   ├── index.html       # Entry point
│   └── favicon.ico
├── src/
│   ├── components/
│   │   ├── Dashboard.tsx           # Main dashboard layout (US1)
│   │   ├── DashboardForm.tsx       # Query parameters form (US1)
│   │   ├── ResultsSection.tsx      # Data display container (US4)
│   │   ├── WeatherCard.tsx         # Weather data display
│   │   ├── NewsSection.tsx         # News articles display
│   │   ├── TwitterSection.tsx      # Twitter data display
│   │   ├── LoadingSpinner.tsx      # Loading state indicator
│   │   ├── ErrorBoundary.tsx       # Error handling wrapper
│   │   ├── SavedQueriesPage.tsx    # Saved queries CRUD view (US2)
│   │   ├── QueryListItem.tsx       # Individual query card (US2)
│   │   ├── Navigation.tsx          # Top nav with page links (US3)
│   │   └── OfflineIndicator.tsx    # Offline status display
│   ├── hooks/
│   │   ├── useAggregateData.ts     # API fetch logic with caching
│   │   ├── useQueryParameters.ts   # URL query string sync
│   │   ├── useSavedQueries.ts      # localStorage CRUD for saved queries
│   │   └── useOnlineStatus.ts      # Network status detection
│   ├── services/
│   │   ├── api/
│   │   │   ├── aggregateClient.ts  # Axios instance + /api/aggregate endpoint
│   │   │   └── types.ts            # API request/response TypeScript types
│   │   ├── storage/
│   │   │   ├── queryCache.ts       # API response caching (localStorage/IndexedDB)
│   │   │   └── savedQueries.ts     # Saved query parameter storage
│   │   └── validation/
│   │       └── queryValidator.ts   # Date format, parameter validation
│   ├── types/
│   │   ├── api.ts                  # API types (AggregateResponse, etc.)
│   │   ├── models.ts               # Domain types (QueryParameters, SavedQuery, UIState)
│   │   └── index.ts                # Barrel export
│   ├── utils/
│   │   ├── dateFormat.ts           # ISO 8601 date parsing/formatting
│   │   ├── cacheExpiry.ts          # TTL calculation (24h, 30d)
│   │   ├── queryString.ts          # URL param serialization/deserialization
│   │   └── fileExport.ts           # JSON export functionality
│   ├── context/
│   │   ├── QueryContext.tsx        # Form state context (US1)
│   │   ├── CacheContext.tsx        # Cache metadata context
│   │   └── UIContext.tsx           # Loading, error, notification state context
│   ├── pages/
│   │   ├── DashboardPage.tsx       # Route: /dashboard
│   │   └── SavedQueriesPage.tsx    # Route: /saved-queries
│   ├── App.tsx                     # Root component, React Router setup
│   ├── main.tsx                    # Entry point (Vite)
│   └── index.css                   # Global Tailwind directives
├── tests/
│   ├── unit/
│   │   ├── hooks/
│   │   │   ├── useAggregateData.test.ts
│   │   │   ├── useQueryParameters.test.ts
│   │   │   ├── useSavedQueries.test.ts
│   │   │   └── useOnlineStatus.test.ts
│   │   ├── utils/
│   │   │   ├── dateFormat.test.ts
│   │   │   ├── cacheExpiry.test.ts
│   │   │   ├── queryString.test.ts
│   │   │   └── fileExport.test.ts
│   │   └── services/
│   │       ├── queryValidator.test.ts
│   │       └── api.test.ts  (mocked with MSW)
│   ├── components/
│   │   ├── Dashboard.test.tsx
│   │   ├── DashboardForm.test.tsx
│   │   ├── ResultsSection.test.tsx
│   │   ├── SavedQueriesPage.test.tsx
│   │   ├── Navigation.test.tsx
│   │   └── ErrorBoundary.test.ts
│   ├── integration/
│   │   ├── fullDataFlow.test.tsx (dashboard → API → results)
│   │   ├── savedQueriesCRUD.test.tsx
│   │   ├── offlineFlow.test.tsx
│   │   └── urlStateSync.test.tsx
│   ├── accessibility/
│   │   ├── wcagA11y.test.tsx (axe-core integration)
│   │   └── keyboardNav.test.tsx (keyboard-only navigation)
│   ├── setup.ts                    # Jest/RTL configuration + MSW server
│   ├── mocks/
│   │   ├── handlers.ts             # MSW request handlers
│   │   └── fixtures.ts             # Sample API responses
│   └── testUtils.tsx               # RTL render with providers wrapper
├── .env.development                # VITE_API_URL=http://localhost:5000
├── .env.production                 # VITE_API_URL=https://api.example.com
├── vite.config.ts                  # Vite config (React plugin, alias setup)
├── tsconfig.json                   # TypeScript strict mode
├── tailwind.config.js              # TailwindCSS theme customization
├── postcss.config.js               # PostCSS with Tailwind plugin
├── jest.config.js                  # Jest configuration
├── package.json                    # Dependencies: react@18, vite, tailwind, axios, react-router@6
└── README.md                       # Setup, dev, build, test instructions
```

**Structure Decision**: 
Single React SPA with feature-organized folder structure (components, hooks, services organized by domain). Vite handles bundling; Tailwind CSS config in repo root. Services folder separated into api/ (Axios), storage/ (localStorage/IndexedDB), and validation/ (input validation). Tests organize into unit/, components/, integration/, and accessibility/ mirroring the source structure. This keeps concerns separated while maintaining simplicity for ~10-15 component app size.

## Phase 0: Research & Decision Documentation

**Objective**: Document all technical decisions, evaluate alternatives, and resolve any remaining unknowns.

**Research Topics** (Pending):
1. IndexedDB vs localStorage for API response caching trade-offs (TTL, quota, performance)
2. Service Worker vs runtime caching strategy for offline support
3. React Context vs Zustand for form state (simplicity vs scalability)
4. URL state sync best practices with React Router v6
5. Accessibility testing strategy (axe-core vs WAVE vs Lighthouse)

**Deliverable**: `research.md` with technology decisions rationale, alternatives considered, and implementation approach

---

## Phase 1: Design & Data Models

**Objective**: Define component structure, API contracts, and implementation roadmap.

**Phase 1 Deliverables**:

### 1. data-model.md
- React component hierarchy (Dashboard → Form, ResultsSection, etc.)
- Component props interfaces (TypeScript)
- Context shape (QueryContext, UIContext, CacheContext)
- Key hook contracts (useAggregateData, useQueryParameters, etc.)
- API response type definitions (QueryParameters, AggregateResponse, SavedQuery, UIState, APIError)
- State transition diagrams for saved query CRUD

### 2. contracts/api.contract.md
- **Endpoint**: GET `/api/aggregate`
- **Request**: Query parameters (date, sortBy, company, country, category, url)
- **Response**: 200 with AggregateResponse DTO (WeatherData, NewsData, TwitterData)
- **Error Responses**: 400 (bad request), 500 (server error), network timeout
- **Caching Rules**: Cache 24 hours, hard delete after 30 days
- **SLAs**: 5-second max response time, 100% error message display

### 3. quickstart.md
- Step-by-step setup (npm install, .env config)
- Development server launch (`npm run dev`)
- Running tests (`npm run test`)
- Building for production (`npm run build`)
- Development workflow checklist

## Phase 2: Task Breakdown (Not in scope of `/speckit.plan`)

Executed by `/speckit.tasks` command after this plan is approved.

**Expected Outputs**:
- `tasks.md` with 50-100 tasks organized by:
  - Phase 1: Setup & infrastructure (5-10 tasks)
  - Phase 2: API integration (5-10 tasks)
  - Phase 3: Dashboard & form (10-15 tasks, US1+US3)
  - Phase 4: Saved queries CRUD (8-12 tasks, US2)
  - Phase 5: Data presentation (8-10 tasks, US4)
  - Phase 6: Responsive design (6-8 tasks, US5)
  - Phase 7: Testing & accessibility (10-15 tasks)
  - Phase 8: Polish & docs (5-10 tasks)

---

## Next Steps

1. ✅ **Specification Complete**: spec.md approved with all 5 clarifications integrated
2. ⏳ **Planning Phase** (current): Executing research and design documentation
3. 📋 **Upcoming**: `/speckit.tasks` to break plan into actionable implementation tasks
4. 🚀 **Implementation**: Begin task execution in sequence (setup → API → dashboard → CRUD → display → mobile → testing)

---

## Assumptions & Constraints

- **Backend Stability**: .NET 8 AggregateController is available, stable, and returns data matching AggregateResponse schema
- **CORS Enabled**: Backend has CORS configured for localhost:3000 (dev) and production domain
- **Browser Support**: Modern browsers only (Chrome 90+, Firefox 88+, Safari 14+) — no IE11/legacy browser support needed
- **Node Version**: Development requires Node.js 18+ (Vite requirement)
- **npm Packages**: All required packages available on npm registry (React 18, Tailwind, Axios, React Router v6, etc.)
- **Accessibility non-negotiable**: WCAG 2.1 Level AA is hard requirement, not optional refinement
- **Offline-first design**: Caching, TTL, and offline UX are core to MVP, not add-ons

---

## Complexity Notes

No overengineering detected in this plan. All decisions align with feature scope and team capacity:
- Context API suffices for UI state management (no Redux boilerplate)
- Vite selected over Create React App for faster dev/build experience
- Tailwind CSS eliminates CSS-in-JS configuration overhead
- React Router v6 is industry standard with excellent URL state support
- Jest + RTL is standard testing approach for React projects
- Accessibility (WCAG 2.1 AA) integrated upfront, not bolted on later

**Gate Status After Planning**: Ready to proceed to Phase 0 research.
