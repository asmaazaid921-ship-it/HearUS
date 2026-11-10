using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Insights.Common;

internal static class InsightAccess
{
    public static void EnsureCanManage(Insight insight, ICurrentUserService currentUser)
    {
        if (insight.AuthorId != currentUser.UserId && !currentUser.IsInRole(AppRoles.Admin))
        {
            throw new ForbiddenAccessException();
        }
    }
}
