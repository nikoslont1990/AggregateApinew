# Quickstart: Run the Modern Weather Landing demo

Recommended: use Expo (quick to bootstrap, supports web static export).

1) Install prerequisites

```bash
# Node and npm installed
npm install -g expo-cli
# or use npx with local project
```

2) Bootstrap project (if starting fresh)

```bash
npx create-expo-app frontend/modern-weather --template expo-template-blank-typescript
cd frontend/modern-weather
```

3) Add fixtures and screens

- Create `app/fixtures/countries.json` with the sample JSON from `research.md`.
- Create `app/screens/Landing.tsx` and `app/screens/About.tsx`. Import fixtures and render cards.

4) Run locally

```bash
cd frontend/modern-weather
npm install
npm run start
# then press 'w' to open web or run on a simulator
```

5) Build static web demo

```bash
expo build:web
# or with expo-cli newer flows:
expo export:web --output dist
```

Notes

- Tests: run `npm test` (Jest) after adding tests.
- For a simple static preview without Expo, you can export a web build and serve the `dist/` folder.
