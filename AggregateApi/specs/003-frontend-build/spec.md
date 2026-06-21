# Feature Specification: Build React Frontend for .NET 8 Backend

**Feature Branch**: `003-frontend-build`  
**Created**: March 26, 2026  
**Status**: Draft  
**Input**: User description: "Build a React frontend for an existing .NET 8 backend with CRUD views for resources exposed by the API and navigation between pages"

## Clarifications

### Session 2026-03-26

- Q: Does the React frontend need to meet WCAG accessibility compliance standards? → A: WCAG 2.1 Level AA (full accessibility with keyboard navigation, screen readers, color contrast, focus management, ARIA labels)
- Q: What is the expected offline capability of the frontend? → A: Full offline support—app caches API responses and allows viewing saved queries and previously fetched results without network; new API calls require connection
- Q: When should the app automatically clear or invalidate cached data? → A: Time-based expiration—cache entries expire after 24 hours; user can manually clear anytime; hard delete after 30 days regardless
- Q: What are the minimum browser version support requirements? → A: Modern browsers only—Chrome/Edge 90+, Firefox 88+, Safari 14+; assumes evergreen browser updates; no polyfills needed
- Q: Should form parameters be reflected in the URL for bookmarking and sharing? → A: URL-synced parameters—form values sync to URL query params; users can bookmark/share links; back-button restores form state

## User Scenarios & Testing

### User Story 1 - View Aggregated Data Dashboard (Priority: P1)

Users need a dashboard to view aggregated weather, news, and Twitter data fetched from the backend AggregateController. This is the core MVP feature that demonstrates the frontend can successfully consume the backend API.

**Why this priority**: This is the critical path MVP feature. Users must be able to see data returned by the backend to validate the integration works. Without this, the frontend has no purpose.

**Independent Test**: Can be fully tested by loading the dashboard, entering query parameters (date, sortBy, company, country, category, url), clicking "Get Data", and verifying weather/news/Twitter data displays correctly.

**Acceptance Scenarios**:

1. **Given** a user navigates to the dashboard, **When** the page loads, **Then** they see a form with input fields for date, sortBy, company, country, category, and url with sensible default values.
2. **Given** a user enters query parameters, **When** they click "Fetch Data", **Then** the dashboard shows a loading state while the request is in progress.
3. **Given** the backend successfully returns data, **When** the request completes, **Then** the weather, news, and Twitter sections display the returned data in an organized layout.
4. **Given** the backend returns an error, **When** the request completes, **Then** the dashboard displays a user-friendly error message with the error details.

---

### User Story 2 - Manage Query Parameters with CRUD Operations (Priority: P1)

Users need to create, read, update, and delete saved query parameter sets to avoid re-entering the same parameters repeatedly when fetching different aggregated data.

**Why this priority**: This P1 feature significantly improves usability by letting users save and reuse parameter combinations, reducing friction for repeated queries.

**Independent Test**: Can be fully tested by creating parameter sets, viewing saved sets in a list, updating a set's values, and deleting sets, verifying persistence across page reloads.

**Acceptance Scenarios**:

1. **Given** a user has filled in query parameters on the dashboard, **When** they click "Save Parameters", **Then** a modal appears allowing them to name the parameter set and save it locally.
2. **Given** a user navigates to a "Saved Queries" page, **When** the page loads, **Then** they see a list of all saved parameter sets with options to use, edit, or delete each one.
3. **Given** a user clicks "Use" on a saved parameter set, **When** the action completes, **Then** the dashboard form populates with those parameters and is ready to fetch data.
4. **Given** a user clicks "Edit" on a saved parameter set, **When** they modify values and click "Save", **Then** the changes persist and the updated set appears in the list.
5. **Given** a user clicks "Delete" on a parameter set, **When** they confirm deletion, **Then** the set is removed from the list and no longer appears.

---

### User Story 3 - Navigate Between Pages and Sections (Priority: P1)

Users need clear navigation to move between the main dashboard, saved queries page, and other sections without losing state or context.

**Why this priority**: Navigation is essential UX infrastructure. Without it, users cannot access all features. This is P1 because it unlocks access to differentiated features.

**Independent Test**: Can be fully tested by clicking navigation links, verifying correct pages load, and confirming parameter state is preserved when navigating back to the dashboard.

**Acceptance Scenarios**:

1. **Given** a user is on the dashboard page, **When** they click the "Saved Queries" navigation link, **Then** they are taken to the Saved Queries page and the navigation highlights the current page.
2. **Given** a user is on any page, **When** they click the "Dashboard" link in navigation, **Then** they return to the dashboard and any unsaved parameters are lost (expected behavior).
3. **Given** a user is on the Saved Queries page, **When** they click "Use" on a saved query, **Then** they are taken to the dashboard with the parameters populated and ready to fetch.
4. **Given** the app is accessed from different browsers/devices, **When** the user navigates between pages, **Then** saved queries persist (stored locally) and navigation works consistently.

---

### User Story 4 - View and Export Data Results (Priority: P2)

Users need clear, organized presentation of weather, news, and Twitter data with potential export capabilities for further analysis.

**Why this priority**: P2 because dashboard display is secondary to fetching data (P1), but important for usability once data is retrieved. Export is nice-to-have.

**Independent Test**: Can be tested by fetching data and verifying each section (weather, news, Twitter) displays distinct content correctly formatted, and export button generates a file.

**Acceptance Scenarios**:

1. **Given** data has been fetched from the API, **When** the results display, **Then** each data type (weather, news, Twitter) is in a separate visual section with appropriate formatting.
2. **Given** the results contain weather data, **When** displayed, **Then** temperature, conditions, and other weather attributes are clearly labeled and readable.
3. **Given** the results contain news articles, **When** displayed, **Then** each article shows title, description, source, and publish date in a card or list format.
4. **Given** the results contain Twitter data, **When** displayed, **Then** tweet text, author, and engagement metrics are clearly visible.
5. **Given** data is displayed on the results section, **When** user clicks "Export to JSON", **Then** a JSON file downloads with the complete data payload.

---

### User Story 5 - Responsive Design Across Devices (Priority: P2)

Users accessing the app on mobile, tablet, or desktop devices need a responsive interface that adapts layout, text size, and interaction areas appropriately for their screen size.

**Why this priority**: P2 because it's important for user experience but secondary to core functionality working correctly. Desktop MVP is sufficient for launch; mobile follows as a refinement.

**Independent Test**: Can be tested by viewing the app at multiple viewport sizes (320px, 768px, 1024px) and verifying layout adapts, text remains readable, and buttons/inputs are touch-friendly at small sizes.

**Acceptance Scenarios**:

1. **Given** a user accesses the dashboard on a mobile device (320-480px width), **When** the page loads, **Then** the form inputs stack vertically, buttons are large enough to tap, and navigation menus collapse into a hamburger menu.
2. **Given** a user accesses the dashboard on a tablet (768px width), **When** the page loads, **Then** the layout uses 2-3 columns where appropriate and remains fully functional.
3. **Given** a user accesses the dashboard on desktop (1024px+), **When** the page loads, **Then** the full layout with multiple columns displays with professional spacing.
4. **Given** a user resizes their browser window, **When** the width changes, **Then** the layout smoothly adapts without horizontal scrolling or text cutoff.
5. **Given** a user interacts with form fields on mobile, **When** typing or selecting values, **Then** the keyboard doesn't obscure critical UI and the interface remains usable.

---

### Edge Cases

- What happens when the backend is unavailable or returns a 500 error? → System displays user-friendly error message with retry option
- What happens if network request times out? → System shows timeout error and allows user to retry
- What happens if user's local storage quota is exceeded when saving queries? → System notifies user that storage is full and suggests deleting old queries
- What happens if user sends invalid parameter values (e.g., invalid date format)? → Frontend validates and shows error before sending to API
- What happens if user has no saved queries? → Saved Queries page shows empty state with helpful message encouraging user to save their first query
- What happens if user browses to an invalid URL? → 404 page displays with link back to dashboard

## Requirements

### Functional Requirements

- **FR-001**: Frontend MUST display a form with input fields for all parameters accepted by the AggregateController (date, sortBy, company, country, category, url)
- **FR-002**: Frontend MUST send HTTP GET requests to `/api/aggregate` with user-entered parameters and handle successful (200) and error responses (400, 500, etc.)
- **FR-003**: Frontend MUST validate that the date field contains a valid ISO 8601 date format before submitting to the API
- **FR-004**: Frontend MUST display aggregated data results in three organized sections: Weather Data, News Articles, and Twitter Data
- **FR-005**: Frontend MUST allow users to save parameter combinations with custom names to local browser storage
- **FR-006**: Frontend MUST allow users to view all saved parameter sets in a dedicated page with view, edit, and delete actions
- **FR-007**: Frontend MUST allow users to load a saved parameter set into the dashboard form with a single click
- **FR-008**: Frontend MUST provide navigation between at least two main pages: Dashboard and Saved Queries
- **FR-009**: Frontend MUST display a loading indicator while API requests are in progress
- **FR-010**: Frontend MUST display user-friendly error messages when API requests fail, including the error type and any details from the backend
- **FR-011**: Frontend MUST support exporting fetched results to a JSON file for user analysis
- **FR-012**: Frontend MUST be responsive and usable on screen sizes from 320px (mobile) to 1920px (desktop) width
- **FR-013**: Frontend MUST use client-side routing to enable navigation between pages without full page reloads
- **FR-014**: Frontend MUST persist saved queries in browser local storage for offline access and persistence across sessions
- **FR-015**: Frontend MUST comply with WCAG 2.1 Level AA accessibility standards including keyboard navigation for all interactive elements, screen reader support for all content, minimum 4.5:1 color contrast ratios, and descriptive ARIA labels
- **FR-016**: Frontend MUST support keyboard-only navigation with visible focus indicators and logical tab order throughout the application
- **FR-017**: Frontend MUST include semantic HTML and ARIA attributes for screen reader accessibility, particularly for form fields, data tables, and dynamic content updates
- **FR-018**: Frontend MUST cache API responses (weather, news, Twitter data) locally and display previously fetched results when the backend is unreachable or the user is offline
- **FR-019**: Frontend MUST display a visual indicator when operating in offline mode and clearly indicate which features require internet connection
- **FR-020**: Frontend MUST allow users to view all saved queries and previously cached results without an active internet connection
- **FR-021**: Frontend MUST automatically expire cached API responses after 24 hours and display a "Last updated" timestamp showing when the data was fetched
- **FR-022**: Frontend MUST provide a manual "Clear Cache" button allowing users to delete all cached results at any time
- **FR-023**: Frontend MUST automatically delete all cached data after 30 days regardless of when it was last accessed or refreshed
- **FR-024**: Frontend MUST be compatible with Chrome/Edge 90+, Firefox 88+, and Safari 14+ and use only native APIs available in these browser versions (no polyfills or transpilation to ES5)
- **FR-025**: Frontend MUST sync form parameter values to URL query parameters (e.g., `?date=2024-01-01&company=Apple`) and restore form state from URL on page load
- **FR-026**: Frontend MUST allow users to bookmark and share URLs containing query parameters; shared links restore all parameters when opened in a new browser/device
- **FR-027**: Frontend MUST implement browser back-button navigation that restores previous form parameter values from URL history

### Key Entities

- **QueryParameters**: Represents a set of search/filter parameters (date, sortBy, company, country, category, url) entered by the user
- **AggregateResponse**: DTO containing three data subsets returned by the API (WeatherData, NewsData, TwitterData) with all attributes from the backend response
- **SavedQuery**: A named collection of QueryParameters stored locally with metadata (name, createdDate, lastUsedDate) for user reference
- **UIState**: Manages client-side UI state including loading flags, error messages, current page/route, and form values
- **APIError**: Represents error responses from the backend with status code, message, and details for display to users

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can complete a full data fetch from initial dashboard load to viewing results in under 5 seconds on a standard internet connection (average 10 Mbps)
- **SC-002**: Saved parameter functionality allows users to save and reuse queries with 100% reliability (no data loss when saving/loading)
- **SC-003**: Dashboard is fully functional and renders correctly on screen sizes from 320px to 1920px width with zero horizontal scrolling or content cutoff
- **SC-004**: All form inputs validate correctly before API submission, with invalid inputs rejected 100% and valid inputs accepted 100% of the time
- **SC-005**: API errors are handled gracefully with user-friendly messages in 100% of error scenarios (network timeout, 4xx, 5xx responses)
- **SC-006**: Navigation between pages completes in under 500ms and has zero unhandled errors
- **SC-007**: Local storage operations (save/load/delete saved queries) complete in under 100ms and succeed 100% of the time
- **SC-008**: Component render performance maintains 60fps frame rate during user interactions (scrolling, typing, button clicks)
- **SC-009**: Code achieves minimum 80% test coverage for components and utility functions
- **SC-010**: Documentation is complete for component APIs, utility functions, and setup/development instructions for new contributors
- **SC-011**: Application passes automated WCAG 2.1 Level AA accessibility audit (axe-core, Lighthouse, WAVE tools) with zero critical and high-priority violations
- **SC-012**: Application successfully displays cached results when backend API is unreachable and clearly indicates offline status to the user; offline features work 100% reliably with data cached within the last 30 days
- **SC-013**: Cache expiration works correctly: entries are marked stale after 24 hours, hard-deleted after 30 days, and can be manually cleared by user with single click; timestamp accuracy is within 1 minute
- **SC-014**: Application functions correctly and passes all tests in Chrome 90+, Firefox 88+, Safari 14+, and Edge 90+; zero browser-specific rendering or functionality issues
- **SC-015**: URL query parameters accurately reflect all form values; bookmarked/shared links restore exact form state 100% reliably; browser back-button navigation works correctly for at least 5 sequential parameter changes

## Assumptions

- **Backend API is stable**: The .NET 8 backend AggregateController is available, functional, and returns responses matching the AggregateResponse schema
- **CORS is configured**: The backend has CORS enabled to allow requests from the frontend origin (localhost:3000 for development)
- **Browser APIs available**: Users have browsers that support ES2020+ JavaScript, localStorage API, and modern CSS (Grid, Flexbox)
- **No authentication required**: Initial MVP assumes the AggregateController endpoint is publicly accessible without authentication/authorization
- **Date format compatibility**: Backend expects and returns dates in ISO 8601 format (YYYY-MM-DDTHH:mm:ssZ)
- **Local storage sufficiency**: Users have at least 5MB of localStorage available for saved queries (typical for modern browsers)
- **React 18 and Node 18+**: Development environment has Node.js 18+ and React 18.x installed and configured

## Implementation Notes

- Consider using React Router v6 for client-side routing and navigation
- Consider using React Query or SWR for server state management and caching
- Consider using Tailwind CSS for styling to ensure responsive design consistency
- Consider using TypeScript for type safety with the backend API contract
- Consider using MSW (Mock Service Worker) for testing without backend dependency
- Environment variables should separate development/production API base URLs

## Open Questions

- Should the app support dark mode/light mode themes? (Not in MVP, can be added in P3)
- Should saved queries include execution history/timestamps? (Out of scope for MVP, P3 feature)
- Should there be user authentication/accounts? (No, MVP assumes public access)
- What's the maximum number of saved queries users should be able to store? (Recommend 50 as reasonable default, limited by localStorage)
