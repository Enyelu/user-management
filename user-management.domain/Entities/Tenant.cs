namespace user_management.domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> LockIds { get; set; } = new List<string>();
        public ICollection<Staff> StaffMembers {  get; set; } = new List<Staff>();
    }
}
