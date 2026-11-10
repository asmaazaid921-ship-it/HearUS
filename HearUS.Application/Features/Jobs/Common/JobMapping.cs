using HearUS.Application.Features.Jobs.Dtos;
using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Jobs.Common;

internal static class JobMapping
{
    public static JobSummaryDto ToSummary(Job job) => new()
    {
        Id = job.Id,
        Title = job.Title,
        CompanyName = job.CompanyName,
        Description = job.Description,
        Location = job.Location,
        IsRemote = job.IsRemote,
        IsAccessibilityFriendly = job.IsAccessibilityFriendly,
        SignVideoUrl = job.SignVideoUrl,
        PostedAt = job.PostedAt,
        IsActive = job.IsActive
    };
}
