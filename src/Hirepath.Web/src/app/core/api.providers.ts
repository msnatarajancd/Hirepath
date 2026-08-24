import { Provider } from '@angular/core';
import { API_BASE_URL, Client } from './api/hirepath-api.client';

/**
 * Wires the NSwag-generated API client. In dev the Angular proxy forwards
 * `/api` to the backend, so the base URL is relative (empty). Override
 * API_BASE_URL for a packaged build that talks to a fixed origin.
 *
 * The client itself lives under core/api/ and is git-ignored — regenerate it
 * with `npm run generate:api`. This wiring is hand-written and committed.
 */
export function provideApiClient(baseUrl = ''): Provider[] {
  return [Client, { provide: API_BASE_URL, useValue: baseUrl }];
}
