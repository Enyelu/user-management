using System;
using user_management.domain.Entities;

namespace user_management.core.Shared
{
    public static class ApplicationConstants
    {
        public static string EmailConfirmationMsg = $"<p> Dear FirstName \n Confirmation of your email is one click away <a href='callbackUrl'>click here</a> to continue</P>";
        public static string ResetPasswordMsg = $"<p> Dear FirstName, to reset your password, <a href='url'>click here</a></p>";
    }
}
