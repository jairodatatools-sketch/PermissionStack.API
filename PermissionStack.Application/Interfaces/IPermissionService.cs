using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<int> RequestPermissionAsync(PermissionRequestDto dto);
        Task<bool> ModifyPermissionAsync(int id, PermissionRequestDto dto);
        Task<IEnumerable<PermissionResponseDto>> GetPermissionsAsync();
    }
}

