namespace user_management.core.DataTransferObjects
{
    public class PasswordRestDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }//encrypted
        public string ConfirmNewPassword { get; set; }//encrypted
    }
}
