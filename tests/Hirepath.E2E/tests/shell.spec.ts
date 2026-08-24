import { expect, test } from '@playwright/test';

test.describe('App shell', () => {
  test('opens to the conversation with onboarding', async ({ page }) => {
    await page.goto('/');

    await expect(page.getByRole('heading', { name: /find your fit/i })).toBeVisible();
    await expect(
      page.getByRole('button', { name: /analyse my resume against a job description/i }),
    ).toBeVisible();
  });

  test('a suggestion chip fills the composer', async ({ page }) => {
    await page.goto('/');

    await page.getByRole('button', { name: /rewrite a bullet for more impact/i }).click();

    await expect(page.getByRole('textbox', { name: /message hirepath/i })).toHaveValue(
      /rewrite a bullet/i,
    );
  });

  test('the theme toggle switches the document theme', async ({ page }) => {
    await page.goto('/');
    const html = page.locator('html');

    await expect(html).toHaveAttribute('data-theme', 'dark');
    await page.getByRole('button', { name: /switch to light theme/i }).click();
    await expect(html).toHaveAttribute('data-theme', 'light');
  });
});
