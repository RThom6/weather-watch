import { expect, test } from '@playwright/test';
import { clickUntil } from './utils';

test('searching lists matching cities', async ({ page }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill('a');
	await expect(page.locator('ul li').first()).toBeVisible();
});

test('clicking a result opens its detail page', async ({ page }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill('warsaw');
	const link = page.getByRole('link', { name: /Warsaw/ });
	await expect(link).toBeVisible();
	await link.click();
	await expect(page).toHaveURL(/\/cities\/\d+$/);
	await expect(page.getByRole('heading', { name: 'Warsaw', exact: true })).toBeVisible();
});

test('add to home makes the city appear on the homepage', async ({ page }) => {
	await page.goto('/cities/search');
	await page.getByPlaceholder('Search by name').fill('warsaw');
	await clickUntil(
		page.getByRole('button', { name: 'Add to home' }).first(),
		page.getByRole('button', { name: 'Remove from home' }).first()
	);

	await page.goto('/');
	await expect(page.getByRole('heading', { name: 'Warsaw', exact: true })).toBeVisible();
});
