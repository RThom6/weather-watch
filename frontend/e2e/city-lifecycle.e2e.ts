import { expect, test } from '@playwright/test';

test('create a city, view it, then delete it', async ({ page }) => {
	page.on('dialog', (dialog) => dialog.accept());

	await page.goto('/cities/create');
	const input = page.getByPlaceholder('Search for a city');
	const searchButton = page.getByRole('button', { name: 'Search', exact: true });
	const createButton = page.getByRole('button', { name: 'Create' }).first();

	await expect(async () => {
		if (await createButton.isVisible()) return;
		if (await searchButton.isVisible()) {
			await input.fill('London');
			await searchButton.click();
		}
		await expect(createButton).toBeVisible({ timeout: 5000 });
	}).toPass({ timeout: 30_000 });

	await createButton.click();

	await expect(page).toHaveURL('/');
	const card = page.getByRole('heading', { name: 'London', exact: true });
	await expect(card).toBeVisible();

	await card.click();
	await expect(page).toHaveURL(/\/cities\/\d+$/);
	await expect(page.getByRole('heading', { name: 'London', exact: true })).toBeVisible();

	await page.getByRole('button', { name: 'Delete' }).click();
	await expect(page).toHaveURL('/');
	await expect(page.getByRole('heading', { name: 'London', exact: true })).toHaveCount(0);
});
