import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Icon } from '../../shared/ui/icon';
import { ResumeStore } from './resume-store';

@Component({
  selector: 'app-resumes',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [Icon],
  templateUrl: './resumes.html',
})
export class Resumes implements OnInit {
  protected readonly store = inject(ResumeStore);
  protected readonly dragging = signal(false);

  async ngOnInit(): Promise<void> {
    await this.store.load();
  }

  protected onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.dragging.set(true);
  }

  protected onDragLeave(): void {
    this.dragging.set(false);
  }

  protected async onDrop(event: DragEvent): Promise<void> {
    event.preventDefault();
    this.dragging.set(false);
    const file = event.dataTransfer?.files?.[0];
    if (file) {
      await this.store.upload(file);
    }
  }

  protected async onPick(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (file) {
      await this.store.upload(file);
      input.value = '';
    }
  }

  protected formatSize(bytes: number): string {
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${Math.round(bytes / 1024)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }
}
