import { test as base, expect } from '@playwright/test';

const apiBase =
	process.env.E2E_API_URL ??
	(process.env.E2E_BASE_URL ? `${process.env.E2E_BASE_URL}/api` : 'http://localhost:5170');

type City = { id: number; name: string };

export const test = base.extend<{ city: City }>({
	city: async ({ request }, use) => {
		const found = await request.get(`${apiBase}/cities/find`, { params: { name: 'Warsaw' } });
		const { cities } = await found.json();
		expect(cities.length, 'city lookup returned no results').toBeGreaterThan(0);
		const { name, countryCode } = cities[0];

		const created = await request.post(`${apiBase}/cities/create`, {
			data: { name, countryCode }
		});
		const { cityId, isSuccess } = await created.json();
		expect(isSuccess, 'city creation failed').toBe(true);

		await use({ id: cityId, name });

		await request.delete(`${apiBase}/cities/${cityId}`);
	}
});

export { expect };
