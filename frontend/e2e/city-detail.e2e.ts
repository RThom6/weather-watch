import { expect, test } from '@playwright/test';
import { clickUntil } from './utils';

test('detail page shows weather, facts and forecast', async ({ page }) => {
	await page.goto('/cities/1');
	await expect(page.getByRole('heading', { name: 'Warsaw', exact: true })).toBeVisible();
	await expect(page.getByText('Feels like')).toBeVisible();
	await expect(page.getByRole('heading', { name: 'About Warsaw' })).toBeVisible();
	await expect(page.getByRole('heading', { name: '5-day forecast' })).toBeVisible();
});

test('add to home toggles on the detail page', async ({ page }) => {
	await page.goto('/cities/1');
	await clickUntil(
		page.getByRole('button', { name: 'Add to home' }),
		page.getByRole('button', { name: 'Remove from home' })
	);
});

test('editing population persists and displays', async ({ page }) => {
	await page.goto('/cities/1');

	const saveButton = page.getByRole('button', { name: 'Save' });
	await clickUntil(page.getByRole('button', { name: 'Edit' }), saveButton);

	await page.locator('form input[type="number"]').first().fill('1790658');
	await saveButton.click();
	await expect(page.getByText('1,790,658')).toBeVisible();
});
