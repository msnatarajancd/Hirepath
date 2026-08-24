import { TestBed } from '@angular/core/testing';
import { ThemeStore } from './theme-store';

describe('ThemeStore', () => {
  beforeEach(() => {
    localStorage.clear();
    document.documentElement.removeAttribute('data-theme');
    TestBed.resetTestingModule();
  });

  it('defaults to dark and reflects it on the document', () => {
    const store = TestBed.inject(ThemeStore);
    TestBed.tick();

    expect(store.theme()).toBe('dark');
    expect(document.documentElement.getAttribute('data-theme')).toBe('dark');
  });

  it('toggles between dark and light and persists the choice', () => {
    const store = TestBed.inject(ThemeStore);

    store.toggle();
    TestBed.tick();

    expect(store.theme()).toBe('light');
    expect(localStorage.getItem('hirepath.theme')).toBe('light');
  });

  it('restores a persisted theme on creation', () => {
    localStorage.setItem('hirepath.theme', 'light');

    const store = TestBed.inject(ThemeStore);

    expect(store.theme()).toBe('light');
  });
});
