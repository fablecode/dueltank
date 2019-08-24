using System;

namespace dueltank.ViewModels.Infrastructure.Common
{
    public class UserInfo
    {
        public static readonly UserInfo Default = new UserInfo
        {
            AccountName = "Fable Code",
            FirstName = "Fable",
            LastName = "Code",
        };

        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object PictureSource { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public bool IsEmpty => string.IsNullOrEmpty(DisplayName.Trim());
        public string AppToken { get; set; }
        public string JwtToken { get; set; }
    }
}