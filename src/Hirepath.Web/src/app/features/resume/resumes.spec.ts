import { provideHttpClient } from '@angular/common/http';
import { render, screen } from '@testing-library/angular';
import { of } from 'rxjs';
import { Client, ResumeSummaryDto } from '../../core/api/hirepath-api.client';
import { Resumes } from './resumes';

function stubClient(resumes: ResumeSummaryDto[]): Client {
  return {
    listResumes: () => of(resumes),
  } as unknown as Client;
}

describe('Resumes', () => {
  it('shows the empty state when there are no resumes', async () => {
    await render(Resumes, {
      providers: [provideHttpClient(), { provide: Client, useValue: stubClient([]) }],
    });

    expect(await screen.findByText(/no resumes yet/i)).toBeInTheDocument();
  });

  it('lists resumes with the default badge', async () => {
    const resume: ResumeSummaryDto = {
      id: '1',
      label: 'My CV',
      fileName: 'cv.pdf',
      contentType: 'application/pdf',
      sizeBytes: 2048,
      isDefault: true,
      skillCount: 8,
      createdAt: new Date(),
    };
    await render(Resumes, {
      providers: [provideHttpClient(), { provide: Client, useValue: stubClient([resume]) }],
    });

    expect(await screen.findByText('My CV')).toBeInTheDocument();
    expect(screen.getByText(/default/i)).toBeInTheDocument();
  });
});
