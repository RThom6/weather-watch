import { defineConfig } from '@playwright/test';

const API_URL = 'http://localhost:5170';
const APP_PORT = 5173;

// Set E2E_BASE_URL (e.g. http://localhost:8080 for the docker stack) to run
// against an already-running app; Playwright then starts no servers of its own.
const externalUrl = process.env.E2E_BASE_URL;

export default defineConfig({
	testMatch: '**/*.e2e.{ts,js}',
	use: {
		baseURL: externalUrl ?? `http://localhost:${APP_PORT}`,
		locale: 'en-US'
	},
	webServer: externalUrl
		? undefined
		: [
				{
					command: 'dotnet run --launch-profile http',
					cwd: '../src/WeatherWatch.Api',
					url: `${API_URL}/cities/search?name=ping`,
					reuseExistingServer: true,
					timeout: 120_000
				},
				{
					command: `npm run dev -- --port ${APP_PORT}`,
					url: `http://localhost:${APP_PORT}`,
					reuseExistingServer: true,
					timeout: 120_000
				}
			]
});
