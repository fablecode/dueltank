using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using dueltank.ViewModels.Infrastructure.Common;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace dueltank.Views.Dialogs.UsernameDialog
{
    public enum UsernameResult
    {
        UsernameOk,
        UsernameFail,
        UsernameCancel,
        Nothing
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsernameContentDialog : ContentDialog
    {
        private readonly UserInfo _userInfo;
        public UsernameResult Result { get; private set; }

        public UsernameContentDialog(UserInfo userInfo)
        {
            _userInfo = userInfo;
            InitializeComponent();
            Opened += UsernameContentDialog_Opened;
            Closing += UsernameContentDialog_Closing;
        }

        private void UsernameContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            Result = UsernameResult.Nothing;
        }

        private void UsernameContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            // User clicked Cancel, ESC, or the system back button.
            Result = UsernameResult.UsernameCancel;
        }
    }
}
