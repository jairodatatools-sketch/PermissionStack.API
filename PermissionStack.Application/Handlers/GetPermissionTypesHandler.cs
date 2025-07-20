using MediatR;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Application.Queries;

namespace PermissionStack.Application.Handlers
{
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, IEnumerable<PermissionTypeDto>>
    {
        private readonly IPermissionService _service;

        public GetPermissionTypesHandler(IPermissionService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<PermissionTypeDto>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetPermissionTypesAsync();
        }
    }
}

