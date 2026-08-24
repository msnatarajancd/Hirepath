import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Icon } from '../../shared/ui/icon';
import { ThemeStore } from '../theme/theme-store';
import { CanvasStore } from './canvas-store';

interface NavItem {
  readonly icon: string;
  readonly label: string;
  readonly link: string;
}

/**
 * The app shell: a slim icon rail (the escape-hatch navigation), the
 * conversation column in the middle, and the canvas panel on the right.
 * One responsive decision lives here, not scattered across components.
 */
@Component({
  selector: 'app-shell',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Icon],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell {
  protected readonly theme = inject(ThemeStore);
  protected readonly canvas = inject(CanvasStore);

  protected readonly nav: readonly NavItem[] = [
    { icon: 'forum', label: 'Chat', link: '/chat' },
    { icon: 'insights', label: 'Analyses', link: '/analyses' },
    { icon: 'description', label: 'Resumes', link: '/resumes' },
    { icon: 'settings', label: 'Settings', link: '/settings' },
  ];
}
