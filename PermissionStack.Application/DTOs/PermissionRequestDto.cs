namespace PermissionStack.Application.DTOs
{
    public class PermissionRequestDto
    {
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime PermissionDate { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
