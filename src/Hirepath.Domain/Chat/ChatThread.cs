using Hirepath.Domain.Common;

namespace Hirepath.Domain.Chat;

/// <summary>
/// A conversation. Pinned context (resume, JD, analysis) travels with the thread
/// so follow-up questions answer against the real stored data cheaply.
/// </summary>
public sealed class ChatThread : Entity
{
    public string Title { get; set; } = "New conversation";

    public Guid? PinnedResumeId { get; set; }
    public Guid? PinnedJobDescriptionId { get; set; }
    public Guid? PinnedAnalysisId { get; set; }

    public List<ChatMessage> Messages { get; set; } = [];
}
