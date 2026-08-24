import { render, screen } from '@testing-library/angular';
import userEvent from '@testing-library/user-event';
import { Chat } from './chat';

describe('Chat', () => {
  it('shows the onboarding welcome and suggestion chips', async () => {
    await render(Chat);

    expect(screen.getByRole('heading', { name: /find your fit/i })).toBeInTheDocument();
    expect(
      screen.getByRole('button', { name: /analyse my resume against a job description/i }),
    ).toBeInTheDocument();
  });

  it('fills the composer when a suggestion chip is clicked', async () => {
    const user = userEvent.setup();
    await render(Chat);

    await user.click(screen.getByRole('button', { name: /rewrite a bullet for more impact/i }));

    const input = screen.getByRole('textbox', { name: /message hirepath/i }) as HTMLInputElement;
    expect(input.value).toContain('Rewrite a bullet');
  });

  it('disables send while the composer is empty', async () => {
    await render(Chat);

    expect(screen.getByRole('button', { name: /send/i })).toBeDisabled();
  });
});
