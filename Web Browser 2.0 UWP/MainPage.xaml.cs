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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Web_Browser_2._0_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void AddTab()
        {
            var newTab = new TabViewItem();
            newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Document";

            TabContent tabContent = new TabContent();
            tabContent.WebAddress = "https://www.google.com";
            newTab.Content = tabContent;

            OurTabView.TabItems.Add(newTab);
        }

        private void TabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
        {
            var newTab = new TabViewItem();
            newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Document";

            TabContent tabContent = new TabContent();
            tabContent.WebAddress = "https://www.google.com";
            newTab.Content = tabContent;

            sender.TabItems.Add(newTab);
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
