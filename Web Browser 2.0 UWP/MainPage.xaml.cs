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
using DataAccessLibrary;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Core;
using Web_Browser_2._0_UWP.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Web_Browser_2._0_UWP
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static MainPage Current;

        CoreApplicationViewTitleBar coreTitleBar;
        ApplicationViewTitleBar titleBar;

        public MainPage()
        {
            this.InitializeComponent();

            Current = this;

            coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            Window.Current.CoreWindow.Activated += CoreWindow_Activated;

            DefaultTabLoad();
        }

        private void DefaultTabLoad()
        {
            DefaultTab.HeaderTemplate = Application.Current.Resources["TabHeaderTemplate"] as DataTemplate;

            TabContent tabContent = new TabContent();
            tabContent.CurrentTab = DefaultTab;
            tabContent.WebAddress = "https://www.google.com";
            DefaultTab.Content = tabContent;
        }

        private void CoreWindow_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            UISettings settings = new UISettings();

            if(args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                AppTitleBar.Background = new SolidColorBrush(Colors.DarkSlateGray);
            }
            else
            {
                AppTitleBar.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void AddTab()
        {
            var newTab = new TabViewItem();
            newTab.HeaderTemplate = Application.Current.Resources["TabHeaderTemplate"] as DataTemplate;
            
            TabContent tabContent = new TabContent();
            tabContent.CurrentTab = newTab;
            tabContent.WebAddress = "https://www.google.com";
            newTab.Content = tabContent;

            OurTabView.TabItems.Add(newTab);
            OurTabView.SelectedItem = newTab;
        }

        private void TabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
        {
            AddTab();
        }

        private void TabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
        {

            if(OurTabView.TabItems.Count > 1)
            {
                sender.TabItems.Remove(args.Tab);
            }
            else
            {
                sender.TabItems.Remove(args.Tab);
                Environment.Exit(0);
            }

            
        }
    }
}
