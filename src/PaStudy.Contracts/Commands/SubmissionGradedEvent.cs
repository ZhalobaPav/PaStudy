using PaStudy.Contracts.Base;

namespace PaStudy.Contracts.Commands;

public record SubmissionGradedEvent(
    Guid EventId,
    DateTime OccurredOn,
    int SubmissionId,
    int CourseId,
    string CourseTitle,
    string AssignmentTitle,
    decimal Grade,
    int MaxPoints,
    string StudentUserId
) : IIntegrationEvent;