import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AiProvider, UpdateSettingsRequest } from '../../core/api/hirepath-api.client';
import { Icon } from '../../shared/ui/icon';
import { SettingsStore } from './settings-store';

@Component({
  selector: 'app-settings',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, Icon],
  templateUrl: './settings.html',
})
export class Settings implements OnInit {
  protected readonly store = inject(SettingsStore);
  protected readonly saved = signal(false);

  protected readonly providers: readonly AiProvider[] = ['Fake', 'Anthropic', 'OpenAI', 'Ollama'];

  protected readonly form = new FormGroup({
    provider: new FormControl<AiProvider>('Fake', { nonNullable: true }),
    model: new FormControl('', { nonNullable: true }),
    apiKey: new FormControl('', { nonNullable: true }),
    baseUrl: new FormControl('', { nonNullable: true }),
    theme: new FormControl<'dark' | 'light'>('dark', { nonNullable: true }),
    demoMode: new FormControl(true, { nonNullable: true }),
    redactPii: new FormControl(false, { nonNullable: true }),
  });

  async ngOnInit(): Promise<void> {
    await this.store.load();
    const s = this.store.settings();
    if (s) {
      this.form.patchValue({
        provider: s.provider,
        model: s.model ?? '',
        baseUrl: s.baseUrl ?? '',
        theme: s.theme === 'light' ? 'light' : 'dark',
        demoMode: s.demoMode,
        redactPii: s.redactPii,
      });
    }
  }

  protected async submit(): Promise<void> {
    const v = this.form.getRawValue();
    const request: UpdateSettingsRequest = {
      provider: v.provider,
      model: v.model.trim() || undefined,
      // Empty = leave the stored key unchanged (null); a typed value updates it.
      apiKey: v.apiKey.length > 0 ? v.apiKey : undefined,
      baseUrl: v.baseUrl.trim() || undefined,
      theme: v.theme,
      demoMode: v.demoMode,
      redactPii: v.redactPii,
    };

    const ok = await this.store.save(request);
    if (ok) {
      this.form.controls.apiKey.reset('');
      this.saved.set(true);
      setTimeout(() => this.saved.set(false), 2500);
    }
  }

  protected get needsKey(): boolean {
    const p = this.form.controls.provider.value;
    return p === 'Anthropic' || p === 'OpenAI';
  }
}
