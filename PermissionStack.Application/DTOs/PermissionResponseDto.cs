namespace PermissionStack.Application.DTOs
{
    public class PermissionResponseDto
    {
        public int Id { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime PermissionDate { get; set; }
        public string PermissionTypeDescription { get; set; }
    }
}

