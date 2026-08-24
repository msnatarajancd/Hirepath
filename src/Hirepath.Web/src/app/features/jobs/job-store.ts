import { Injectable, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Client, CreateJobRequest, JobSummaryDto } from '../../core/api/hirepath-api.client';

type Status = 'idle' | 'loading' | 'error';

/** Owns the saved job descriptions and the paste-to-create operation. */
@Injectable({ providedIn: 'root' })
export class JobStore {
  private readonly client = inject(Client);

  private readonly _jobs = signal<JobSummaryDto[]>([]);
  private readonly _status = signal<Status>('idle');
  private readonly _saving = signal(false);
  private readonly _error = signal<string | null>(null);

  readonly jobs = this._jobs.asReadonly();
  readonly status = this._status.asReadonly();
  readonly saving = this._saving.asReadonly();
  readonly error = this._error.asReadonly();
  readonly loading = computed(() => this._status() === 'loading');
  readonly isEmpty = computed(() => this._status() === 'idle' && this._jobs().length === 0);

  async load(): Promise<void> {
    this._status.set('loading');
    this._error.set(null);
    try {
      this._jobs.set(await firstValueFrom(this.client.listJobs()));
      this._status.set('idle');
    } catch {
      this._status.set('error');
      this._error.set('Could not load job descriptions.');
    }
  }

  async create(request: CreateJobRequest): Promise<boolean> {
    this._saving.set(true);
    this._error.set(null);
    try {
      await firstValueFrom(this.client.createJob(request));
      await this.load();
      return true;
    } catch {
      this._error.set('Could not save the job description.');
      return false;
    } finally {
      this._saving.set(false);
    }
  }
}
