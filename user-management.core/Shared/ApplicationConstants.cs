using System;
using user_management.domain.Entities;

namespace user_management.core.Shared
{
    public static class ApplicationConstants
    {
        public static string EmailConfirmationMsg = $"<p> Dear FirstName \n Confirmation of your email is one click away <a href='callbackUrl'>click here</a> to continue</P>";
        public static string ResetPasswordMsg = $"<p> Dear FirstName, to reset your password, <a href='url'>click here</a></p>";
        public static string OnboardTenantMsg = $"<p> Dear Name, welcome to clay solutions, Your tenant ID is TenantId. <a href='https://www.my-clay.com/'>Click here to read more about us</a></p>";
        public static string LockAddedMsg = $"<p> Dear Name, a new lock has been added to you account. Lock Id LockId. <a href='https://www.my-clay.com/'>Click here to read more about us</a></p>";
    }
}
