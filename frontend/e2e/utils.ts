import { expect, type Locator } from '@playwright/test';

export async function clickUntil(action: Locator, expected: Locator) {
	await expect(async () => {
		if (!(await expected.isVisible())) {
			await action.click();
		}
		await expect(expected).toBeVisible({ timeout: 1000 });
	}).toPass();
}
