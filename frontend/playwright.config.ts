import { defineConfig } from '@playwright/test';

const API_URL = 'http://localhost:5170';
const APP_PORT = 5173;

export default defineConfig({
	testMatch: '**/*.e2e.{ts,js}',
	use: {
		baseURL: `http://localhost:${APP_PORT}`,
		locale: 'en-US'
	},
	webServer: [
		{
			command:
				'dotnet run --project ../src/WeatherWatch.Api/WeatherWatch.Api.csproj --launch-profile http',
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
