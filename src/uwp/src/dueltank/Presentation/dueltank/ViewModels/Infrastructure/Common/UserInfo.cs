using System;

namespace dueltank.ViewModels.Infrastructure.Common
{
    public class UserInfo
    {
        public static readonly UserInfo Default = new UserInfo
        {
            AccountName = "Fable Code",
            FirstName = "Fable",
            LastName = "Code"
        };

        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object PictureSource { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public bool IsEmpty => string.IsNullOrEmpty(DisplayName.Trim());
    }
}