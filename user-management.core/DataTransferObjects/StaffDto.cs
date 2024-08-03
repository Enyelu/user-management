namespace user_management.core.DataTransferObjects
{
    public class StaffDto
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
    }
}
