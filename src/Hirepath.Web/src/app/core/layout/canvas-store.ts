import { Injectable, computed, signal } from '@angular/core';

/**
 * Controls the canvas panel beside the conversation. Chat drives on the left;
 * deep work opens here on the right. On mobile the shell renders it as a sheet.
 */
@Injectable({ providedIn: 'root' })
export class CanvasStore {
  private readonly _open = signal(false);
  private readonly _title = signal<string>('');

  readonly isOpen = this._open.asReadonly();
  readonly title = this._title.asReadonly();
  readonly hasContent = computed(() => this._title().length > 0);

  open(title: string): void {
    this._title.set(title);
    this._open.set(true);
  }

  close(): void {
    this._open.set(false);
  }

  toggle(): void {
    this._open.update((v) => !v);
  }
}
