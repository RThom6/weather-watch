import { defineConfig } from '@playwright/test';

export default defineConfig({
	testMatch: '**/*.e2e.{ts,js}',
	use: {
		baseURL: 'http://localhost:8080',
		locale: 'en-US'
	}
});
