import { expect, test } from '@playwright/test';

// These exercise the shell's escape-hatch navigation and the Phase 1 screens
// rendering. They run against the web dev server; screens that need the API
// degrade to their form/empty state, which is what we assert here.
test.describe('Navigation', () => {
  test('reaches the settings screen and shows provider options', async ({ page }) => {
    await page.goto('/');
    await page.getByRole('link', { name: 'Settings' }).click();

    await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible();
    await expect(page.getByRole('radio', { name: /demo/i })).toBeVisible();
  });

  test('reaches the resumes screen and shows the dropzone', async ({ page }) => {
    await page.goto('/');
    await page.getByRole('link', { name: 'Resumes' }).click();

    await expect(page.getByRole('heading', { name: 'Resumes' })).toBeVisible();
    await expect(page.getByText(/drop a resume here/i)).toBeVisible();
  });

  test('reaches the job descriptions screen', async ({ page }) => {
    await page.goto('/');
    await page.getByRole('link', { name: 'Jobs' }).click();

    await expect(page.getByRole('heading', { name: 'Job descriptions' })).toBeVisible();
  });
});
