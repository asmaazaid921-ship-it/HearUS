using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Jobs.Common;

internal static class JobDraftMapper
{
    public static Job Apply(IJobDraft draft, Job job)
    {
        job.Title = draft.Title!.Trim();
        job.CompanyName = draft.CompanyName!.Trim();
        job.Description = draft.Description!.Trim();
        job.SignVideoUrl = Normalize(draft.SignVideoUrl);
        job.Location = Normalize(draft.Location);
        job.ApplyUrl = Normalize(draft.ApplyUrl);
        job.ContactEmail = Normalize(draft.ContactEmail);
        job.IsActive = draft.IsActive;
        return job;
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
