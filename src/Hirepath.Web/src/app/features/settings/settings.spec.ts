import { render, screen } from '@testing-library/angular';
import userEvent from '@testing-library/user-event';
import { of } from 'rxjs';
import { Client, SettingsDto, UpdateSettingsRequest } from '../../core/api/hirepath-api.client';
import { Settings } from './settings';

function stubClient(overrides: Partial<Client> = {}): Client {
  const settings: SettingsDto = {
    provider: 'Fake',
    model: '',
    hasApiKey: false,
    baseUrl: undefined,
    theme: 'dark',
    demoMode: true,
    redactPii: false,
  };
  return {
    getSettings: () => of(settings),
    updateSettings: (body: UpdateSettingsRequest) =>
      of({ ...settings, ...body, hasApiKey: !!body.apiKey }),
    ...overrides,
  } as unknown as Client;
}

describe('Settings', () => {
  it('renders provider options with Demo selected', async () => {
    await render(Settings, { providers: [{ provide: Client, useValue: stubClient() }] });

    expect(await screen.findByRole('radio', { name: /demo/i })).toHaveAttribute(
      'aria-checked',
      'true',
    );
    expect(screen.getByRole('radio', { name: /anthropic/i })).toBeInTheDocument();
  });

  it('reveals the API key field when a keyed provider is chosen', async () => {
    const user = userEvent.setup();
    await render(Settings, { providers: [{ provide: Client, useValue: stubClient() }] });

    expect(screen.queryByLabelText(/api key/i)).not.toBeInTheDocument();
    await user.click(await screen.findByRole('radio', { name: /anthropic/i }));

    expect(screen.getByLabelText(/api key/i)).toBeInTheDocument();
  });

  it('saves settings when the form is submitted', async () => {
    const update = vi.fn((body: UpdateSettingsRequest) =>
      of({ ...body, hasApiKey: false } as unknown as SettingsDto),
    );
    const user = userEvent.setup();
    await render(Settings, {
      providers: [{ provide: Client, useValue: stubClient({ updateSettings: update }) }],
    });

    await user.click(await screen.findByRole('button', { name: /save settings/i }));

    expect(update).toHaveBeenCalledOnce();
  });
});
