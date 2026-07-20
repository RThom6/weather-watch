# Weather Watch

A small weather app for Seiko takehome test a .NET API that wraps OpenWeather + RestCountries, and a SvelteKit
frontend for searching cities, saving them to a homepage, and viewing current conditions
plus a 5-day forecast.

- **Backend** — `src/WeatherWatch.Api` (.NET 10 minimal API, SQLite)
- **Frontend** — `frontend` (SvelteKit + Tailwind)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org)
- API keys for [OpenWeather](https://openweathermap.org/api) and
  [RestCountries](https://restcountries.com) (an API-layer key)
- For the Docker route only: Docker with a running engine (on macOS, e.g. `colima start`)

## Configuration

Put your API keys in `src/WeatherWatch.Api/appsettings.Development.json`:

```json
{
  "OpenWeather": { "ApiKey": "your_openweather_key" },
  "RestCountries": { "ApiKey": "your_restcountries_key" }
}
```

The frontend reads the API location from `frontend/.env`:

```
PUBLIC_API_BASE_URL=http://localhost:5170
```

## Running in development

Two processes. **Backend** (from the repo root):

```bash
dotnet run --project src/WeatherWatch.Api
```

The API listens on <http://localhost:5170>. It creates the SQLite database
(`citywatch.db`) on first run.

**Frontend** (in a second terminal):

```bash
cd frontend
npm install
npm run dev
```

Open <http://localhost:5173>.

## Running with Docker

Everything (API, frontend, and a Caddy reverse proxy) runs behind a single origin:

```bash
docker compose up --build
```

Open <http://localhost:8080>.

Notes:

- The backend reads its keys from `appsettings.Development.json` (baked into the image)
- The database lives in the `city-db` volume and starts empty — add cities via the
  **Create** page.
- Caddy routes `/api/*` to the backend and everything else to the frontend, so the
  browser only ever talks to `localhost:8080`.

## Running the end-to-end tests

The Playwright suite lives in `frontend/e2e`. Tests are self-contained. Each one creates and
deletes the data it needs via the API — so they run against any environment.

```bash
cd frontend
npm run test:e2e      # installs browsers on first run, then runs headless
```

### Interactive UI mode

The UI runner lets you watch each test, step through actions, and re-run individually:

```bash
cd frontend
npx playwright test --ui
```

By default the tests start their own dev servers (backend on `:5170`, frontend on `:5173`)
and target <http://localhost:5173>.

### Testing against the Docker stack

Point the tests at the already-running Docker app; Playwright then starts no servers of its
own:

```bash
E2E_BASE_URL=http://localhost:8080 npx playwright test --ui
```

(Drop `--ui` for a headless run.)

## Useful commands

| Command | Where | What |
| --- | --- | --- |
| `dotnet run --project src/WeatherWatch.Api` | repo root | Start the API |
| `npm run dev` | `frontend` | Start the frontend dev server |
| `npm run check` | `frontend` | Type-check the frontend |
| `npm run test:e2e` | `frontend` | Run the Playwright suite headless |
| `npx playwright test --ui` | `frontend` | Run the Playwright suite in UI mode |
| `docker compose up --build` | repo root | Run the whole stack in Docker |


## Acknowledgements

The Weather backgrounds were sourced from ondersumer077's weatherCard repo: <https://github.com/ondersumer07/weatherCard/tree/master>
