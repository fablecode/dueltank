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
using dueltank.ViewModels.Username;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace dueltank.Views.Username
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsernameView : Page
    {
        public UsernameView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = DataContext as UsernameViewModel;
            viewModel?.LoadUserInfo(e.Parameter as UserInfo);

            base.OnNavigatedTo(e);
        }
    }
}
