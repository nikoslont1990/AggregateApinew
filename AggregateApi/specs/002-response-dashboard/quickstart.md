# Quickstart: Response Dashboard UI

**Phase**: 1 (Design & Contracts)  
**Date**: March 23, 2026  
**Goal**: Get the dashboard running locally with all tests passing

## Prerequisites

- Node.js 18+ and npm 9+
- Backend running locally on `http://localhost:5000` (or configured API URL)
- Git with feature branch `002-response-dashboard` checked out
- VS Code with recommended extensions (TypeScript, ESLint, Jest)

---

## 1. Environment Setup (5 min)

### 1.1 Install Dependencies

```bash
cd frontend/modern-weather

# Install packages (or update if already installed)
npm install

# Verify React Query and testing libraries are present
npm list @tanstack/react-query @testing-library/react jest msw

# Install missing packages if needed:
npm install @tanstack/react-query @tanstack/react-query-devtools
npm install --save-dev @testing-library/react @testing-library/jest-dom @testing-library/user-event
npm install --save-dev jest @types/jest ts-jest
npm install --save-dev msw
npm install --save-dev jest-environment-jsdom
npm install --save-dev @babel/preset-typescript
```

### 1.2 Verify Configuration Files

Check that these files exist and are configured:

```bash
# TypeScript configuration
cat tsconfig.json
# Should have: "jsx": "react-jsx", "lib": ["ES2020", "DOM", "DOM.Iterable"]

# Jest configuration (should exist or be in package.json)
cat jest.config.js
# Should include: preset: "ts-jest", testEnvironment: "jsdom"

# ESLint (should exist)
cat .eslintrc.json
# Should include React plugin
```

### 1.3 Environment Variables (if needed)

Create `.env.local` in `frontend/modern-weather/`:

```
REACT_APP_API_BASE_URL=http://localhost:5000
REACT_APP_API_TIMEOUT=5000
```

---

## 2. Development Workflow (15 min)

### 2.1 Start Development Server

```bash
cd frontend/modern-weather

# Development mode with hot reload
npm run dev
# or
npm start

# Open browser to http://localhost:3000
```

### 2.2 Check Dashboard Route

The dashboard should be accessible at:
```
http://localhost:3000/dashboard
or
http://localhost:3000/dashboard?date=2026-03-23T15:30:00Z&country=US&company=Apple&category=business
```

If route doesn't exist yet, see **Section 3: Initial Implementation**.

### 2.3 Verify Backend Connection

In browser DevTools Console:
```javascript
// Test API connectivity
fetch('http://localhost:5000/api/aggregate?date=2026-03-23T15:30:00Z&country=US&company=Apple&category=business')
  .then(r => r.json())
  .then(data => console.log('Backend response:', data))
  .catch(e => console.error('Backend error:', e))
```

Expected output: `AggregateResponse` object with weather, news, Twitter data (or nulls if APIs unavailable).

---

## 3. Initial Implementation Checklist (for developers)

### 3.1 Directory Structure

Create the component hierarchy:

```bash
cd frontend/modern-weather/src

# Create directories
mkdir -p app/components/Dashboard
mkdir -p app/components/LoadingState
mkdir -p app/services
mkdir -p app/hooks
mkdir -p app/types
mkdir -p app/utils
mkdir -p tests/mocks
mkdir -p tests/fixtures
```

### 3.2 Key Files to Create (see task list)

**Components** (with tests):
- `Dashboard/Dashboard.tsx` + `.test.tsx`
- `Dashboard/ParameterForm.tsx` + `.test.tsx`
- `Dashboard/DataSections.tsx` + `.test.tsx`
- `Dashboard/WeatherCard.tsx` + `.test.tsx`
- `Dashboard/NewsSection.tsx` + `.test.tsx`
- `Dashboard/TwitterSection.tsx` + `.test.tsx`
- `Dashboard/ErrorBoundary.tsx` + `.test.tsx`
- `LoadingState/LoadingState.tsx` + `.test.tsx`

**Services** (with tests):
- `services/aggregateApi.ts` + `.test.ts`
- `services/queryClient.ts`

**Hooks** (with tests):
- `hooks/useAggregateData.ts` + `.test.ts`
- `hooks/useQueryParameters.ts` + `.test.ts`

**Utilities** (with tests):
- `utils/validators.ts` + `.test.ts`
- `utils/formatters.ts` + `.test.ts`

**Types**:
- `types/aggregate.types.ts`
- `types/parameters.types.ts`

**Test Infrastructure**:
- `tests/setup.ts` - Jest setup, MSW server initialization
- `tests/mocks/handlers.ts` - MSW HTTP handlers
- `tests/mocks/aggregateApi.mock.ts` - Mock data
- `tests/fixtures/testData.ts` - Test fixtures

### 3.3 Sample Component Skeleton

```typescript
// src/app/components/Dashboard/Dashboard.tsx
import React, { Suspense } from 'react';
import { QueryClientProvider } from '@tanstack/react-query';
import queryClient from '../../services/queryClient';
import ParameterForm from './ParameterForm';
import DataSections from './DataSections';
import ErrorBoundary from './ErrorBoundary';
import LoadingState from '../LoadingState/LoadingState';

export default function Dashboard(): React.ReactElement {
  return (
    <QueryClientProvider client={queryClient}>
      <ErrorBoundary>
        <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 p-4">
          <div className="max-w-7xl mx-auto">
            <h1 className="text-3xl font-bold mb-6 text-gray-900">Data Dashboard</h1>
            
            <ParameterForm />
            
            <Suspense fallback={<LoadingState />}>
              <DataSections />
            </Suspense>
          </div>
        </div>
      </ErrorBoundary>
    </QueryClientProvider>
  );
}
```

---

## 4. Running Tests (10 min)

### 4.1 Run All Tests

```bash
cd frontend/modern-weather

# Run tests in watch mode (recommended during development)
npm test -- --watch

# Run tests once and exit
npm test

# Run tests with coverage
npm test -- --coverage
```

### 4.2 Expected Test Output

```
PASS  src/app/hooks/useAggregateData.test.ts
  useAggregateData
    ✓ fetches and caches data (325ms)
    ✓ handles API errors gracefully (212ms)
    ✓ refetches on parameter change (298ms)

PASS  src/app/utils/validators.test.ts
  Parameter Validators
    ✓ validates date format correctly (5ms)
    ✓ validates country parameter (3ms)
    ✓ rejects invalid parameters (4ms)

PASS  src/app/components/Dashboard/Dashboard.test.tsx
  Dashboard Component
    ✓ renders with all sections (450ms)
    ✓ displays loading state while fetching (200ms)
    ✓ shows error when API fails (280ms)

Test Suites: 12 passed, 12 total
Tests:       87 passed, 87 total
Snapshots:   0 total
Time:        8.234 s
```

### 4.3 Debugging Tests

```bash
# Run single test file
npm test -- Dashboard.test.tsx

# Run with detailed output
npm test -- --verbose

# Debug test (opens inspector)
node --inspect-brk ./node_modules/.bin/jest --runInBand
```

---

## 5. Manual Testing Checklist

### 5.1 Desktop (1024px+)

- [ ] Dashboard loads without errors
- [ ] All 4 data sections visible (weather, news category, news, Twitter)
- [ ] Multi-column layout (e.g., 2 columns for news)
- [ ] Parameter form updates data when submitted
- [ ] Loading spinner shows while fetching
- [ ] Error message displays on backend error
- [ ] Refresh button works

### 5.2 Tablet (768px - 1023px)

- [ ] Dashboard is responsive
- [ ] Single column layout for news
- [ ] All controls accessible without horizontal scroll
- [ ] Touch interactions work (on actual device if possible)

### 5.3 Mobile (< 768px)

- [ ] Fully responsive single-column layout
- [ ] No horizontal scroll
- [ ] Form is touch-friendly (large buttons, inputs)
- [ ] Text is readable (min 16px font)
- [ ] Images scale properly

### 5.4 API Integration

- [ ] Dashboard shows real data from backend
- [ ] Weather displays temperature, condition, location
- [ ] News shows title, source, date for multiple articles
- [ ] Twitter post displays with engagement metrics
- [ ] Partial data (some sections empty) handled gracefully
- [ ] API error (e.g., slow backend) shows user-friendly message

### 5.5 Error Scenarios

- [ ] Invalid date parameter → form validation error
- [ ] Backend timeout → "Please try again" message
- [ ] Network disconnected → network error message
- [ ] 500 error from backend → "Server error" message
- [ ] Missing API key (if auth needed) → authentication error

---

## 6. Build & Deployment (5 min)

### 6.1 Production Build

```bash
cd frontend/modern-weather

# Create optimized production build
npm run build

# Output: ./dist/ (or specified output directory)
```

### 6.2 Build Verification

```bash
# Check bundle size
npm run build -- --stats

# Serve production build locally
npm run preview
# Open http://localhost:4173
```

### 6.3 Pre-deployment Checklist

- [ ] All tests pass: `npm test`
- [ ] ESLint passes: `npm run lint`
- [ ] Build succeeds: `npm run build`
- [ ] No console errors in browser
- [ ] Performance metrics acceptable (LCP < 2.5s, CLS < 0.1)
- [ ] CORS configured on backend
- [ ] Environment variables set in deployment environment

---

## 7. Troubleshooting

### 7.1 Dependencies Issue

```bash
# Clear npm cache and reinstall
rm -rf node_modules package-lock.json
npm install
```

### 7.2 TypeScript Errors

```bash
# Regenerate type files from OpenAPI (if configured)
npm run generate:types

# Check for typescript errors
npm run type-check
```

### 7.3 Jest Configuration

```bash
# Verify Jest can find/run tests
npm test -- --showConfig

# Debug Jest transformation
npm test -- --verbose Dashboard.test.tsx
```

### 7.4 Backend Connection Issues

```bash
# Check backend is running
curl -v http://localhost:5000/api/aggregate?date=2026-03-23T15:30:00Z&country=US&company=Apple&category=business

# Check CORS headers in response
# Should include: Access-Control-Allow-Origin: [dashboard-url]
```

### 7.5 Browser DevTools

```javascript
// In browser console
localStorage.getItem('dashboard-params')  // Check cached params
sessionStorage.getItem('api-cache')       // Check cache
navigator.serviceWorker.getRegistrations() // Check for SW issues
```

---

## 8. Next Steps

1. **Familiarize**: Read [spec.md](spec.md), [data-model.md](data-model.md), [contracts/api.contract.md](contracts/api.contract.md)
2. **Implement**: Compare [tasks.md](tasks.md) for implementation order
3. **Test**: Write tests alongside implementation (TDD approach recommended)
4. **Review**: Submit PR following [CONTRIBUTING.md](../../docs/CONTRIBUTING.md)
5. **Deploy**: Merge to main → CI runs tests → deploy to staging/prod

---

## Helpful Resources

- [React Query Documentation](https://tanstack.com/query/latest)
- [React Testing Library Docs](https://testing-library.com/docs/react-testing-library/intro/)
- [Mock Service Worker](https://mswjs.io/)
- [Tailwind CSS](https://tailwindcss.com/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Jest Documentation](https://jestjs.io/)

---

## Support

- Backend issues: Check `/Presentation/Controllers/AggregateController.cs`
- Frontend issues: Check `frontend/modern-weather/` source
- Test failures: Review error in Jest output + check MSW network tab in DevTools
- CORS errors: Frontend browser console will show; configure backend CORS policy
