# Data Model: Modern Weather Landing

JSON schema / TypeScript interfaces (minimal)

Country
```ts
interface Country {
  id: string; // e.g. "usa"
  name: string;
  isoCode: string;
  flag: string; // local asset path
  region?: string;
}
```

TemperatureRecord
```ts
interface TemperatureRecord {
  countryId: string;
  valueCelsius: number;
  observedAt?: string; // ISO timestamp optional for mock
}
```

CountryDetails
```ts
interface CountryDetails {
  countryId: string;
  description: string;
  populationApprox?: string;
  sampleForecast: { time: string; tempC: number }[];
}
```

MockDataSource
```ts
interface MockDataSource {
  countries: Array<Country & { temperatureC: number; details: CountryDetails }>
}
```

Storage: keep `countries.json` in `frontend/app/fixtures/` and import it as a module during development.
