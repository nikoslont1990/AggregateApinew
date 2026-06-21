# API Contract: Response Dashboard UI

**Phase**: 1 (Design & Contracts)  
**Date**: March 23, 2026  
**Endpoint**: POST `/api/aggregate` (GET with query params)

## Contract Overview

The dashboard consumes the existing backend `/api/aggregate` endpoint. This document specifies the contract frontend expects, including request validation, response format, error handling, and performance SLAs.

## Request Contract

### Endpoint

```
GET /api/aggregate
Host: [backend-url]
```

### Query Parameters

When dashboard makes a request, it sends these URL parameters:

```
GET /api/aggregate?date=2026-03-23T15:30:00Z&country=US&company=Apple&category=business&sortBy=popularity
```

| Parameter | Type | Required | Format | Example | Constraints |
|-----------|------|----------|--------|---------|-------------|
| `date` | string | ✅ Yes | ISO 8601 | `2026-03-23T15:30:00Z` | Valid UTC timestamp |
| `country` | string | ✅ Yes | Country name/code | `US`, `Greece` | Non-empty, 1-255 chars |
| `company` | string | ✅ Yes | Company name | `Apple` | Non-empty, 1-255 chars |
| `category` | string | ✅ Yes | News category | `business`, `sports` | Non-empty, 1-255 chars |
| `sortBy` | string | ✅ Yes | Sort field | `popularity` | One of: popularity, relevance, publishedAt |
| `url` | string | ❌ No | URL | `https://twitter.com/...` | Valid URL if provided |

### Request Headers

```
GET /api/aggregate?... HTTP/1.1
Host: api.example.com
Accept: application/json
Content-Type: application/json
// CORS required (see SLA section)
```

### Frontend Parameter Preparation

Dashboard validates before sending:
- `date`: Parsed as valid ISO 8601; URL-encoded if necessary
- `country`: Non-empty string
- `company`: Non-empty string
- `category`: Non-empty string
- `sortBy`: Validated against enum
- `url`: Valid URL or omitted

### Validation Errors (Client-side)

If validation fails, dashboard does NOT send request. User sees error in parameter form.

---

## Response Contract

### Success Response (HTTP 200)

```json
{
  "weatherApiData": {
    "location": "Seattle",
    "country": "United States",
    "temperature": 15,
    "temperatureF": 59,
    "condition": "Cloudy",
    "iconUrl": "https://api.weather.com/icons/cloudy.png",
    "humidity": 75,
    "windSpeed": 12.5,
    "pressure": 1013,
    "visibility": 10,
    "uvIndex": 3,
    "sunrise": "2026-03-23T06:45:00Z",
    "sunset": "2026-03-23T19:15:00Z",
    "timestamp": "2026-03-23T15:30:00Z"
  },
  "newsApiData": [
    {
      "id": "article-001",
      "title": "Apple announces new product line",
      "description": "Apple unveils next-generation consumer products...",
      "content": "Full article content here...",
      "source": {
        "id": "bbc-news",
        "name": "BBC News"
      },
      "author": "John Doe",
      "url": "https://bbc.com/news/article-001",
      "urlToImage": "https://bbc.com/images/article-001.jpg",
      "publishedAt": "2026-03-23T14:00:00Z",
      "category": "business",
      "sentiment": "positive"
    }
  ],
  "newsApiCategoryData": [
    {
      "id": "article-002",
      "title": "Greek tourism rebounds...",
      "description": "Tourism industry recovery",
      "source": { "name": "Reuters" },
      "url": "https://reuters.com/article-002",
      "publishedAt": "2026-03-23T13:00:00Z"
    }
  ],
  "twitterData": {
    "id": "tweet-001",
    "text": "@Interior shares insights on environmental policy",
    "author": {
      "id": "user-123",
      "name": "Department of Interior",
      "handle": "@Interior",
      "avatar_url": "https://twitter.com/interior/avatar.jpg",
      "followers": 1500000
    },
    "created_at": "2026-03-23T15:00:00Z",
    "engagement": {
      "likes": 25000,
      "retweets": 8000,
      "replies": 1200,
      "views": 500000
    },
    "url": "https://twitter.com/Interior/status/507185938620219395",
    "hashtags": ["environmental", "policy"],
    "media": [
      {
        "type": "photo",
        "url": "https://twitter.com/Interior/media/001.jpg"
      }
    ]
  }
}
```

### Response Structure Rules

```typescript
interface AggregateResponse {
  weatherApiData: WeatherObject | null;    // null if weather API fails
  newsApiData: NewsArticle[] | null;       // null if news API fails, empty [] if no results
  newsApiCategoryData: NewsArticle[] | null; // null if category API fails
  twitterData: TwitterPost | null;         // null if Twitter API fails or no match
}
```

**Rules**:
- All fields are either `object/array` or `null` (never `undefined`)
- Null indicates "data unavailable" (API error, no results, etc.)
- Empty arrays `[]` indicate "API succeeded but no results"
- Each data section is **independent**: one failure doesn't affect others
- All timestamps in response are UTC ISO 8601 format
- Response encoding: UTF-8 JSON

### Response Headers

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: [bytes]
Cache-Control: max-age=300, must-revalidate    // 5 min cache
Expires: [date in 5 min]
ETag: [hash]
Vary: Accept-Encoding
```

---

## Error Response Contract

### Error Response (HTTP 4xx / 5xx)

Backend returns standard error format:

```json
{
  "type": "about:blank",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request parameters are invalid. The 'date' parameter must be a valid ISO 8601 timestamp.",
  "traceId": "0HN08V6A1N3QV:00000001"
}
```

### Frontend Interpretation

| HTTP Status | Meaning | Dashboard Action |
|-------------|---------|------------------|
| 400 | Bad request (invalid params) | Show validation error; user can fix and retry |
| 401/403 | Unauthorized/Forbidden | Show authentication error (check API key/CORS) |
| 404 | Not found | Show "endpoint not available" error |
| 500-599 | Server error | Show "backend error, please try again" + retry button |
| Timeout (>5s) | Network timeout | Show timeout error + auto-retry after 3s |

### Error Handling by Dashboard

```typescript
try {
  const response = await fetchAggregateData(params);
  // Update UIState with response data
  updateUIState({ data: response, error: null, isLoading: false });
} catch (error) {
  // Parse error
  const dashboardError: DashboardError = {
    type: error.status >= 500 ? "backend" : (error.status === 0 ? "network" : "validation"),
    message: error.message || "Failed to fetch data",
    details: error.error?.detail,
    code: error.error?.title,
    retryable: error.status >= 500 || error.status === 0,
    timestamp: new Date().toISOString()
  };
  
  updateUIState({ error: dashboardError, isLoading: false });
  
  // Auto-retry on network errors (exponential backoff)
  if (dashboardError.retryable) {
    scheduleRetry(retryCount++);
  }
}
```

---

## Performance SLA

| Metric | Target | Acceptable Range |
|--------|--------|------------------|
| **Response Time** | < 1s | < 3s (p95) |
| **Availability** | 99% | Request succeeds 95%+ of time |
| **Timeout** | 5 seconds | Dashboard times out after 5s |
| **Payload Size** | < 500 KB | Uncompressed JSON |
| **Concurrent Requests** | Support 100+ | From single user (dashboard + refresh) |

---

## CORS Requirements

Dashboard runs on different origin than backend. Backend MUST configure CORS:

```
Access-Control-Allow-Origin: [dashboard-origin]
Access-Control-Allow-Methods: GET, OPTIONS
Access-Control-Allow-Headers: Content-Type, Accept
Access-Control-Max-Age: 86400
```

**Frontend Assumption**: CORS is already configured (outside scope of dashboard feature).

---

## Caching Strategy

### Browser Cache

- **Weather**: Max 5 minutes (weather changes infrequently)
- **News**: Max 5 minutes (news updates regularly)
- **Twitter**: Max 3 minutes (tweets/engagement change frequently)

Dashboard respects `Cache-Control` headers from backend.

### React Query Cache

- **Stale Time**: 5 minutes (data considered fresh for 5 min)
- **Cache Time**: 10 minutes (keep in memory for 10 min in case user returns to same params)
- **Refetch Interval**: None (unless user enables auto-refresh)

### Conditional Requests (Optional Enhancement)

Backend should return `ETag` headers for cache validation:
```
GET /api/aggregate?... HTTP/1.1
If-None-Match: "abc123def"
```

If data unchanged, return 304 Not Modified.

---

## Data Transformation & Mapping

### From Backend to Frontend

Frontend maps backend `AggregateResponse` to component-ready data:

```typescript
// Backend WeatherApiData → WeatherData (internal type)
const adaptWeatherData = (raw: any): WeatherData => ({
  location: raw.location,
  country: raw.country,
  temperature: raw.temperature,
  // ... transform other fields
});

// Backend NewsArticle[] → NewsArticle[] (already typed)
// No transformation needed; direct pass-through

// Backend TwitterData → TwitterPost (type alignment)
const adaptTwitterData = (raw: any): TwitterPost => ({
  id: raw.id,
  text: raw.text || raw.content, // Backend may vary field names
  author: { ...raw.user, handle: raw.user.screen_name }, // Rename field
  // ...
});
```

---

## Backward Compatibility

- Backend may add optional fields to response; dashboard ignores unknowns
- Backend should NOT remove required fields; this is breaking change (requires version bump)
- Dashboard uses optional chaining (`?.`) to safely access potentially missing fields

---

## Testing Scenarios

### Test Case 1: Happy Path
- Request: Valid parameters → 200 OK with full data → Dashboard renders all sections ✅

### Test Case 2: Partial Data
- Request: Valid parameters → 200 OK, weather missing → Dashboard shows news/Twitter, skips weather section ✅

### Test Case 3: Network Error
- Request: Valid params → Network timeout → Dashboard shows error + retry button ✅

### Test Case 4: Backend Error
- Request: Valid params → 500 error → Dashboard shows "server error, try again" ✅

### Test Case 5: Invalid Parameters
- Request: Missing required param → 400 error → Dashboard shows validation error ✅

---

## Contract Version

- **Version**: 1.0
- **Effective Date**: March 23, 2026
- **Backend Compatibility**: AggregateApi/.NET 8 with OpenAPI v3.0.1
- **Breaking Change Process**: Any breaking change requires MAJOR version bump + migration guide
