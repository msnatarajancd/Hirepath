import { Injectable, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Client, SettingsDto, UpdateSettingsRequest } from '../../core/api/hirepath-api.client';
import { ThemeStore } from '../../core/theme/theme-store';

type Status = 'idle' | 'loading' | 'saving' | 'error';

/** Loads and saves app settings, and keeps the theme in sync with the server. */
@Injectable({ providedIn: 'root' })
export class SettingsStore {
  private readonly client = inject(Client);
  private readonly theme = inject(ThemeStore);

  private readonly _settings = signal<SettingsDto | null>(null);
  private readonly _status = signal<Status>('idle');
  private readonly _error = signal<string | null>(null);

  readonly settings = this._settings.asReadonly();
  readonly status = this._status.asReadonly();
  readonly error = this._error.asReadonly();
  readonly loading = computed(() => this._status() === 'loading');
  readonly saving = computed(() => this._status() === 'saving');

  async load(): Promise<void> {
    this._status.set('loading');
    this._error.set(null);
    try {
      const settings = await firstValueFrom(this.client.getSettings());
      this._settings.set(settings);
      this.theme.set(settings.theme === 'light' ? 'light' : 'dark');
      this._status.set('idle');
    } catch {
      this._status.set('error');
      this._error.set('Could not load settings. Is the API running?');
    }
  }

  async save(request: UpdateSettingsRequest): Promise<boolean> {
    this._status.set('saving');
    this._error.set(null);
    try {
      const settings = await firstValueFrom(this.client.updateSettings(request));
      this._settings.set(settings);
      this.theme.set(settings.theme === 'light' ? 'light' : 'dark');
      this._status.set('idle');
      return true;
    } catch {
      this._status.set('error');
      this._error.set('Could not save settings.');
      return false;
    }
  }
}
