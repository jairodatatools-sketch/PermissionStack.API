using MediatR;
using PermissionStack.Application.Commands;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, bool>
{
    private readonly IPermissionService _permissionService;
    private readonly IPermissionEventPublisher _eventPublisher;

    public ModifyPermissionHandler(
        IPermissionService permissionService,
        IPermissionEventPublisher eventPublisher)
    {
        _permissionService = permissionService;
        _eventPublisher = eventPublisher;
    }

    public async Task<bool> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
        var success = await _permissionService.ModifyPermissionAsync(request.Id, request.Dto);

        if (!success)
            return false;

        await _eventPublisher.PublishAsync(new PermissionOperationEventDto
        {
            NameOperation = "modify"
        });

        return true;
    }
}


