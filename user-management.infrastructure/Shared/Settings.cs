namespace user_management.infrastructure.Shared
{
    public class Settings
    {
        public string CipherKeyIvPhrase { get; set; }
        public string ApplicationBaseUrl { get; set; }
        public string ConfirmEmailUrl { get; set; }
        public string ResetPasswordEmailUrl { get; set; }
        public string EmailSenderUrl { get; set; }
        public string EmailSenderAppKey { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSenderEmail { get; set; }
    }
}
