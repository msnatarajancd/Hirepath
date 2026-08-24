import { ChangeDetectionStrategy, Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateJobRequest } from '../../core/api/hirepath-api.client';
import { Icon } from '../../shared/ui/icon';
import { JobStore } from './job-store';

@Component({
  selector: 'app-jobs',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, Icon],
  templateUrl: './jobs.html',
})
export class Jobs implements OnInit {
  protected readonly store = inject(JobStore);

  protected readonly form = new FormGroup({
    company: new FormControl('', { nonNullable: true }),
    title: new FormControl('', { nonNullable: true }),
    rawText: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(30)],
    }),
  });

  async ngOnInit(): Promise<void> {
    await this.store.load();
  }

  protected async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    const request: CreateJobRequest = {
      rawText: v.rawText.trim(),
      company: v.company.trim() || undefined,
      title: v.title.trim() || undefined,
      location: undefined,
      workMode: 'Unspecified',
      seniority: 'Unspecified',
    };

    const ok = await this.store.create(request);
    if (ok) {
      this.form.reset({ company: '', title: '', rawText: '' });
    }
  }
}
