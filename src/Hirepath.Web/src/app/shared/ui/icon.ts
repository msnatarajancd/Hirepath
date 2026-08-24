import { ChangeDetectionStrategy, Component, input } from '@angular/core';

/**
 * Renders a Material Symbols (Rounded) ligature. Thin wrapper so icons are
 * consistent and we never sprinkle raw font spans through templates.
 */
@Component({
  selector: 'app-icon',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<span
    class="material-symbols-rounded"
    [style.font-size.px]="size()"
    aria-hidden="true"
    >{{ name() }}</span
  >`,
  styles: [
    `
      :host {
        display: inline-flex;
        line-height: 0;
      }
      .material-symbols-rounded {
        font-family: 'Material Symbols Rounded';
        font-weight: normal;
        font-style: normal;
        user-select: none;
        font-variation-settings:
          'FILL' 0,
          'wght' 400,
          'GRAD' 0,
          'opsz' 24;
      }
    `,
  ],
})
export class Icon {
  readonly name = input.required<string>();
  readonly size = input(22);
}
