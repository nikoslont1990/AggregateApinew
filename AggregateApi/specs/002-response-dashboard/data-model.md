# Data Model: Response Dashboard UI

**Phase**: 1 (Design & Contracts)  
**Date**: March 23, 2026  
**Reference**: [spec.md](spec.md)

## Overview

The Response Dashboard consumes aggregated data from the backend Aggregate controller and manages user-configurable query parameters. This document defines the data structures, relationships, and validation rules.

## Core Entities

### QueryParameters (Client-side State)

User-configurable filters for data retrieval. Source of truth is URL query parameters or form state.

```typescript
interface QueryParameters {
  date: string;           // ISO 8601 format: YYYY-MM-DDTHH:mm:ssZ
  country: string;        // ISO 3166-1 alpha-2 code or country name (e.g., "US", "Greece")
  company: string;        // Company name for news filtering (e.g., "Apple")
  category: string;       // News category (e.g., "business", "sports", "health")
  sortBy: string;         // Sorting key for news (e.g., "popularity", "relevance", "publishedAt")
  url?: string;           // Twitter/social URL for event tracking (optional)
}

// Validation Rules:
// - date: REQUIRED, must parse as valid ISO 8601 timestamp
// - country: REQUIRED, non-empty string, 1-255 characters
// - company: REQUIRED, non-empty string, 1-255 characters
// - category: REQUIRED, non-empty string, 1-255 characters
// - sortBy: REQUIRED, value from predefined set: ["popularity", "relevance", "publishedAt"]
// - url: OPTIONAL, must be valid URL if provided

// Default Values:
const DEFAULT_PARAMETERS: Partial<QueryParameters> = {
  country: "US",
  company: "Apple",
  category: "business",
  sortBy: "popularity",
};
```

### AggregateResponse (Server Response)

Aggregated data returned by backend `/api/aggregate` endpoint. Maps to `AggregateResponse` .NET model.

```typescript
interface AggregateResponse {
  weatherApiData: WeatherData | null;
  newsApiData: NewsArticle[] | null;
  newsApiCategoryData: NewsArticle[] | null;
  twitterData: TwitterPost | null;
}

// State Transitions:
// 1. LOADING -> SUCCESS (with data) | ERROR
// 2. SUCCESS -> LOADING (on refresh) | ERROR (on next fetch)
// 3. ERROR -> LOADING (on retry)
// 4. PARTIAL (e.g., weather success, news error) allowed per spec
```

### WeatherData

Meteorological information for the selected country.

```typescript
interface WeatherData {
  location: string;               // City/location name
  country: string;                // Country name
  temperature: number;            // Celsius
  temperatureF?: number;          // Fahrenheit (optional)
  condition: string;              // Description (e.g., "Sunny", "Rainy")
  iconUrl?: string;               // Weather icon URL
  humidity: number;               // Percentage (0-100)
  windSpeed: number;              // km/h or m/s (depends on API)
  pressure?: number;              // hPa
  visibility?: number;            // km
  uvIndex?: number;               // 0-11+ scale
  sunrise?: string;               // ISO timestamp
  sunset?: string;                // ISO timestamp
  timestamp: string;              // ISO timestamp of data collection
}

// Validation Rules:
// - temperature: REQUIRED, number in range -50 to 60 (Celsius)
// - humidity: REQUIRED, 0-100
// - windSpeed: REQUIRED, >= 0
// - timestamp: REQUIRED, valid ISO 8601 timestamp
```

### NewsArticle

Individual news article from news APIs.

```typescript
interface NewsArticle {
  id: string;                     // Unique article ID (source + URL hash)
  title: string;                  // Article title (1-500 chars)
  description: string;            // Summary text (0-2000 chars)
  content?: string;               // Full article text (optional, may be truncated)
  source: {
    id?: string;                  // Source API ID
    name: string;                 // Source name (e.g., "BBC", "Reuters")
  };
  author?: string;                // Article author name
  url: string;                    // URL to full article
  urlToImage?: string;            // Article thumbnail/hero image URL
  publishedAt: string;            // ISO timestamp
  category?: string;              // News category tag (e.g., "business", "tech")
  sentiment?: "positive" | "negative" | "neutral"; // Optional sentiment analysis
}

// Validation Rules:
// - id: REQUIRED, unique within response
// - title: REQUIRED, non-empty, max 500 chars
// - url: REQUIRED, valid URL
// - publishedAt: REQUIRED, valid ISO 8601 timestamp
// - description: OPTIONAL, may be empty; null handled gracefully
// - source.name: REQUIRED, non-empty

// Pagination (implicit in arrays):
// - Backend may return up to 100 articles per category
// - Client truncates to 20 displayed in UI (rest available on scroll/pagination)
```

### TwitterData

Social media post/event data.

```typescript
interface TwitterPost {
  id: string;                     // Post ID
  text: string;                   // Post content (0-280 chars for X/Twitter)
  author: {
    id: string;
    name: string;
    handle: string;               // @handle
    avatar_url?: string;
    followers?: number;
  };
  created_at: string;             // ISO timestamp
  engagement: {
    likes: number;                // Like count
    retweets: number;             // Retweet/share count
    replies: number;              // Reply count
    views?: number;               // View count (X API v2+)
  };
  url: string;                    // URLs to post
  hashtags?: string[];            // Array of hashtag strings
  media?: {
    type: "photo" | "video" | "gif";
    url: string;
  }[];
  related_links?: Array<{
    title: string;
    url: string;
  }>;
}

// Validation Rules:
// - id: REQUIRED, unique
// - text: REQUIRED, non-empty, max 280 chars
// - author.handle: REQUIRED, starts with @
// - created_at: REQUIRED, valid ISO 8601 timestamp
// - engagement metrics: 0 as default if missing
```

### UIState

Client-side UI state (not persisted unless explicitly cached).

```typescript
interface UIState {
  isLoading: boolean;             // True during API request
  error: DashboardError | null;   // Error details if fetch failed
  lastUpdated: string | null;     // ISO timestamp of last successful fetch
  selectedParams: QueryParameters; // Currently selected/displayed params
  isRefreshing: boolean;          // True during manual/auto refresh
  autoRefreshInterval?: number;   // Milliseconds; null = disabled
  isAutoRefreshEnabled: boolean;
}

interface DashboardError {
  type: "network" | "validation" | "backend" | "unknown";
  message: string;                // User-friendly error message
  details?: string;               // Technical details for debugging
  code?: string;                  // Backend error code (e.g., "WEATHER_API_503")
  retryable: boolean;             // User can retry?
  timestamp: string;              // ISO timestamp
}

// State Transitions:
// IDLE -> LOADING: user submits parameters
// LOADING -> SUCCESS (no error, data present)
// LOADING -> PARTIAL_ERROR (some data missing)
// LOADING -> ERROR (all data missing)
// SUCCESS/PARTIAL_ERROR/ERROR -> LOADING (on refresh)
// LOADING -> LOADING (ignore duplicate requests)
```

## Data Flow Diagram

```
User Input (URL/Form)
    ↓
QueryParameters Validation
    ↓
HTTP Request with params → Backend /api/aggregate
    ↓
AggregateResponse (with Weather, News, Twitter)
    ↓
Parse & normalize (handle nulls, partial data)
    ↓
UIState (data + loading + error)
    ↓
React Components (render by type: Weather, News, Twitter)
    ↓
User Views Dashboard

On Error:
HTTP Error → DashboardError message → UI error display → User can retry
```

## Relationships

- **QueryParameters → AggregateResponse**: 1:1 (each parameter set produces one response)
- **AggregateResponse → WeatherData**: 1:0..1 (optional; null if API fails)
- **AggregateResponse → NewsArticle[]**: 1:0..* (array; empty if no results)
- **AggregateResponse → TwitterData**: 1:0..1 (optional; null if API fails or no match)
- **UIState → QueryParameters**: Every state change reflects current user params
- **UIState → DashboardError**: Present only when error occurs

## Validation Rules Summary

| Entity | Field | Required | Type | Constraints |
|--------|-------|----------|------|-------------|
| QueryParameters | date | ✅ | string | ISO 8601, valid timestamp |
| | country | ✅ | string | 1-255 chars, non-empty |
| | company | ✅ | string | 1-255 chars, non-empty |
| | category | ✅ | string | 1-255 chars, non-empty |
| | sortBy | ✅ | string | One of: popularity, relevance, publishedAt |
| | url | ❌ | string | Valid URL if provided |
| WeatherData | temperature | ✅ | number | -50 to 60°C |
| | humidity | ✅ | number | 0-100 |
| | windSpeed | ✅ | number | ≥ 0 |
| | timestamp | ✅ | string | ISO 8601 |
| NewsArticle | id | ✅ | string | Unique |
| | title | ✅ | string | 1-500 chars |
| | url | ✅ | string | Valid URL |
| | publishedAt | ✅ | string | ISO 8601 |
| TwitterPost | id | ✅ | string | Unique |
| | text | ✅ | string | 0-280 chars |
| | author.handle | ✅ | string | Starts with @ |
| | created_at | ✅ | string | ISO 8601 |

## Persistence Strategy

- **Query Parameters**: Cached in localStorage for quick recall + URL for bookmarking/sharing
- **API Responses**: React Query cache (5 min stale time, 10 min total time) + memory
- **Error History**: Memory only (not persisted across sessions)
- **User Preferences** (optional enhancement): localStorage for UI theme, results per page

## Constraints & Assumptions

- All timestamps are in UTC (ISO 8601 format)
- Backend ensures response contains only valid JSON; null values indicate "data unavailable"
- News article arrays limited to ~100 items per response (server-side pagination)
- Twitter API may return zero or one result; no pagination for Twitter endpoint
- Weather data updates every 10-30 min server-side; client respects via Cache-Control headers
- Database not used; all data is request/response ephemeral
