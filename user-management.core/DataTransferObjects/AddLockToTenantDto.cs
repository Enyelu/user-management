namespace user_management.core.DataTransferObjects
{
    public class AddLockToTenantDto
    {
        public string LockId { get; set; }//encrypted
        public string TenantId { get; set; }//encrypted
    }
}
