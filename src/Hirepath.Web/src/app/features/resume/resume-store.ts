import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Client, ResumeSummaryDto } from '../../core/api/hirepath-api.client';

type Status = 'idle' | 'loading' | 'error';

/** Owns the list of resumes and the upload/manage operations. */
@Injectable({ providedIn: 'root' })
export class ResumeStore {
  private readonly client = inject(Client);
  private readonly http = inject(HttpClient);

  private readonly _resumes = signal<ResumeSummaryDto[]>([]);
  private readonly _status = signal<Status>('idle');
  private readonly _uploading = signal(false);
  private readonly _error = signal<string | null>(null);

  readonly resumes = this._resumes.asReadonly();
  readonly status = this._status.asReadonly();
  readonly uploading = this._uploading.asReadonly();
  readonly error = this._error.asReadonly();
  readonly loading = computed(() => this._status() === 'loading');
  readonly isEmpty = computed(() => this._status() === 'idle' && this._resumes().length === 0);

  async load(): Promise<void> {
    this._status.set('loading');
    this._error.set(null);
    try {
      this._resumes.set(await firstValueFrom(this.client.listResumes()));
      this._status.set('idle');
    } catch {
      this._status.set('error');
      this._error.set('Could not load resumes.');
    }
  }

  async upload(file: File, label?: string): Promise<boolean> {
    this._uploading.set(true);
    this._error.set(null);
    try {
      // The generated client's multipart upload is unreliable (NSwag + .NET
      // form-file schema), so we post the file directly. See ADR 0006. Every
      // other operation goes through the generated client.
      const form = new FormData();
      form.append('file', file, file.name);
      if (label) {
        form.append('label', label);
      }
      await firstValueFrom(this.http.post('/api/resumes', form));
      await this.load();
      return true;
    } catch (err) {
      this._error.set(this.messageFor(err));
      return false;
    } finally {
      this._uploading.set(false);
    }
  }

  async setDefault(id: string): Promise<void> {
    await firstValueFrom(this.client.setDefaultResume(id));
    await this.load();
  }

  async remove(id: string): Promise<void> {
    await firstValueFrom(this.client.deleteResume(id));
    await this.load();
  }

  private messageFor(err: unknown): string {
    const status = (err as { status?: number })?.status;
    if (status === 415) {
      return 'That file type is not supported. Use a PDF, DOCX, or TXT.';
    }
    if (status === 400) {
      return 'That file could not be read. Check it is under 10 MB and not empty.';
    }
    return 'Upload failed. Please try again.';
  }
}
