namespace user_management.domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Staff> StaffMembers {  get; set; } = new List<Staff>();
    }
}
