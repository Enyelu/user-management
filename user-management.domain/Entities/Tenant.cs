namespace user_management.domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public List<Guid> LockIds { get; set; } = new List<Guid>();
        public ICollection<Staff> StaffMembers {  get; set; } = new List<Staff>();
    }
}
