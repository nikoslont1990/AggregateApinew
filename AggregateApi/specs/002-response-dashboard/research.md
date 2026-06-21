# Research: Response Dashboard UI

**Phase**: 0 (Outline & Research)  
**Date**: March 23, 2026  
**Context**: React dashboard for aggregated backend data (weather, news, Twitter)

## Research Tasks & Findings

### 1. React Composition Patterns for Data Fetching

**Task**: Best practices for React composition patterns with data fetching (hooks, render props, query libraries)

**Decision**: Use React Hooks + React Query (TanStack Query) for data fetching and state management

**Rationale**:
- React Hooks are the modern, recommended approach (Render Props are legacy)
- React Query provides automatic caching, synchronization, background refetching, and stale-while-revalidate pattern
- Reduces boilerplate compared to manual `useState` + `useEffect` patterns
- Built-in error handling, loading states, and retry logic
- Seamlessly integrates with TypeScript
- Industry standard (used by Meta, Amazon, Microsoft projects)

**Alternatives Considered**:
- **Manual useEffect + useState**: Simple but error-prone (race conditions, memory leaks, stale closures), more boilerplate, no built-in caching
- **SWR (stale-while-revalidate)**: Lightweight alternative, simpler than React Query but less powerful for complex scenarios
- **Apollo Client**: Designed for GraphQL, overkill for REST API consumption

**Implementation Details**:
- Install: `npm install @tanstack/react-query axios`
- Create custom hooks (`useAggregateData`) wrapping `useQuery`
- QueryClient configuration with sensible defaults (stale time: 5 min, cache time: 10 min)
- Integrate with suspense for cleaner code (React 18.x supports experimental suspense boundaries)

---

### 2. Responsive Design Strategy

**Task**: Responsive design strategies in React (CSS-in-JS vs Tailwind vs Module CSS)

**Decision**: Use Tailwind CSS with mobile-first breakpoints

**Rationale**:
- Tailwind is industry standard with excellent React integration
- Mobile-first approach aligns with responsive design best practices
- Highly configurable for project-specific breakpoints (320px, 768px, 1024px)
- No CSS file bloat; unused styles tree-shake automatically via PurgeCSS
- Strong TypeScript support
- Active community, extensive documentation

**Breakpoints** (Tailwind defaults with customization):
```
sm: 640px   (skip - use mobile first at 0)
md: 768px   (tablets)
lg: 1024px  (desktops)
xl: 1280px  (large desktops)
```

**Alternatives Considered**:
- **CSS Modules**: Better encapsulation but requires manual responsive logic, not ideal for dashboard with many breakpoints
- **CSS-in-JS (styled-components, emotion)**: Powerful but slower at runtime, larger bundle size; Tailwind's JIT compiler is faster
- **Plain CSS**: No tooling support for responsive breakpoints, maintenance burden

**Implementation Approach**:
- Use Tailwind's responsive prefixes: `flex md:flex-row lg:grid lg:grid-cols-2`
- Custom Tailwind config for dashboard-specific colors/spacing
- Component library approach: reusable responsive components (Card, Grid, Sidebar)

---

### 3. Jest + React Testing Library Patterns

**Task**: Jest + React Testing Library testing patterns for components with async data

**Decision**: Comprehensive unit test suite with React Testing Library + Jest + MSW for API mocking

**Rationale**:
- React Testing Library enforces best practices: test user behavior, not implementation details
- Jest provides fast, zero-config test running with great DX
- MSW (Mock Service Worker) allows testing realistic API scenarios without hitting real backend
- Comprehensive testing aligns with constitutional requirement ("REQUIRED for business logic")
- Enables confident refactoring
- High code coverage target: 80%+ for components, 90%+ for hooks/utilities

**Testing Strategy**:
- **Unit Tests**: Individual components, hooks, utilities
- **Integration Tests**: Dashboard with mocked API calls
- **No E2E in this feature** (separate tool like Cypress/Playwright)

**Key Tools**:
- `@testing-library/react` - Component rendering and user-centric queries
- `@testing-library/user-event` - Realistic user interactions
- `jest` - Test runner and assertions
- `msw` (Mock Service Worker) - Network request mocking
- `@testing-library/jest-dom` - Custom matchers for DOM assertions

**Alternatives Considered**:
- **Enzyme**: Legacy, less aligned with React best practices
- **Shallow Rendering**: Undershoots integration depth, misses component interaction issues
- **Real API calls in tests**: Slow, flaky, not reproducible in CI

**Implementation Details**:
```typescript
// Example: Testing useAggregateData hook
test('useAggregateData fetches and caches data', async () => {
  // Arrange
  const queryClient = new QueryClient();
  const wrapper = ({ children }) => (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );

  // Act
  const { result } = renderHook(
    () => useAggregateData({ date: '2026-03-23', country: 'US' }),
    { wrapper }
  );

  // Wait for loading → loaded transition
  await waitFor(() => {
    expect(result.current.isLoading).toBe(false);
  });

  // Assert
  expect(result.current.data).toEqual(mockAggregateResponse);
  expect(result.current.error).toBeNull();
});
```

---

### 4. State Management Pattern

**Task**: State management patterns (Context API vs Redux vs Zustand) for dashboard parameter state

**Decision**: Use Context API + custom hooks for local parameter state, React Query for server state

**Rationale**:
- Context API is sufficient for dashboard parameter state (small, flat state tree)
- React Query handles server state (caching, synchronization)
- Separation of concerns: local state (UI parameters) vs server state (data)
- Reduces complexity: no Redux boilerplate for this feature
- Easier onboarding for React developers
- URL parameters (via React Router) as source of truth enables bookmarking/sharing

**Context Structure**:
```typescript
interface DashboardParams {
  date: string;
  country: string;
  company: string;
  category: string;
  sortBy: string;
}

const DashboardContext = createContext<{
  params: DashboardParams;
  setParams: (params: DashboardParams) => void;
}>({...});
```

**Alternatives Considered**:
- **Redux**: Overkill for this feature, excessive boilerplate, performance overhead
- **Zustand**: Lightweight but unnecessary complexity; Context + hooks is simpler for this scope
- **Prop drilling**: Acceptable for 2-3 levels; dashboard depth justifies Context

---

### 5. Error Boundary & Error Handling

**Task**: Error boundary and error handling patterns in React 18

**Decision**: Error Boundary component + user-friendly error display with recovery options

**Rationale**:
- Error Boundary catches React component lifecycle errors (render errors)
- Prevents entire app crash; gracefully degrades to fallback UI
- Separate error boundary for dashboard vs global app boundary
- Spec requirement: "display error messages when backend API calls fail or timeout"
- React 18 has improved error boundary APIs and DevTools integration

**Error Handling Strategy**:
- **Render Errors**: Error Boundary wrapper
- **API Errors**: Handled in React Query (queryFn errors, auto-retry)
- **User Feedback**: Toast notifications + inline error messages
- **Retry Button**: Allow manual retry after network error

**Implementation Approach**:
```typescript
class DashboardErrorBoundary extends React.Component {
  componentDidCatch(error, errorInfo) {
    // Log to monitoring service
    logErrorToService(error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return <ErrorFallback onReset={this.resetError} />;
    }
    return this.props.children;
  }
}
```

---

### 6. HTTP Mocking with MSW

**Task**: HTTP mocking strategies with MSW for comprehensive test coverage

**Decision**: Mock Service Worker (MSW) for integration-level testing of API calls

**Rationale**:
- MSW intercepts requests at the network layer (not library layer)
- Works across component, hook, and integration tests
- Realistic mock scenarios (errors, timeouts, partial responses)
- No library changes needed; tests use actual HTTP client code
- Simpler than stubbing library calls; catches transport layer issues

**Mock Setup**:
```typescript
// mocks/handlers.ts
import { http, HttpResponse } from 'msw';

export const handlers = [
  http.get('https://api.example.com/api/aggregate', () => {
    return HttpResponse.json(mockAggregateResponse);
  }),
  // Simulating timeout
  http.get('https://api.example.com/api/aggregate', async () => {
    await new Promise(resolve => setTimeout(resolve, 1000));
    return HttpResponse.error();
  }),
];
```

**Alternatives Considered**:
- **Jest mocking (jest.mock)**: Tighter coupling to implementation, fewer realistic scenarios
- **Vitest mocking**: Similar limitations; MSW is platform-agnostic
- **Real backend calls**: Slow, flaky, not reproducible

---

### 7. TypeScript Types from OpenAPI

**Task**: TypeScript types generation from OpenAPI spec (optional enhancement)

**Decision**: Generate TypeScript types from backend OpenAPI spec using `openapi-generator` or `swagger-typescript-api`

**Rationale**:
- Backend already exposes OpenAPI (Swagger) - reuse it
- Automatically keeps frontend types in sync with backend changes
- Reduces manual DTO maintenance
- Strong type safety for API responses
- Industry best practice for API integration

**Tool Choice**: `openapi-typescript` (lightweight, zero-config, recommended by OpenAPI community)

**Implementation**:
```bash
npm install -D openapi-typescript
# Generate types
npx openapi-typescript http://localhost:5000/swagger/v1/swagger.json -o src/types/api.generated.ts
```

**Alternatives Considered**:
- **Manual DTO files**: Fragile, goes out of sync with backend
- **No types (any)**: Defeats TypeScript benefits
- **swagger-codegen**: Heavier, generates too much boilerplate

---

## Summary of Decisions

| Area | Decision | Confidence |
|------|----------|-----------|
| **Data Fetching** | React Query (TanStack Query) | ✅ High |
| **Responsive Design** | Tailwind CSS + mobile-first | ✅ High |
| **Testing** | Jest + React Testing Library + MSW | ✅ High |
| **State Management** | Context API + React Router (URL params) | ✅ High |
| **Error Handling** | Error Boundary + Toast notifications | ✅ High |
| **API Mocking** | Mock Service Worker (MSW) | ✅ High |
| **TypeScript Types** | Generate from OpenAPI spec | ℹ️ Medium (optional enhancement) |

## Next Steps

- **Phase 1**: Use these decisions to create data-model.md, API contracts, and quickstart guide
- **Phase 1**: Verify decisions align with existing project setup (e.g., existing package.json, tsconfig)
- **Phase 2**: Generate task list with specific implementation steps
