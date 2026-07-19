import { expect, test } from '@playwright/test';

test('empty homepage shows guidance and nav links', async ({ page }) => {
	await page.goto('/');
	await expect(page.getByRole('heading', { name: 'My cities' })).toBeVisible();
	await expect(page.getByText('No cities on your homepage yet.')).toBeVisible();
	await expect(page.getByRole('link', { name: 'Search' })).toBeVisible();
	await expect(page.getByRole('link', { name: 'Create' })).toBeVisible();
});

test('navigates from homepage to search', async ({ page }) => {
	await page.goto('/');
	await page.getByRole('link', { name: 'Search' }).click();
	await expect(page.getByRole('heading', { name: 'Search cities' })).toBeVisible();
	await page.getByRole('link', { name: 'Back' }).click();
	await expect(page.getByRole('heading', { name: 'My cities' })).toBeVisible();
});

test('navigates from homepage to create', async ({ page }) => {
	await page.goto('/');
	await page.getByRole('link', { name: 'Create' }).click();
	await expect(page.getByRole('heading', { name: 'Create a city' })).toBeVisible();
});
