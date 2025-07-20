using MediatR;
using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Queries
{
    public class GetPermissionTypesQuery : IRequest<IEnumerable<PermissionTypeDto>>
    {
    }
}

