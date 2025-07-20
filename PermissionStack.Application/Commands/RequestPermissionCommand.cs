using System.Security;
using MediatR;
using PermissionStack.Application.DTOs;


namespace PermissionStack.Application.Commands
{
    public class RequestPermissionCommand : IRequest<int>
    {
        public PermissionRequestDto Dto { get; }

        public RequestPermissionCommand(PermissionRequestDto dto)
        {
            Dto = dto;
        }
    }

}
