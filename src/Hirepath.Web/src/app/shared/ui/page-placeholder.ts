import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Icon } from './icon';

/**
 * Honest empty state for routes whose feature is not built yet. Route data
 * supplies the title/subtitle/icon via component input binding. Never pretends
 * a feature exists — it says plainly what's coming.
 */
@Component({
  selector: 'app-page-placeholder',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [Icon],
  template: `
    <div class="ph">
      <div class="ph__icon"><app-icon [name]="icon()" [size]="28" /></div>
      <h1 class="ph__title">{{ title() }}</h1>
      <p class="ph__subtitle">{{ subtitle() }}</p>
      <span class="ph__tag">Coming in a later phase</span>
    </div>
  `,
  styles: [
    `
      :host {
        display: grid;
        place-items: center;
        height: 100%;
        padding: 24px;
      }
      .ph {
        text-align: center;
        max-width: 420px;
      }
      .ph__icon {
        display: inline-grid;
        place-items: center;
        width: 60px;
        height: 60px;
        margin-bottom: 18px;
        color: var(--accent);
        background: var(--accent-soft);
        border-radius: var(--radius-lg);
      }
      .ph__title {
        font-size: 22px;
        font-weight: 650;
        margin: 0 0 8px;
      }
      .ph__subtitle {
        color: var(--text-muted);
        margin: 0 0 18px;
        line-height: 1.55;
      }
      .ph__tag {
        display: inline-block;
        font-size: 12px;
        color: var(--text-faint);
        border: 1px solid var(--border);
        border-radius: var(--radius-full);
        padding: 4px 12px;
      }
    `,
  ],
})
export class PagePlaceholder {
  readonly title = input('Coming soon');
  readonly subtitle = input('This part of Hirepath is on the way.');
  readonly icon = input('hourglass_empty');
}
