import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Icon } from '../../shared/ui/icon';
import { CanvasStore } from '../../core/layout/canvas-store';

interface Suggestion {
  readonly icon: string;
  readonly text: string;
}

/**
 * The conversation surface — the app's front door. In Phase 0 this renders the
 * onboarding empty state and a working composer; the live assistant, tools, and
 * widgets arrive in later phases.
 */
@Component({
  selector: 'app-chat',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, Icon],
  templateUrl: './chat.html',
  styleUrl: './chat.scss',
})
export class Chat {
  private readonly canvas = inject(CanvasStore);

  protected readonly draft = signal('');

  protected readonly suggestions: readonly Suggestion[] = [
    { icon: 'upload_file', text: 'Analyse my resume against a job description' },
    { icon: 'target', text: 'Where are the biggest gaps for this role?' },
    { icon: 'edit_note', text: 'Rewrite a bullet for more impact' },
    { icon: 'compare_arrows', text: 'Compare two of my resumes' },
  ];

  protected useSuggestion(text: string): void {
    this.draft.set(text);
  }

  protected previewCanvas(): void {
    this.canvas.open('Full analysis');
  }

  protected send(): void {
    // Wired to the assistant in Phase 3. For now the composer is inert on submit.
    if (this.draft().trim().length === 0) {
      return;
    }
  }
}
