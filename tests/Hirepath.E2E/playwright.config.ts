import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright e2e config. The full Demo-Mode flow (upload → paste JD → analysis →
 * follow-ups → reload) grows across phases; Phase 0 boots only the web dev
 * server and checks the shell renders, since the onboarding screen makes no API
 * call. The API web server is added here in Phase 1 when intake needs it.
 */
const webBaseURL = 'http://localhost:4200';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  reporter: process.env.CI ? 'line' : [['html', { open: 'never' }]],
  use: {
    baseURL: webBaseURL,
    trace: 'on-first-retry',
  },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
  webServer: {
    command: 'npm --prefix ../../src/Hirepath.Web start',
    url: webBaseURL,
    reuseExistingServer: !process.env.CI,
    timeout: 180_000,
  },
});
