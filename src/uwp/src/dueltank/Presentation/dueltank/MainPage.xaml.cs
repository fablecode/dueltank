using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace dueltank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CustomizeTitleBar();
        }

        void MaximizeWindowOnLoad()
        {
            //var view = DisplayInformation.GetForCurrentView();

            //// Get the screen resolution (APIs available from 14393 onward).
            //var resolution = new Size(view.ScreenWidthInRawPixels, view.ScreenHeightInRawPixels);

            //// Calculate the screen size in effective pixels. 
            //// Note the height of the Windows Taskbar is ignored here since the app will only be given the maxium available size.
            //var scale = view.ResolutionScale == ResolutionScale.Invalid ? 1 : view.RawPixelsPerViewPixel;
            //var bounds = new Size(resolution.Width / scale, resolution.Height / scale);

            //ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void CustomizeTitleBar()
        {
            // customize title area
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(AppTitleBar);


            // customize buttons' colors
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = Colors.Red;
            titleBar.ForegroundColor = Colors.White;


            titleBar.ButtonBackgroundColor = Colors.Red;
            titleBar.ButtonHoverBackgroundColor = GetSolidColorBrush("#20FFFFFF ").Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonInactiveForegroundColor = GetSolidColorBrush("#FFF58989").Color;
            titleBar.ButtonInactiveBackgroundColor = Colors.Red;

        }

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
