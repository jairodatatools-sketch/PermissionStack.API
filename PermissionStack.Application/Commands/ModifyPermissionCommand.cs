using MediatR;
using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Commands
{
    public class ModifyPermissionCommand : IRequest<bool>
    {
        public int Id { get; }
        public PermissionRequestDto Dto { get; }

        public ModifyPermissionCommand(int id, PermissionRequestDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }

}

