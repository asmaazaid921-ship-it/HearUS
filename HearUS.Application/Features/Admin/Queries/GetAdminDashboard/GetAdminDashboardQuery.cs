using HearUS.Application.Features.Admin.Dtos;
using MediatR;

namespace HearUS.Application.Features.Admin.Queries.GetAdminDashboard;

public sealed record GetAdminDashboardQuery : IRequest<AdminDashboardDto>;
