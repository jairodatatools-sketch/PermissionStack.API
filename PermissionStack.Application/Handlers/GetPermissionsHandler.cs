using MediatR;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Application.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PermissionStack.Application.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionResponseDto>>
    {
        private readonly IPermissionService _service;
        private readonly IPermissionEventPublisher _eventPublisher;

        public GetPermissionsHandler(
            IPermissionService service,
            IPermissionEventPublisher eventPublisher)
        {
            _service = service;
            _eventPublisher = eventPublisher;
        }

        public async Task<IEnumerable<PermissionResponseDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _service.GetPermissionsAsync();

            await _eventPublisher.PublishAsync(new PermissionOperationEventDto
            {
                NameOperation = "get"
            });

            return result;
        }
    }
}


