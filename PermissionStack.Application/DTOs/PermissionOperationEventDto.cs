namespace PermissionStack.Application.DTOs
{
    public class PermissionOperationEventDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NameOperation { get; set; } // "modify", "request", "get"
    }
}
