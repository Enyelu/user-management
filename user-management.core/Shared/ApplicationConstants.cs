namespace user_management.core.Shared
{
    public static class ApplicationConstants
    {
        public static string EmailConfirmationMsg = $"<p> Dear FirstName, <br/><br/> Confirmation of your email is one click away <a href='callbackUrl'>click here</a> to continue</P>";
        public static string ResetPasswordMsg = $"<p> Dear FirstName, <br/><br/> To reset your password, <a href='url'>click here</a></p>";
        public static string OnboardTenantMsg = $"<p> Dear Name, <br/><br/> Welcome to clay solutions, Your tenant ID is TenantId. <a href='https://www.my-clay.com/'>Click here to read more about us</a></p>";
        public static string LockAddedMsg = $"<p> Dear Name, <br/><br/> A new lock has been added to your account. Lock Id LockId. <a href='https://www.my-clay.com/'>Click here to read more about us</a></p>";
        public static string RoleAssignmentMsg = $"<p> Dear Name, <br/><br/> A new role roleValue has been added to your account. <a href='https://www.my-clay.com/'>Click here to read more about us</a></p>";
    }
}
