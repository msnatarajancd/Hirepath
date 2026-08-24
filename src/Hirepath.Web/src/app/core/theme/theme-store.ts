import { Injectable, effect, signal } from '@angular/core';

export type Theme = 'dark' | 'light';

const STORAGE_KEY = 'hirepath.theme';

/**
 * Owns the active theme. Writes `data-theme` on <html> and persists the choice.
 * Dark is the default. One place decides how theming works.
 */
@Injectable({ providedIn: 'root' })
export class ThemeStore {
  private readonly _theme = signal<Theme>(this.readInitial());

  /** The active theme, as a readonly signal. */
  readonly theme = this._theme.asReadonly();

  constructor() {
    effect(() => {
      const theme = this._theme();
      document.documentElement.setAttribute('data-theme', theme);
      localStorage.setItem(STORAGE_KEY, theme);
    });
  }

  toggle(): void {
    this._theme.update((t) => (t === 'dark' ? 'light' : 'dark'));
  }

  set(theme: Theme): void {
    this._theme.set(theme);
  }

  private readInitial(): Theme {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored === 'dark' || stored === 'light') {
      return stored;
    }
    return 'dark';
  }
}
