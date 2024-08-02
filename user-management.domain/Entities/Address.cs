namespace user_management.domain.Entities
{
    public class Address : BaseEntity
    {
        public string StreetNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}