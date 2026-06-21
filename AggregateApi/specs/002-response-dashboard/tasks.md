---
description: "Implementation tasks for Response Dashboard UI feature"
---

# Tasks: Response Dashboard UI

**Input**: Design documents from `/specs/002-response-dashboard/`  
**Branch**: `002-response-dashboard`  
**Prerequisites**: [plan.md](plan.md) ✅, [spec.md](spec.md) ✅, [research.md](research.md) ✅, [data-model.md](data-model.md) ✅, [contracts/api.contract.md](contracts/api.contract.md) ✅

**Test Coverage**: Comprehensive unit tests with Jest + React Testing Library + MSW for all components, hooks, services, and utilities (80%+ coverage target)

**Organization**: Tasks grouped by user story (US1-US4) enabling independent implementation, testing, and MVP delivery. Each story phase is independently testable and deployable.

## Format Reference

```
- [ ] [TaskID] [P?] [Story] Description with exact file paths
```

- **[P]**: Parallelizable (different files, no inter-task dependencies)
- **[Story]**: User story label (US1, US2, US3, US4)
- Paths are relative to `frontend/modern-weather/`

## Dependency Overview

```
Phase 1 (Setup) 
  ↓
Phase 2 (Foundational - types, services, utils, test infrastructure)
  ↓ (blocks all stories)
Phase 3 (US1 - View Aggregate Data Dashboard) ← MVP foundation
  ↓ (US2 depends on US1 components)
Phase 4 (US2 - Query Parameter Configuration)
  ↓ (optional, non-blocking)
Phase 5 (US3 - Data Presentation & Readability)
Phase 6 (US4 - Real-time Data Refresh) ← P3, optional for MVP
  ↓
Phase 7 (Polish & Cross-cutting)
```

**MVP Scope**: Complete Phase 1, 2, 3, 4, and first half of Phase 7 (error handling, documentation). Skip Phase 5 (styling) and Phase 6 (refresh) for MVP.

---

## Phase 1: Setup (Project Initialization)

**Purpose**: Create directory structure and configure build environment

### 1.1 Project Structure

- [ ] T001 Create dashboard component directories in `src/app/components/Dashboard/`
- [ ] T002 Create service layer directory `src/app/services/`
- [ ] T003 Create custom hooks directory `src/app/hooks/`
- [ ] T004 Create shared types directory `src/app/types/`
- [ ] T005 Create utility functions directory `src/app/utils/`
- [ ] T006 [P] Create test infrastructure directory `tests/mocks/` and `tests/fixtures/`
- [ ] T007 [P] Create Jest configuration file `jest.config.js` with TypeScript and jsdom setup

### 1.2 Dependencies

- [ ] T008 Install React Query: `npm install @tanstack/react-query @tanstack/react-query-devtools` in `frontend/modern-weather/`
- [ ] T009 [P] Install testing dependencies: `npm install --save-dev @testing-library/react @testing-library/jest-dom @testing-library/user-event jest @types/jest ts-jest jest-environment-jsdom` 
- [ ] T010 [P] Install MSW: `npm install --save-dev msw`
- [ ] T011 [P] Verify TypeScript configuration `tsconfig.json` has `"jsx": "react-jsx"`, `"lib": ["ES2020", "DOM", "DOM.Iterable"]`, `"esModuleInterop": true`

### 1.3 Configuration

- [ ] T012 [P] Update Jest configuration in `jest.config.js` to use `ts-jest` preset and `jsdom` environment
- [ ] T013 [P] Create Jest setup file `tests/setup.ts` with MSW server initialization and React Query test utilities
- [ ] T014 [P] Configure `.env.local` with `REACT_APP_API_BASE_URL=http://localhost:5000` and `REACT_APP_API_TIMEOUT=5000`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Shared infrastructure that ALL user stories depend on

### 2.1 Type Definitions

- [ ] T015 Create `src/app/types/aggregate.types.ts` with TypeScript interfaces:
  - `WeatherData`, `NewsArticle`, `TwitterPost`, `AggregateResponse`
  - See [data-model.md](data-model.md#core-entities) for specifications
- [ ] T016 [P] Create `src/app/types/parameters.types.ts` with:
  - `QueryParameters` interface with validation
  - `DashboardError` interface
  - `UIState` interface

### 2.2 Utility Functions & Validation

- [ ] T017 Create `src/app/utils/validators.ts`:
  - `validateDate(dateString): boolean` - ISO 8601 validation
  - `validateCountry(country): boolean` - non-empty string
  - `validateCompany(company): boolean` - non-empty string
  - `validateCategory(category): boolean` - non-empty string
  - `validateSortBy(sortBy): boolean` - enum validation [popularity, relevance, publishedAt]
  - `validateQueryParameters(params): { valid: boolean; errors: string[] }` - full validation
- [ ] T018 [P] Create unit tests `src/app/utils/validators.test.ts`:
  - Test each validator function with valid/invalid inputs
  - Test validateQueryParameters with edge cases
  - Minimum 15 test cases, 90%+ coverage

- [ ] T019 Create `src/app/utils/formatters.ts`:
  - `formatTemperature(celsius, unit?: 'C'|'F'): string` → "15°C" or "59°F"
  - `formatDate(isoString): string` → "Mar 23, 2026"
  - `formatTime(isoString): string` → "3:30 PM UTC"
  - `formatNumber(num): string` → "1.2K" for 1200, "1.2M" for 1200000
  - `truncateText(text, maxLength): string` → adds "..." if truncated
  - `formatEngagement(num): string` → engagement metrics (likes, retweets)

- [ ] T020 [P] Create unit tests `src/app/utils/formatters.test.ts`:
  - Test each formatter with various inputs
  - Test edge cases (null, undefined, extreme values)
  - Minimum 20 test cases, 90%+ coverage

### 2.3 HTTP Client & API Service

- [ ] T021 Create `src/app/services/aggregateApi.ts`:
  - Initialize Axios instance with base URL from `REACT_APP_API_BASE_URL`
  - Set timeout to `REACT_APP_API_TIMEOUT` (5000ms)
  - Add error interceptor that transforms errors to `DashboardError`
  - Export typed function: `fetchAggregateData(params: QueryParameters): Promise<AggregateResponse>`
  - Handle HTTP errors (4xx, 5xx) and network timeouts gracefully

- [ ] T022 [P] Create unit tests `src/app/services/aggregateApi.test.ts`:
  - Mock Axios and test success response mapping
  - Test error handling (400, 500, timeout)
  - Test parameter serialization for URL
  - Test timeout logic
  - Minimum 12 test cases with MSW request mocking

### 2.4 React Query Setup

- [ ] T023 Create `src/app/services/queryClient.ts`:
  - Initialize QueryClient with stale time: 5 min, cache time: 10 min
  - Enable DevTools in development
  - Export configured `queryClient` instance

### 2.5 Custom Hooks

- [ ] T024 Create `src/app/hooks/useQueryParameters.ts`:
  - Custom hook to manage query parameters from URL and form state
  - `useQueryParameters(): { params: QueryParameters; setParams: (p: QueryParameters) => void; errors: string[] }`
  - Sync with URL via react-router or URLSearchParams
  - Validate on change

- [ ] T025 [P] Create unit tests `src/app/hooks/useQueryParameters.test.ts`:
  - Test parameter extraction from URL
  - Test parameter updates and URL sync
  - Test validation error handling
  - Test default values
  - Minimum 10 test cases, 85%+ coverage

- [ ] T026 Create `src/app/hooks/useAggregateData.ts`:
  - Custom hook wrapping React Query: `useQuery` from `@tanstack/react-query`
  - Signature: `useAggregateData(params: QueryParameters): { data: AggregateResponse | undefined; isLoading: boolean; error: DashboardError | null; refetch: () => void }`
  - Use query key: `['aggregateData', params]` for automatic cache invalidation on parameter change
  - Handle partial data (some fields null/empty)
  - Implement retry on network errors (exponential backoff)

- [ ] T027 [P] Create unit tests `src/app/hooks/useAggregateData.test.ts`:
  - Test successful data fetching
  - Test loading and error states
  - Test cache behavior (stale time, refetch)
  - Test error retry logic
  - Test parameter change triggers refetch
  - Test partial data handling
  - Minimum 15 test cases with MSW mocking, 85%+ coverage

### 2.6 Test Infrastructure

- [ ] T028 Create `tests/mocks/handlers.ts` (MSW request handlers):
  - Handler for `GET /api/aggregate` with success response
  - Handler for `GET /api/aggregate` with partial data (missing weather)
  - Handler for `GET /api/aggregate` with network error
  - Handler for `GET /api/aggregate` with timeout (delay > 5s)
  - Export `handlers` array for server configuration

- [ ] T029 [P] Create `tests/mocks/aggregateApi.mock.ts`:
  - Export mock data: `mockAggregateResponse` (complete success response)
  - Export `mockWeatherData` fixture
  - Export `mockNewsArticles` fixture (5+ articles)
  - Export `mockTwitterData` fixture
  - Export `mockErrorResponse` fixtures (400, 500, timeout)

- [ ] T030 [P] Create `tests/fixtures/testData.ts`:
  - Export `DEFAULT_QUERY_PARAMS` fixture
  - Export `INVALID_QUERY_PARAMS` fixture
  - Export UI state fixtures for different scenarios (loading, success, error)

---

## Phase 3: User Story 1 - View Aggregate Data Dashboard (P1)

**Acceptance**: User navigates to dashboard and sees all 4 data sections (weather, news category, news, Twitter) displayed responsively on desktop/tablet/mobile

### 3.1 Main Dashboard Component

- [ ] T031 [US1] Create `src/app/components/Dashboard/Dashboard.tsx`:
  - Parent component wrapping entire dashboard
  - Render `QueryClientProvider` with configured `queryClient`
  - Render `ErrorBoundary` component (to be created in T036)
  - Render `ParameterForm` component (P1.2, created in Phase 4)
  - Render `DataSections` component (P1.1, created in T033)
  - Use Tailwind CSS: `min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 p-4`
  - Responsive layout: `max-w-7xl mx-auto`
  - Include title: "Aggregate Data Dashboard"

- [ ] T032 [P] [US1] Create unit tests `src/app/components/Dashboard/Dashboard.test.tsx`:
  - Test component renders without crashing
  - Test QueryClientProvider is present
  - Test ErrorBoundary wraps children
  - Test dashboard layout structure (title, form, data sections)
  - Mock child components (ParameterForm, DataSections)
  - Minimum 8 test cases, 80%+ coverage

### 3.2 Data Sections Container

- [ ] T033 [US1] Create `src/app/components/Dashboard/DataSections.tsx`:
  - Container component that renders 4 data sections
  - Use `useAggregateData` hook to fetch data
  - Use `useQueryParameters` hook to get current params
  - Render loading state (LoadingState component, created in T034)
  - Display `WeatherCard` if weather data present (created in T037)
  - Display 2 separate `NewsSection` components:
    1. For `newsApiData` (main news)
    2. For `newsApiCategoryData` (category-filtered news)
  - Display `TwitterSection` component (created in T039)
  - Responsive grid: `grid md:grid-cols-2 lg:grid-cols-4 gap-4` with fallback to stacked
  - Handle partial data: show only non-null sections
  - Show error message if all data is null

- [ ] T034 [P] [US1] Create unit tests `src/app/components/Dashboard/DataSections.test.tsx`:
  - Test rendering all 4 sections with mock data
  - Test loading state display
  - Test partial data handling (missing weather, empty news)
  - Test error display when fetch fails
  - Test responsive grid layout
  - Minimum 10 test cases, 80%+ coverage

### 3.3 Loading State Component

- [ ] T035 [US1] Create `src/app/components/LoadingState/LoadingState.tsx`:
  - Display skeleton screens for 4 data sections
  - Use Tailwind CSS for skeleton animation: `animate-pulse`
  - Render 4 placeholder cards: `bg-gray-200 rounded-lg h-64 animate-pulse`
  - Text placeholders: `bg-gray-300 h-4 w-3/4 rounded mb-2 animate-pulse`

- [ ] T036 [P] [US1] Create unit tests `src/app/components/LoadingState/LoadingState.test.tsx`:
  - Test skeleton UI renders
  - Test animation classes present
  - Test 4 placeholder sections
  - Minimum 3 test cases, 85%+ coverage

### 3.4 Error Boundary Component

- [ ] T037 [US1] Create `src/app/components/Dashboard/ErrorBoundary.tsx`:
  - Class component extending `React.Component`
  - Implement `componentDidCatch(error, errorInfo)`
  - Display fallback UI with error message and reset button
  - Log error to console in development
  - Prevent entire app crash
  - Style: error box with red border, centered layout

- [ ] T038 [P] [US1] Create unit tests `src/app/components/Dashboard/ErrorBoundary.test.tsx`:
  - Test error boundary catches render errors
  - Test fallback UI displays
  - Test reset button functionality
  - Test child component renders when no error
  - Minimum 6 test cases, 80%+ coverage

### 3.5 Weather Card Component

- [ ] T039 [US1] Create `src/app/components/Dashboard/WeatherCard.tsx`:
  - Props: `weather: WeatherData`
  - Display layout:
    - Location: `<h3>className="text-lg font-bold">{weather.location}, {weather.country}</h3>`
    - Temperature: `<div className="text-4xl font-bold">{formatTemperature(weather.temperature)}</div>`
    - Condition: `<p className="text-gray-600">{weather.condition}</p>`
    - Grid for metrics (Humidity, Wind Speed, Pressure): 3 columns
  - Use `formatTemperature`, `formatNumber` utilities
  - Responsive: `sm:block hidden md:block` (hidden on very small)
  - Card styling: `bg-white rounded-lg shadow p-4`

- [ ] T040 [P] [US1] Create unit tests `src/app/components/Dashboard/WeatherCard.test.tsx`:
  - Test rendering with mock weather data
  - Test temperature formatting
  - Test all metrics display (humidity, wind, etc.)
  - Test responsive classes applied
  - Minimum 7 test cases, 80%+ coverage

---

## Phase 4: User Story 2 - Query Parameter Configuration (P1)

**Acceptance**: User submits parameter form and dashboard updates to show filtered data within 2 seconds

### 4.1 Parameter Form Component

- [ ] T041 [US2] Create `src/app/components/Dashboard/ParameterForm.tsx`:
  - Use `useQueryParameters` hook for state management
  - Form inputs:
    1. Date picker: `<input type="date" />` → ISO 8601 conversion
    2. Country dropdown: `<select>` with preset options (US, Greece, UK, etc.)
    3. Company input: `<input type="text" placeholder="Company name" />`
    4. Category dropdown: `<select>` with options (business, sports, health, etc.)
    5. SortBy dropdown: `<select>` with options (popularity, relevance, publishedAt)
  - Submit button: "Apply Filters" with loading state disabled during fetch
  - Validation: Show inline error messages for invalid inputs
  - Responsive: `flex flex-col md:flex-row gap-3 md:gap-2`
  - Use Tailwind CSS for inputs: `border rounded px-3 py-2 focus:outline-none focus:ring-2`

- [ ] T042 [P] [US2] Create unit tests `src/app/components/Dashboard/ParameterForm.test.tsx`:
  - Test form rendering with all input fields
  - Test parameter updates on input change
  - Test form submission calls setParams
  - Test validation errors display inline
  - Test invalid date shows error
  - Test default values populate
  - Minimum 12 test cases, 85%+ coverage

### 4.2 Integration: Dashboard with Parameters

- [ ] T043 [US2] Connect `ParameterForm` to `Dashboard` (update Dashboard.tsx):
  - Pass `useQueryParameters` hook state to child components
  - Verify form submission triggers data refetch via `useAggregateData`
  - Test user workflow: fill form → submit → data updates

- [ ] T044 [P] [US2] Create integration test `src/app/components/Dashboard/Dashboard.integration.test.tsx`:
  - User submits parameter form
  - Verify API is called with correct parameters
  - Verify DataSections re-render with new data
  - Test 2-second response time (mock delay)
  - Test loading state shows during fetch
  - Minimum 5 integration test cases

---

## Phase 5: User Story 3 - Data Presentation & Readability (P2)

**Acceptance**: Each data section (weather, news, Twitter) is visually distinct with clear formatting and key information highlighted

### 5.1 News Section Component

- [ ] T045 [US3] Create `src/app/components/Dashboard/NewsSection.tsx`:
  - Props: `articles: NewsArticle[]; title?: string` (e.g., "Latest News" or "Business News")
  - Display articles as cards:
    - Title: `<h4 className="text-base font-semibold">{article.title}</h4>`
    - Source + date: `<p className="text-sm text-gray-500">{article.source.name} • {formatDate(article.publishedAt)}</p>`
    - Description: `<p className="text-sm text-gray-700">{truncateText(article.description, 200)}</p>`
    - URL link: `<a href={article.url} target="_blank" className="text-blue-600 hover:underline">Read more →</a>`
  - Responsive grid: `grid gap-3` with articles in list on mobile, 2 columns on tablet
  - Handle empty array: Show "No articles found" message
  - Limit displayed articles to 10 (truncate longer lists with "Show more" button for future)

- [ ] T046 [P] [US3] Create unit tests `src/app/components/Dashboard/NewsSection.test.tsx`:
  - Test rendering multiple articles
  - Test article card structure (title, source, description, link)
  - Test empty state message
  - Test text truncation (description > 200 chars)
  - Test link attributes (target="_blank", href)
  - Test max 10 articles displayed
  - Minimum 10 test cases, 85%+ coverage

### 5.2 Twitter Section Component

- [ ] T047 [US3] Create `src/app/components/Dashboard/TwitterSection.tsx`:
  - Props: `tweet: TwitterPost | null`
  - If null, show "No posts available" message
  - Display tweet:
    - Author: `<p className="font-semibold">@{tweet.author.handle}</p>`
    - Content: `<p className="text-base mt-2">{tweet.text}</p>`
    - Timestamp: `<p className="text-sm text-gray-500">{formatTime(tweet.created_at)}</p>`
    - Engagement: `<div className="flex gap-4 mt-3"><span>{formatEngagement(tweet.engagement.likes)} Likes</span>...[Retweets][Replies]</div>`
  - Card styling: `bg-white rounded-lg shadow p-4 border-l-4 border-blue-400`

- [ ] T048 [P] [US3] Create unit tests `src/app/components/Dashboard/TwitterSection.test.tsx`:
  - Test rendering with mock tweet data
  - Test null/empty state
  - Test author and engagement display
  - Test engagement number formatting
  - Minimum 8 test cases, 85%+ coverage

### 5.3 Responsive Layout Refinement

- [ ] T049 [US3] Update DataSections layout for optimal readability:
  - Desktop (lg: 1024px+): 4-column grid (weather, news, news category, Twitter)
  - Tablet (md: 768px): 2-column grid (weather+Twitter, news+category)
  - Mobile (<768px): 1-column stack
  - Add section headers: `<h2 className="text-xl font-bold mb-3">Weather</h2>` for each section

- [ ] T050 [P] [US3] Create responsive layout tests:
  - Test layout classes rendered correctly at different viewport sizes
  - Test 4-column grid layout on desktop
  - Test 2-column on tablet
  - Test single column on mobile

---

## Phase 6: User Story 4 - Real-time Data Refresh (P3 - Optional for MVP)

**Acceptance**: User can manually refresh or enable auto-refresh to see updated data

### 6.1 Refresh Functionality

- [ ] T051 [US4] Add refresh button to `DataSections` component:
  - Button: "🔄 Refresh" with loading state (spinner while fetching)
  - Call `refetch()` from `useAggregateData`
  - Disable button during fetch
  - Show success toast: "Data refreshed successfully" (if not already shown)

- [ ] T052 [P] [US4] Create unit tests for refresh button:
  - Test button renders
  - Test click calls refetch
  - Test loading state during fetch
  - Test disabled state during fetch

### 6.2 Auto-Refresh (Optional Enhancement)

- [ ] T053 [US4] Create `useAutoRefresh` hook:
  - Props: `enabled: boolean; intervalMs: number`
  - Automatically call `fetchAggregateData` on interval
  - Stop on unmount
  - Gracefully handle errors (show subtle toast, don't break UI)

- [ ] T054 [P] [US4] Create unit tests for auto-refresh:
  - Test interval timer starts on enabled=true
  - Test cleanup on unmount
  - Test error handling during auto-refresh

### 6.3 Refresh UI Controls

- [ ] T055 [US4] Add auto-refresh toggle to `ParameterForm`:
  - Checkbox: "🔄 Auto-refresh" 
  - Interval selector: "Refresh every [5/10/30] seconds"
  - Only show interval selector if auto-refresh enabled

- [ ] T056 [P] [US4] Create unit tests for auto-refresh toggle:
  - Test checkbox toggles auto-refresh state
  - Test interval selector appears/disappears

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Error handling, performance optimization, documentation, and code quality

### 7.1 Error Handling & User Feedback

- [ ] T057 Create `src/app/components/Dashboard/ErrorMessage.tsx`:
  - Display `DashboardError` with user-friendly message
  - Show retry button for retryable errors
  - Styling: `bg-red-50 border-l-4 border-red-500 p-4 rounded`

- [ ] T058 [P] Create unit tests for error message component:
  - Test error displays with message
  - Test retry button present for retryable=true
  - Test retry button absent for retryable=false

- [ ] T059 Create toast notification utility `src/app/utils/toast.ts`:
  - `showToast(message: string; type: 'success'|'error'|'info'): void`
  - Auto-dismiss after 3 seconds
  - CSS transition for fade in/out

- [ ] T060 [P] Create unit tests for toast utility:
  - Test toast displays and dismisses
  - Test different types (success, error, info)

### 7.2 Performance Optimization

- [ ] T061 Add React.memo to pure components:
  - `WeatherCard`, `NewsSection`, `TwitterSection`
  - Prevent unnecessary re-renders
  - Add `displayName` for easier debugging

- [ ] T062 [P] Implement code splitting for Dashboard:
  - Lazy load `Dashboard` component using `React.lazy` if needed
  - Add `Suspense` fallback

### 7.3 Accessibility (a11y)

- [ ] T063 Add accessibility attributes:
  - Form labels with `htmlFor` attributes
  - ARIA labels for icon buttons
  - Semantic HTML markup (use `<button>`, not `<div>`)
  - Keyboard navigation support (Tab, Enter)

- [ ] T064 [P] Create accessibility tests:
  - Test form is keyboard navigable
  - Test ARIA labels present
  - Test semantic HTML

### 7.4 Documentation

- [ ] T065 Create `DASHBOARD.md` component documentation:
  - Overview of dashboard architecture
  - Component hierarchy diagram (text)
  - Setup and usage instructions
  - Testing guide

- [ ] T066 [P] Update JSDoc comments in all components and services:
  - Document props, return types, side effects
  - Add examples for complex functions

### 7.5 Code Quality & Type Safety

- [ ] T067 Run TypeScript compiler: `npm run type-check` (or `tsc --noEmit`)
  - Ensure zero TypeScript errors
  - Verify all types properly exported/imported

- [ ] T068 [P] Run ESLint: `npm run lint`
  - Fix all linting errors
  - Ensure React hooks rules satisfied (`eslint-plugin-react-hooks`)

- [ ] T069 Run final test suite: `npm test -- --coverage`
  - Ensure 80%+ coverage on components
  - Ensure 90%+ coverage on utils/services/hooks
  - Fix any failing tests

### 7.6 Build & Bundle Check

- [ ] T070 [P] Run production build: `npm run build`
  - Ensure no build errors
  - Check bundle size (target: <200KB for dashboard feature)
  - Verify no console errors

- [ ] T071 [P] Performance testing:
  - Verify LCP < 2.5s (via Lighthouse or DevTools)
  - Verify CLS < 0.1
  - Verify TTI < 3s
  - Run performance budget check if configured

### 7.7 Final Integration & Verification

- [ ] T072 Manual testing checklist:
  - [ ] Desktop (1024px) layout correct, all sections visible
  - [ ] Tablet (768px) layout responsive, usable
  - [ ] Mobile (320px) layout responsive, touch-friendly
  - [ ] Form submission updates data
  - [ ] Error handling works (mock backend error)
  - [ ] Loading states display
  - [ ] Parameter validation works (invalid input shows error)
  - [ ] Refresh button works (refetch triggers)
  - [ ] No console errors or warnings
  - [ ] Accessibility: keyboard navigation works

- [ ] T073 [P] Run full integration test suite:
  - All unit tests pass
  - All integration tests pass
  - MSW mocks working correctly
  - No flaky tests

- [ ] T074 Documentation:
  - Update README with dashboard feature
  - Update CONTRIBUTING.md if needed
  - Verify quickstart.md still accurate

---

## Summary

| Phase | Purpose | Task Range | Est. Tasks | Status |
|-------|---------|-----------|-----------|--------|
| 1 | Setup & Config | T001-T014 | 14 | Setup |
| 2 | Foundational | T015-T030 | 16 | Blocking all stories |
| 3 | US1 (P1) | T031-T040 | 10 | MVP core feature |
| 4 | US2 (P1) | T041-T044 | 4 | MVP parameter handling |
| 5 | US3 (P2) | T045-T050 | 6 | Enhanced UX |
| 6 | US4 (P3) | T051-T056 | 6 | Optional refresh |
| 7 | Polish | T057-T074 | 18 | Quality & performance |
| | **TOTAL** | **T001-T074** | **74 tasks** | |

**MVP Scope (Priority)**: Complete Phases 1-4 + Part of 7 (error handling, tests, docs)
- Estimated: 48 tasks (Phases 1-4 = 44 core tasks + 4 essential polish tasks)
- Timeline: 2-3 weeks for full feature + 1 week for MVP-only
- Test coverage target: 80%+ overall, 90%+ for sensitive paths (validation, error handling)

**Independent Testing Per Story**:
- **US1 Testable**: Phase 2 + Phase 3 complete (API service + Dashboard + DataSections rendering)
- **US2 Testable**: US1 + Phase 4 complete (form input + parameter updates + data refetch)
- **US3 Testable**: US1-2 + Phase 5 complete (styled components for each data type)
- **US4 Testable**: US1-3 + Phase 6 complete (refresh button + auto-refresh)
- **All Testable**: All phases complete + Phase 7 polish passes QA

---

## Execution Guide

### Start Here (MVP)
1. Complete Phase 1 (Setup) - 1 hour
2. Complete Phase 2 (Foundational) - 1-2 days
3. Complete Phase 3 (US1) - 1-2 days
4. Complete Phase 4 (US2) - 0.5-1 day
5. Complete Phase 7 partial (errors, tests, docs) - 1 day

**MVP Ready**: 4-6 days

### Then (Post-MVP)
6. Complete Phase 5 (US3) - 1 day
7. Complete Phase 6 (US4) - 1 day
8. Complete Phase 7 remaining - 1 day (polish, optimization, accessibility)

**Full Feature**: 2-3 weeks total

### Testing Strategy
- **During Development** (TDD): Write tests as you implement each task
- **Per-Phase**: Run `npm test` after each phase completes
- **Pre-Commit**: Run `npm test`, `npm run lint`, `npm run type-check`
- **Pre-Merge**: `npm run build` + full test suite + manual checklist (T072)

### Success Criteria (from Spec)
- ✅ SC-001: 3-second load time
- ✅ SC-002: Responsive on 320px, 768px, 1024px
- ✅ SC-003: 2-second parameter update
- ✅ SC-004: 95% API success rate
- ✅ SC-005: Error messages < 500ms
- ✅ SC-006: Graceful degradation for partial data
- ✅ SC-007: 4+/5 user satisfaction
- ✅ SC-008: Performance metrics (LCP, CLS, TTI)
