// Captures each screen at three widths in both themes into docs/screenshots/.
// Run with the web dev server already up (npm --prefix src/Hirepath.Web start),
// or via `npm run shots` which starts one. Reviewed by a human before a phase
// is called done (verification gate 7).
import { chromium } from '@playwright/test';
import { mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';

const here = dirname(fileURLToPath(import.meta.url));
const outDir = join(here, '../../docs/screenshots');
const baseURL = process.env.BASE_URL ?? 'http://localhost:4200';

const widths = [
  { name: '375', width: 375, height: 812 },
  { name: '768', width: 768, height: 1024 },
  { name: '1440', width: 1440, height: 900 },
];
const routes = [
  { name: 'chat', path: '/chat' },
  { name: 'resumes', path: '/resumes' },
  { name: 'jobs', path: '/jobs' },
  { name: 'settings', path: '/settings' },
];
const themes = ['dark', 'light'];

await mkdir(outDir, { recursive: true });
const browser = await chromium.launch();

for (const theme of themes) {
  for (const w of widths) {
    const context = await browser.newContext({
      viewport: { width: w.width, height: w.height },
    });
    const page = await context.newPage();
    await page.addInitScript((t) => localStorage.setItem('hirepath.theme', t), theme);
    for (const route of routes) {
      await page.goto(`${baseURL}${route.path}`);
      await page.waitForTimeout(400);
      const file = join(outDir, `${route.name}-${theme}-${w.name}.png`);
      await page.screenshot({ path: file, fullPage: false });
      console.log(`captured ${file}`);
    }
    await context.close();
  }
}

await browser.close();
