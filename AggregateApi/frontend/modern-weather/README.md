# Modern Weather Frontend

A responsive, accessible React 18 dashboard that consumes aggregated weather, news, and Twitter data from a .NET 8 backend API. Built with TypeScript, Vite, Tailwind CSS, and React Router v6.

## Features

✅ **Core Features (MVP - Phase 1-5)**
- View aggregated weather, news, and Twitter data from backend API
- Save, edit, delete query parameter sets (CRUD operations)
- Navigate between Dashboard and Saved Queries pages
- URL-synced parameters for bookmarking and sharing
- Browser back-button navigation support
- Offline-first design with intelligent caching (24-hour TTL, 30-day hard delete)
- Data export to JSON format

✨ **Extended Features (Phase 6-8)**
- Export aggregated data to JSON format
- Responsive design optimized for mobile (320px), tablet (768px), desktop (1024px+)
- WCAG 2.1 Level AA accessibility compliance
- Keyboard navigation and screen reader support
- 80%+ code test coverage
- Comprehensive documentation

## Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Frontend Framework** | React | 18.x |
| **Language** | TypeScript | 5.x |
| **Build Tool** | Vite | 8.x |
| **Styling** | Tailwind CSS | 4.x |
| **Routing** | React Router | v6 |
| **State Management** | React Context API | - |
| **HTTP Client** | Axios | 1.x |
| **Testing** | Jest + React Testing Library | Latest |
| **API Mocking** | MSW (Mock Service Worker) | 2.x |
| **Accessibility** | axe-core | 4.x |

## Quick Start

### Prerequisites

- **Node.js**: 18.0.0 or higher
- **.NET 8 Backend**: Running on `http://localhost:5000` (configurable via `.env`)
- **npm**: 9.0.0 or higher (included with Node)

### Installation

```bash
# Install dependencies
npm install

# Configure environment
cp .env.development .env  # for local development
```

### Development

```bash
# Start development server (http://localhost:3000)
npm run dev

# In another terminal, start the backend if not running
# cd ../.. (to AggregateApi root)
# dotnet run

# Development server auto-reloads on file changes
# Press 'q' to stop the dev server
```

### Building for Production

```bash
# Create optimized production build
npm run build

# Preview production build locally
npm run preview

# Output goes to ./dist/ directory
```

## Testing

```bash
# Run all tests
npm run test

# Run tests in watch mode (re-runs on file changes)
npm run test:watch

# Run tests with coverage report
npm run test:coverage

# Expects 80%+ coverage across components and utilities
```

## Code Quality

```bash
# Type checking
npm run type-check

# Linting
npm run lint

# Format code with Prettier
npm run format
```

## Project Structure

```text
src/
├── components/          # React components (Dashboard, Forms, Display, etc.)
├── hooks/              # Custom React hooks (useAggregateData, useSavedQueries, etc.)
├── services/
│   ├── api/           # Axios API client and types
│   ├── storage/       # localStorage and IndexedDB utilities
│   └── validation/    # Input validation functions
├── types/             # TypeScript type definitions
├── utils/             # Utility functions (date, cache, export, etc.)
├── context/           # React Context providers
├── pages/             # Page components (Dashboard, SavedQueries)
├── App.tsx            # Root component with routing setup
├── main.tsx           # Entry point
└── index.css          # Tailwind CSS directives and custom styles

tests/
├── unit/              # Unit tests for hooks, utils, services
├── components/        # Component tests
├── integration/       # End-to-end feature tests
├── accessibility/     # WCAG 2.1 accessibility tests
└── mocks/            # MSW mock handlers for API

```

## Environment Variables

### Development (.env.development)
```
VITE_API_URL=http://localhost:5000
VITE_APP_NAME="Modern Weather Frontend"
```

### Production (.env.production)
```
VITE_API_URL=https://api.example.com
VITE_APP_NAME="Modern Weather Frontend"
```

## API Integration

The frontend communicates with the `.NET 8 AggregateController` endpoint:

**Endpoint**: `GET /api/aggregate`

**Query Parameters**:
- `date` (required): ISO 8601 date (YYYY-MM-DDTHH:mm:ssZ)
- `sortBy` (optional): "relevance" or "popularity" (default: "relevance")
- `company` (optional): Company filter (default: "Apple")
- `country` (optional): Country filter (default: "Greece")
- `category` (optional): News category (default: "business")
- `url` (optional): Twitter/social media URL to monitor

**Response**: 
```json
{
  "weatherApiData": { /* weather info */ },
  "newsApiData": [ /* article objects */ ],
  "newsApiCategoryData": [ /* category news */ ],
  "twitterData": { /* tweet objects */ }
}
```

**Error Handling**:
- 400: Bad request (invalid parameters)
- 500: Server error
- Network timeout: Automatic retry with user notification

## Offline Support

The app implements progressive enhancement for offline scenarios:

1. **API Response Caching**: Weather, news, Twitter data cached to localStorage
2. **Cache Expiration**: 24-hour freshness window, 30-day hard delete, manual clear button
3. **Offline Indicator**: Visual status bar shows cache age and offline state
4. **Saved Queries**: Always available offline (stored in localStorage)
5. **Fallback UI**: Shows cached results with "offline" badge when API unavailable

## Accessibility

The application meets **WCAG 2.1 Level AA** accessibility standards:

✅ **Keyboard Navigation**
- Full keyboard support for all interactive elements
- Visible focus indicators (2px outline with proper contrast)
- Logical tab order throughout the app
- Skip-to-main-content link

✅ **Screen Reader Support**
- Semantic HTML (`<button>`, `<form>`, `<label>`, `<main>`, etc.)
- ARIA attributes for dynamic content and form fields
- Descriptive link and button labels
- Form error announcements

✅ **Visual Design**
- Minimum 4.5:1 color contrast ratio (text on background)
- No color-only information (icons + text labels)
- Readable font sizes (16px+ for body text)
- Mobile-friendly touch targets (44px+ minimum)

✅ **Testing**
- Automated testing with axe-core and jest-axe
- Manual testing with screen readers (NVDA simulation)
- Keyboard-only navigation testing
- Lighthouse accessibility audit (target: 95+)

## Browser Support

| Browser | Version | Status |
|---------|---------|--------|
| Chrome/Edge | 90+ | ✅ Full support |
| Firefox | 88+ | ✅ Full support |
| Safari | 14+ | ✅ Full support |
| IE 11 | - | ❌ Not supported |

**Build includes**:
- ES2020+ features (no polyfills)
- Modern CSS (Grid, Flexbox, CSS Variables)
- No transpilation to ES5

## Performance Targets

| Metric | Target | Current |
|--------|--------|---------|
| **FCP** (First Contentful Paint) | <2s | TBD |
| **LCP** (Largest Contentful Paint) | <2.5s | TBD |
| **Data Fetch** (load to results) | <5s | TBD |
| **Page Navigation** | <500ms | TBD |
| **localStorage Operations** | <100ms | TBD |
| **Frame Rate** (interactions) | 60fps | TBD |

Run `npm run build` and then `npm run preview`, then open DevTools Performance tab to profile.

## Development Workflows

### Adding a New Component

1. Create component file in `src/components/YourComponent.tsx`
2. Write component tests in `tests/components/YourComponent.test.tsx`
3. Add accessibility attributes (`aria-label`, `role`, semantic HTML)
4. Add to `src/components/index.ts` for exports
5. Run `npm run test:coverage` to verify test coverage

### Adding a New Type

1. Define interface in `src/types/yourType.ts`
2. Export from `src/types/index.ts`
3. Use in components: `import type { YourType } from '@/types'`

### Adding an API Service

1. Create `src/services/api/yourService.ts` with Axios client
2. Export typed methods: `async function getYourData(params): Promise<YourType>`
3. Write tests in `tests/unit/services/`
4. Document in `src/services/README.md`

### Modifying Styling

1. Update Tailwind classes in component JSX or add custom styles in `src/index.css`
2. Verify WCAG contrast in `tailwind.config.js` color palette
3. Test responsive breakpoints (320px, 768px, 1024px)
4. Run Lighthouse accessibility audit

## Contributing

### Code Style
- **Language**: TypeScript (strict mode enabled)
- **Formatting**: Prettier (run `npm run format`)
- **Linting**: ESLint (run `npm run lint`)
- **Naming**: camelCase for variables, PascalCase for components/types

### Testing Requirements
- All features require component tests
- Utilities require unit tests
- Integration tests for multi-component workflows
- Accessibility tests for keyboard nav and screen readers
- Target: 80%+ coverage

### Pull Request Checklist
- [ ] Tests pass (`npm run test`)
- [ ] Coverage meets 80%+ threshold (`npm run test:coverage`)
- [ ] No TypeScript errors (`npm run type-check`)
- [ ] Code formatted (`npm run format`)
- [ ] Linting passes (`npm run lint`)
- [ ] Accessibility axe report clean (`npm run test`)
- [ ] Works in Chrome 90+, Firefox 88+, Safari 14+
- [ ] Documentation updated

## Troubleshooting

### Port 3000 already in use
```bash
# Kill process on port 3000, or use different port
npm run dev -- --port 3001
```

### CORS errors from backend
```
Error: Access to XMLHttpRequest at 'http://localhost:5000/api/aggregate'
```
**Solution**: Ensure .NET backend has CORS enabled for `http://localhost:3000`

### Tests fail with localStorage errors
```bash
# localStorage mocks are set up in tests/setup.ts
# If still failing, ensure jest.config.js has testEnvironment: 'jsdom'
```

### Tailwind classes not applying
```bash
# Compiled CSS in src/index.css
# Ensure index.css is imported in src/main.tsx
# Rebuild: npm run build
```

## Release Process

1. Write/update tests for new features
2. Ensure all tests pass: `npm run test`
3. Check coverage meets threshold: `npm test -- --coverage`
4. Build production: `npm run build`
5. Test production build locally: `npm run preview`
6. Deploy to hosting (e.g., Vercel, AWS S3, Azure Static Web Apps)

## Documentation

- **Component APIs**: See `src/README.md`
- **Hook Usage**: See `src/hooks/README.md`
- **API Services**: See `src/services/README.md`
- **Testing Guide**: See `TESTING.md`
- **Accessibility Guide**: See `ACCESSIBILITY.md`
- **Deployment**: See `DEPLOYMENT.md`

## License

See LICENSE in root of AggregateApi repository.

## Support

For issues, feature requests, or questions:
1. Check existing GitHub issues
2. File a new issue with reproduction steps
3. Contact the development team

---

**Version**: 1.0.0 MVP  
**Last Updated**: March 26, 2026  
**Status**: 🚀 Ready for Phase 2 (Foundations)
