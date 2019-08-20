using System;
using dueltank.ViewModels.Infrastructure.Common;

namespace dueltank.ViewModels.Shell
{
    public class ShellArgs
    {
        public Type ViewModel { get; set; }
        public object Parameter { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}