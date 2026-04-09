namespace PaStudy.Core.Helpers.Enums;

public enum SubmissionStatus: byte
{
    Draft = 0,
    Submitted = 1,
    Graded = 2,
    LateAndRejected = 3,
    LateAndAccepted = 4,
    Rejected = 5
}
