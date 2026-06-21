import { http, HttpResponse, setupServer } from 'msw';

// Sample API response data
const sampleAggregateResponse = {
  weatherApiData: {
    location: 'Athens, Greece',
    temperature: 22,
    condition: 'Partly Cloudy',
    humidity: 65,
    windSpeed: 12,
    lastUpdated: new Date().toISOString(),
  },
  newsApiData: [
    {
      title: 'Technology Advances in 2024',
      description: 'Latest developments in AI and cloud computing',
      source: 'Tech Daily',
      publishedAt: new Date().toISOString(),
      url: 'https://example.com/news/1',
    },
    {
      title: 'Global Markets Update',
      description: 'Stock market report for Apple Inc.',
      source: 'Market News',
      publishedAt: new Date().toISOString(),
      url: 'https://example.com/news/2',
    },
  ],
  newsApiCategoryData: [],
  twitterData: {
    tweets: [
      {
        id: '1',
        text: 'Exciting announcement coming soon!',
        author: '@InteriorDept',
        likes: 1234,
        retweets: 567,
        createdAt: new Date().toISOString(),
      },
    ],
  },
};

// MSW request handlers
export const handlers = [
  // GET /api/aggregate
  http.get('*/api/aggregate', () => {
    return HttpResponse.json(sampleAggregateResponse, { status: 200 });
  }),

  // Error response example
  http.get('*/api/aggregate/error', () => {
    return HttpResponse.json(
      { error: 'Internal Server Error', message: 'An unexpected error occurred' },
      { status: 500 }
    );
  }),

  // Timeout simulation
  http.get('*/api/aggregate/slow', async () => {
    await new Promise(resolve => setTimeout(resolve, 5000));
    return HttpResponse.json(sampleAggregateResponse, { status: 200 });
  }),
];

// Setup the server with handlers
export const server = setupServer(...handlers);
