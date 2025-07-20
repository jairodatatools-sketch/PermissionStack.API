using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Interfaces
{
    public interface IPermissionEventPublisher
    {
        Task PublishAsync(PermissionOperationEventDto dto);
    }
}
