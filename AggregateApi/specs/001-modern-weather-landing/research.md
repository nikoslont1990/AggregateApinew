# Research: Modern Weather Landing (Phase 0)

Decision summary

- Chosen stack: React Native with Expo (TypeScript recommended).
  - Rationale: Fast bootstrapping, works on iOS/Android and web (`expo start --web`) for demo/static export.
  - Alternative considered: Plain React Native CLI — more setup; React (web) — not mobile-first.

- Data approach: Embedded JSON fixtures for countries, country details, and temperature records.
  - Rationale: No network dependence, deterministic data for demos, easy to mock in tests.

- Navigation: React Navigation for stack-based flows (Landing → About).

- Testing: Jest + React Native Testing Library for components; optional Storybook for visual QA.

Mock data format

Example JSON shape (countries.json):

```json
{
  "countries": [
    {
      "id": "usa",
      "name": "United States",
      "isoCode": "US",
      "flag": "assets/flags/us.svg",
      "region": "Americas",
      "temperatureC": 22,
      "details": {
        "description": "Mocked country description.",
        "populationApprox": "331M",
        "sampleForecast": [ { "time": "09:00", "tempC": 20 }, { "time": "12:00", "tempC": 22 }, { "time": "15:00", "tempC": 21 } ]
      }
    }
  ]
}
```

Notes

- Use local SVG/PNG assets for flags to avoid runtime downloads.
- Keep mock fixture minimal and easy to extend.
