import { test, expect } from './fixtures';
import { clickUntil } from './utils';

test('searching lists matching cities', async ({ page, city }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill(city.name);
	await expect(page.locator('ul li').first()).toBeVisible();
});

test('clicking a result opens its detail page', async ({ page, city }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill(String(city.id));
	const link = page.getByRole('link', { name: new RegExp(city.name) });
	await expect(link.first()).toBeVisible();
	await link.first().click();
	await expect(page).toHaveURL(/\/cities\/\d+$/);
	await expect(page.getByRole('heading', { name: city.name, exact: true })).toBeVisible();
});

test('add to home makes the city appear on the homepage', async ({ page, city }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill(String(city.id));
	await clickUntil(
		page.getByRole('button', { name: 'Add to home' }).first(),
		page.getByRole('button', { name: 'Remove from home' }).first()
	);

	await page.goto('/');
	await expect(page.getByRole('heading', { name: city.name, exact: true })).toBeVisible();
});
