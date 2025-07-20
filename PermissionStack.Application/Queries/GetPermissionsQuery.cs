using MediatR;
using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Queries
{
    public class GetPermissionsQuery : IRequest<IEnumerable<PermissionResponseDto>> 
    {
    }

}
