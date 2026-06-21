# Implementation Tasks: React Frontend for .NET 8 Backend

**Feature**: Build React Frontend  
**Branch**: `003-frontend-build`  
**Created**: March 26, 2026  
**Spec**: [spec.md](spec.md) | **Plan**: [plan.md](plan.md)  
**Status**: Ready for Implementation

---

## Task Overview

**Total Tasks**: 87 implementation tasks across 8 phases  
**Estimated Duration**: 3-4 weeks (MVP in 1-2 weeks)  
**MVP Scope**: Phases 1-4 + Phase 7 (testing/a11y) = 56 tasks  
**Test Coverage Target**: 80%+ across components and utilities  
**Key Success Metrics**: SC-001 through SC-015

### User Story Mapping

- **Phase 3**: User Story 1 (P1) - View Aggregated Data Dashboard - 12 tasks
- **Phase 4**: User Story 2 (P1) - Manage Query Parameters CRUD - 10 tasks
- **Phase 5**: User Story 3 (P1) - Navigate Between Pages - 8 tasks
- **Phase 6**: User Story 4 (P2) - View and Export Data - 6 tasks
- **Phase 7**: User Story 5 (P2) - Responsive Design - 8 tasks
- **Phase 8**: Cross-cutting & Polish - 9 tasks

---

## Phase 1: Project Setup & Infrastructure (12 tasks, 2-3 hours)

### Setup Tasks

- [ ] T001 Create Vite + React 18 + TypeScript project with `npm create vite@latest`
- [ ] T002 [P] Install core dependencies: react@18, react-dom@18, react-router-dom@6, axios, tailwindcss, typescript
- [ ] T003 [P] Install dev dependencies: @testing-library/react, @testing-library/jest-dom, jest, ts-jest, vite, @types/react
- [ ] T004 [P] Install accessibility & testing tools: msw, axe-core, @axe-core/react, jest-axe
- [ ] T005 Configure TypeScript (tsconfig.json) with strict mode enabled
- [ ] T006 Configure Vite (vite.config.ts) with React plugin and path aliases (@/components, @/services, etc.)
- [ ] T007 Configure Tailwind CSS (tailwind.config.js, postcss.config.js) with accessibility color palette
- [ ] T008 Configure Jest (jest.config.js) with TypeScript support and RTL configuration
- [ ] T009 Create .env.development and .env.production with VITE_API_URL configuration
- [ ] T010 [P] Create project directory structure: src/{components,hooks,services,types,utils,context,pages}, tests/{unit,components,integration,accessibility}
- [ ] T011 Create initial GitHub issue templates and contribution guidelines
- [ ] T012 Create README.md with setup, dev, build, test, and deployment instructions

---

## Phase 2: Foundational Services & Utilities (14 tasks, 3-4 hours)

### API & Data Service Layer

- [ ] T013 [P] Create src/services/api/aggregateClient.ts - Axios instance with base URL, interceptors, error handling
- [ ] T014 [P] Create src/services/api/types.ts - TypeScript interfaces for AggregateResponse, WeatherData, NewsData, TwitterData, APIError
- [ ] T015 [P] Create src/services/validation/queryValidator.ts - Date format validation, parameter validation functions
- [ ] T016 [P] Create src/services/storage/queryCache.ts - localStorage/IndexedDB caching with 24-hour TTL and 30-day hard delete logic
- [ ] T017 [P] Create src/services/storage/savedQueries.ts - CRUD operations for saved query parameter sets
- [ ] T018 [P] Create src/utils/dateFormat.ts - ISO 8601 date parsing, formatting, and validation utilities
- [ ] T019 [P] Create src/utils/cacheExpiry.ts - TTL calculation, expiration check, timestamp management
- [ ] T020 [P] Create src/utils/queryString.ts - URL query parameter serialization, deserialization, syncing
- [ ] T021 [P] Create src/utils/fileExport.ts - JSON export functionality with proper formatting
- [ ] T022 Create src/types/api.ts - Re-export API types from aggregateClient.ts
- [ ] T023 Create src/types/models.ts - Domain model interfaces (QueryParameters, SavedQuery, UIState, APIError)
- [ ] T024 Create src/types/index.ts - Barrel export for all types
- [ ] T025 [P] Create tests/mocks/handlers.ts - MSW request handlers for /api/aggregate endpoint
- [ ] T026 [P] Create tests/mocks/fixtures.ts - Sample API response data for testing

### Core Utilities Tests

- [ ] T027 [P] Write unit tests for dateFormat.ts (parsing, formatting, validation)
- [ ] T028 [P] Write unit tests for cacheExpiry.ts (TTL calculation, expiration checks)
- [ ] T029 [P] Write unit tests for queryString.ts (serialization, deserialization)
- [ ] T030 [P] Write unit tests for queryValidator.ts (date, parameter validation)
- [ ] T031 [P] Write unit tests for fileExport.ts (JSON export with proper formatting)

---

## Phase 3: User Story 1 - View Aggregated Data Dashboard (P1, 12 tasks, 4-6 hours)

### Dashboard Components

- [ ] T032 [US1] Create src/types/models.ts with QueryParameters interface (date, sortBy, company, country, category, url)
- [ ] T033 [US1] Create src/context/QueryContext.tsx - Context for form state management (parameters, validation errors)
- [ ] T034 [US1] Create src/context/UIContext.tsx - Context for loading, error, notification states
- [ ] T035 [US1] Create src/hooks/useAggregateData.ts - Custom hook for API calls, response caching, error handling (SR-002)
- [ ] T036 [US1] Create src/components/DashboardForm.tsx - Form component with 6 input fields (date, sortBy, company, country, category, url) with default values (FR-001)
- [ ] T037 [US1] Create src/components/ResultsSection.tsx - Container component for displaying aggregated results with loading state (FR-004, FR-009)
- [ ] T038 [US1] Create src/components/WeatherCard.tsx - Display weather data with temperature, conditions, attributes (FR-004, SC-001)
- [ ] T039 [US1] Create src/components/NewsSection.tsx - Display news articles with title, description, source, publish date (FR-004)
- [ ] T040 [US1] Create src/components/TwitterSection.tsx - Display tweet data with text, author, engagement metrics (FR-004)
- [ ] T041 [US1] Create src/components/LoadingSpinner.tsx - Loading indicator component shown during API requests (FR-009)
- [ ] T042 [US1] Implement error handling and display user-friendly error messages (FR-010, SC-005)
- [ ] T043 [US1] Create src/pages/DashboardPage.tsx - Main dashboard page layout and page-level state management
- [ ] T044 [US1] Write integration tests for full dashboard data fetch flow (fetch → display)
- [ ] T045 [US1] Add keyboard navigation and accessibility attributes to dashboard form and results (FR-016)

---

## Phase 4: User Story 2 - Manage Query Parameters with CRUD Operations (P1, 10 tasks, 4-5 hours)

### Saved Queries Management

- [ ] T046 [US2] Create src/types/models.ts with SavedQuery interface (name, parameters, createdDate, lastUsedDate)
- [ ] T047 [US2] Create src/hooks/useSavedQueries.ts - Custom hook for CRUD operations on saved queries (read, create, update, delete)
- [ ] T048 [US2] Implement localStorage persistence for saved queries with error handling (FR-005, FR-006, FR-014, SC-002)
- [ ] T049 [US2] Create src/components/SavedQueriesPage.tsx - Page displaying list of saved queries with CRUD actions (FR-006)
- [ ] T050 [US2] Create src/components/QueryListItem.tsx - Card component for each saved query with use, edit, delete buttons (FR-006, FR-007)
- [ ] T051 [US2] Create save parameter modal component - Allow users to name and save current form state (FR-005)
- [ ] T052 [US2] Create edit query modal component - Allow updating saved query name and parameters (FR-006)
- [ ] T053 [US2] Implement "Use" button to populate form from saved query (FR-007)
- [ ] T054 [US2] Add empty state message when user has no saved queries (edge case)
- [ ] T055 [US2] Write unit and integration tests for saved queries CRUD operations with localStorage
- [ ] T056 [US2] Add accessibility attributes to saved queries page and modals (FR-016, ARIA labels for CRUD buttons)

---

## Phase 5: User Story 3 - Navigate Between Pages and Sections (P1, 8 tasks, 2-3 hours)

### Routing & Navigation

- [ ] T057 [US3] Set up React Router v6 with routes for /dashboard and /saved-queries (FR-008, FR-013)
- [ ] T058 [US3] Create src/components/Navigation.tsx - Top navigation bar with links to Dashboard and Saved Queries (FR-008)
- [ ] T059 [US3] Implement route-based page navigation without full page reloads (FR-013)
- [ ] T060 [US3] Implement active page highlight in navigation (FR-008)
- [ ] T061 [US3] Implement browser back-button navigation that restores form parameters from URL history (FR-027)
- [ ] T062 [US3] Add 404 page component with link back to dashboard (edge case)
- [ ] T063 [US3] Write integration tests for navigation between pages and state preservation (SC-006)
- [ ] T064 [US3] Add keyboard navigation for menu items and proper focus management (FR-016)

---

## Phase 6: User Story 4 - View and Export Data Results (P2, 6 tasks, 2-3 hours)

### Data Presentation & Export

- [ ] T065 [US4] Create src/utils/fileExport.ts - JSON export function with proper formatting (FR-011)
- [ ] T066 [US4] Add "Export to JSON" button to results section (FR-011)
- [ ] T067 [US4] Implement download functionality that generates file with timestamp (FR-011)
- [ ] T068 [US4] Test export with various data sizes and formats
- [ ] T069 [US4] Write component tests for export button and download behavior
- [ ] T070 [US4] Add screen reader announcements for export action (FR-016)

---

## Phase 7: User Story 5 - Responsive Design Across Devices (P2, 8 tasks, 3-4 hours)

### Mobile & Responsive Layout

- [ ] T071 [US5] Configure Tailwind CSS breakpoints for mobile (320px), tablet (768px), desktop (1024px, 1920px) (FR-012)
- [ ] T072 [US5] Implement mobile form layout - inputs stack vertically on mobile (320-480px) (FR-012)
- [ ] T073 [US5] Create hamburger navigation menu for mobile devices (FR-008, FR-012)
- [ ] T074 [US5] Implement responsive results layout - single column on mobile, multi-column on desktop (FR-010, FR-012)
- [ ] T075 [US5] Test and fix keyboard visibility on mobile devices (FR-012, edge case)
- [ ] T076 [US5] Test at multiple viewport sizes and fix horizontal scrolling (SC-003)
- [ ] T077 [US5] Write responsive design tests using viewport testing tools
- [ ] T078 [US5] Verify touch targets are large enough on mobile (FR-012, accessibility)

---

## Phase 8: Testing, Accessibility, Caching & Polish (20 tasks, 5-7 hours)

### Offline Support & Caching

- [ ] T079 Implement API response caching with localStorage/IndexedDB (FR-018, SC-012)
- [ ] T080 [P] Add "Last updated" timestamp display on cached results (FR-021)
- [ ] T081 [P] Implement 24-hour cache expiration logic (FR-021, SC-013)
- [ ] T082 [P] Implement 30-day hard delete for old cache entries (FR-023, SC-013)
- [ ] T083 [P] Create src/components/CacheManagement.tsx - Settings panel with "Clear Cache" button (FR-022, SC-013)
- [ ] T084 [P] Create src/components/OfflineIndicator.tsx - Visual indicator of offline status (FR-019)
- [ ] T085 [P] Create src/hooks/useOnlineStatus.ts - Detect network connectivity status
- [ ] T086 [P] Display offline indicator and disable API calls when offline (FR-019, SC-012)
- [ ] T087 [P] Write tests for offline flows and cache expiration logic (5 tests)

### Accessibility Testing & Compliance

- [ ] T088 Run axe-core automated accessibility audit (SC-011)
- [ ] T089 Run Lighthouse accessibility audit (SC-011)
- [ ] T090 Fix all critical and high-priority accessibility violations (SC-011)
- [ ] T091 Test keyboard-only navigation on all pages (FR-016)
- [ ] T092 Verify color contrast ratios meet 4.5:1 minimum for all text (FR-015)
- [ ] T093 Test screen reader compatibility (NVDA, JAWS simulation)
- [ ] T094 Write jest-axe tests for all components (FR-016, SC-011)
- [ ] T095 Create accessibility testing guide for contributors

### URL State Synchronization

- [ ] T096 Create src/hooks/useQueryParameters.ts - Hook to sync form state with URL query params (FR-025)
- [ ] T097 Implement URL parameter encoding/decoding for safe sharing (FR-025, FR-026)
- [ ] T098 Test bookmarking functionality with URL parameters restored correctly (FR-026, SC-015)
- [ ] T099 Test back-button navigation through parameter changes (FR-027, SC-015)
- [ ] T100 Write integration tests for URL state sync (5 parameter change sequences)

### Component Test Coverage

- [ ] T101 Write component tests for Dashboard.tsx - rendering, state management, user interactions
- [ ] T102 Write component tests for DashboardForm.tsx - form input, validation, submission
- [ ] T103 Write component tests for ResultsSection.tsx - data display, loading/error states
- [ ] T104 Write component tests for WeatherCard.tsx, NewsSection.tsx, TwitterSection.tsx
- [ ] T105 Write component tests for SavedQueriesPage.tsx - CRUD operations, empty state
- [ ] T106 Write component tests for Navigation.tsx - routing, active links, accessibility
- [ ] T107 Write component tests for ErrorBoundary.tsx - error handling, recovery
- [ ] T108 Write snapshot tests for major components (verify against regressions)

### Documentation & Setup

- [ ] T109 Create src/README.md explaining component structure and component APIs
- [ ] T110 Create src/hooks/README.md documenting all custom hooks and their usage
- [ ] T111 Create src/services/README.md documenting API client, storage, validation services
- [ ] T112 Create TESTING.md with test setup, writing tests, running test suite
- [ ] T113 Create ACCESSIBILITY.md with keyboard shortcuts, WCAG compliance, testing procedures
- [ ] T114 Create DEPLOYMENT.md with build, environment setup, production deployment steps
- [ ] T115 Update main README.md with quick start, feature overview, architecture diagram

### Browser Compatibility & Performance

- [ ] T116 Test on Chrome/Chromium 90+ - verify all features, rendering, input handling (FR-024)
- [ ] T117 Test on Firefox 88+ - verify layout, form behavior, keyboard navigation (FR-024)
- [ ] T118 Test on Safari 14+ - verify flexbox, grid, CSS features, form inputs (FR-024)
- [ ] T119 Test on Edge 90+ - verify parity with Chrome (FR-024)
- [ ] T120 Profile performance with Lighthouse - optimize bundle size, FCP, LCP targets (SC-008)
- [ ] T121 Test 60fps performance during user interactions (scrolling, typing, button clicks) (SC-008)
- [ ] T122 Measure and verify <5 second data fetch to results display (SC-001)
- [ ] T123 Measure and verify <500ms page navigation (SC-006)
- [ ] T124 Measure and verify <100ms local storage operations (SC-007)
- [ ] T125 Run Webpack Bundle Analyzer - identify and remove large dependencies

### Error Handling & Edge Cases

- [ ] T126 Test backend 500 error response - display user-friendly error message (SC-005, edge case)
- [ ] T127 Test network timeout - show timeout error and retry option (SC-005, edge case)
- [ ] T128 Test localStorage quota exceeded - notify user and suggest cleanup (edge case)
- [ ] T129 Test invalid parameter submission (empty date, bad format) - frontend validation (FR-003, edge case)
- [ ] T130 Test empty results - display appropriate empty state (edge case)
- [ ] T131 Test invalid URL navigation - show 404 page (edge case)
- [ ] T132 Test with slow network - verify loading states visible and UX acceptable (SC-001)
- [ ] T133 Test API response with missing/malformed data fields

### Final QA & Release

- [ ] T134 Achieve 80%+ code coverage across components and utilities (SC-009)
- [ ] T135 Run full test suite and verify all tests passing (green build)
- [ ] T136 Perform manual end-to-end testing on all user stories (all 5 US)
- [ ] T137 Verify all success criteria metrics are met (SC-001 through SC-015)
- [ ] T138 Run security audit (npm audit, check dependencies for vulnerabilities)
- [ ] T139 Create CHANGELOG.md documenting features, fixes, breaking changes
- [ ] T140 Tag release version in git with feature completion notes
- [ ] T141 Deploy to staging/preview environment
- [ ] T142 Collect stakeholder feedback and document for Phase 2 enhancements

---

## Task Dependency Graph

### Critical Path (MVP Minimum)

```
Setup (T001-T012) 
  ↓
Foundations (T013-T031)
  ├→ Dashboard (T032-T045) [US1]
  ├→ Saved Queries (T046-T056) [US2]
  ├→ Routing (T057-T064) [US3]
  ↓
Testing & A11y (T088-T115)
  ↓
Browser Compat & Performance (T116-T125)
  ↓
Final QA (T134-T142)
```

### Parallel Execution Opportunities

**Phase 1**: All 12 tasks can run in parallel (no dependencies)
**Phase 2**: All 14 tasks can run in parallel (only depend on Phase 1)
**Phase 3**: Dashboard components can build in parallel after Context/Hooks created (T034-T035 before T036+)
**Phase 4**: Saved Queries components mostly parallel after hooks (T047 before T049-T053)
**Phase 5**: Navigation setup (T057) before component implementation (T058-T064)
**Phase 6-8**: Many tests can run in parallel; dependencies only where one component depends on another's API

### Suggested Sequential Execution (Risk-minimized MVP)

1. **T001-T012** (Setup): Serial - must complete first
2. **T013-T031** (Foundations): Parallel - all independent
3. **T032, T033, T034, T035** (Context/Hooks): Serial - foundation for component layer
4. **T036-T045** (Dashboard components): Parallel - Form → Results display → Tests
5. **T046, T047, T048** (Saved Queries setup): Serial - foundation
6. **T049-T056** (Saved Queries components): Parallel - components and tests
7. **T057-T064** (Routing): Serial - depends on component structure
8. **T088-T107** (Testing & A11y): Parallel - can run alongside earlier phases
9. **T116-T142** (Final validation): Serial - performed last

---

## Success Criteria Mapping

| Task(s) | Criterion | Measurement |
|---------|-----------|------------|
| T032-T045 | SC-001 | <5 second fetch-to-display time |
| T046-T056 | SC-002 | 100% reliable save/load/delete with no data loss |
| T071-T078 | SC-003 | 320-1920px renders correctly, zero scrolling |
| T046, T101-T106 | SC-004 | 100% valid inputs accepted, invalid rejected |
| T042, T126-T133 | SC-005 | All error types display user-friendly messages |
| T057-T064 | SC-006 | <500ms navigation, zero errors |
| T048, T056 | SC-007 | <100ms localStorage operations |
| T120-T122 | SC-008 | 60fps during interactions |
| T101-T108, T134 | SC-009 | 80%+ code coverage |
| T109-T115 | SC-010 | Complete setup, API, testing docs |
| T088-T095 | SC-011 | Passes axe/Lighthouse with zero violations |
| T079-T087 | SC-012 | Offline display works reliably |
| T079-T087 | SC-013 | Cache expires correctly (24h/30d) |
| T116-T119 | SC-014 | Passes on Chrome 90+, Firefox 88+, Safari 14+, Edge 90+ |
| T096-T100 | SC-015 | URL params sync, bookmarking works, back-button correct |

---

## Effort Estimates

| Phase | Description | Tasks | Est. Hours | Key Risks |
|-------|-------------|-------|-----------|-----------|
| 1 | Setup | 12 | 2-3 | Configuration issues, dependency conflicts |
| 2 | Foundations | 14 | 3-4 | API contract mismatch, caching complexity |
| 3 | Dashboard (US1) | 12 | 4-6 | API integration, error handling |
| 4 | CRUD (US2) | 10 | 4-5 | localStorage edge cases, modal UX |
| 5 | Routing (US3) | 8 | 2-3 | URL state sync, browser history |
| 6 | Export (US4) | 6 | 2-3 | File download, different data formats |
| 7 | Responsive (US5) | 8 | 3-4 | Mobile testing, performance on slow devices |
| 8 | Testing/A11y/Polish | 20 | 5-7 | Accessibility edge cases, browser compatibility |
| **TOTAL** | **All phases** | **87** | **26-35 hours** | See per-phase details |

**MVP (Phases 1-5 + subset of 8)**: ~18-22 hours = 2-3 developer days  
**Full feature (all phases)**: ~26-35 hours = 3-4.5 developer days

---

## Implementation Strategy

### Week 1: MVP Foundation
- Days 1-2: Setup + Foundations (T001-T031)
- Days 2-3: Dashboard UI + API integration (T032-T045)
- Days 3: Saved Queries basic CRUD (T046-T056)
- Day 3-4: Routing between pages (T057-T064)
- End of Day 4: Accessible, basic MVP working

### Week 2: Polish & Testing
- Days 5-6: Offline support + Caching (T079-T087)
- Days 6-7: Component tests + Accessibility (T088-T115)
- Days 7-8: Browser testing + Performance optimization (T116-T125)
- Day 8: Final QA + Documentation (T134-T142)

### Week 3+ (Optional Enhancements)
- Phase 6: Enhanced data visualization, export capabilities
- Phase 7: Responsive design refinements, mobile optimization
- Additional features: Dark mode, user preferences, analytics

---

## Quality Gates

- ✅ **Gate 1 (After Phase 2)**: All utilities pass tests, API client working, no TypeErrors
- ✅ **Gate 2 (After Phase 5)**: All 3 P1 user stories functioning end-to-end, navigation working
- ✅ **Gate 3 (After Phase 8)**: 80%+ test coverage, accessibility audit passing, performance targets met, all success criteria verified

---

## Notes for Implementation

1. **Start with T001-T012**: Don't skip setup; proper configuration pays dividends later
2. **API mocking (T025-T026, tests)**: Use MSW from day one; don't wait for backend to be available
3. **Accessibility from the start**: Don't treat WCAG compliance as an afterthought; build semantic HTML and ARIA from component creation
4. **Testing strategy**: Write tests as you build each component (TDD or test-after); don't batch testing at the end
5. **Browser testing**: Test on real devices/browsers, not just DevTools emulation (T116-T119)
6. **Performance**: Profile early and often (Lighthouse, DevTools Performance tab); don't optimize prematurely without measurement
7. **Documentation**: Update docs as you implement (not last); makes developer onboarding smooth
8. **Version control**: Commit after each major task (or every 2-3 small tasks); keeps history clean
9. **Stakeholder demos**: After Phase 5 is a good checkpoint to show working MVP to stakeholders
10. **Deferred features**: Don't implement dark mode, execution history, or auth (mentioned as out-of-scope); focus on MVP requirements only

---

## Independent Testability per User Story

### US1 (Dashboard) - Independent Test Script
1. Load `/dashboard`
2. Enter: date="2024-01-01", sortBy="relevance", company="Apple", country="Greece", category="business", url="[sample]"
3. Click "Fetch Data" → Verify loading spinner appears
4. Verify results display with weather/news/Twitter sections
5. Verify "Last updated" timestamp
6. Test error path: disconnect API, click fetch, verify error message
7. ✅ **Result**: US1 is independently testable without US2 or US3

### US2 (CRUD) - Independent Test Script
1. Load `/dashboard`
2. Enter parameters, click "Save Parameters"
3. Enter name "Test Query 1", save
4. Navigate to `/saved-queries`
5. Verify "Test Query 1" appears in list
6. Click "Use", verify form populates
7. Click "Edit", change name to "Test Query 1 Updated", save
8. Verify list updated
9. Click "Delete", confirm, verify removed
10. ✅ **Result**: US2 is independently testable without US1 (uses mock data)

### US3 (Routing) - Independent Test Script
1. Load app on `/dashboard`
2. Click "Saved Queries" link → verify route changes to `/saved-queries`
3. Click "Dashboard" link → verify back on `/dashboard`
4. Manually navigate to `/invalid` → verify 404 page
5. Test back-button through 5+ route changes
6. ✅ **Result**: US3 is independently testable with minimal mocking

### US4 (Export) - Independent Test Script
1. Load `/dashboard`
2. Fetch sample data
3. Click "Export to JSON"
4. Verify file downloads with correct structure
5. Parse JSON and verify all data fields present
6. ✅ **Result**: US4 independently testable after US1

### US5 (Responsive) - Independent Test Script
1. Load dashboard at 320px width (mobile)
2. Verify form inputs stack vertically
3. Verify hamburger menu visible
4. Test touch interactions (buttons, inputs)
5. Resize to 768px (tablet) → verify 2-3 column layout
6. Resize to 1024px (desktop) → verify full layout
7. ✅ **Result**: US5 independently testable at component level (no backend needed)

---

**Ready for implementation. Start with Phase 1 Setup tasks. Happy coding!** 🚀
