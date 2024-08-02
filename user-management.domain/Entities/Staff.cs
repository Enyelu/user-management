namespace user_management.domain.Entities
{
    public class Staff : BaseEntity
    {
        public Guid AppUserId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
