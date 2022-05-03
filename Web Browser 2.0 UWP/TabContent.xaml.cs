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
using Windows.UI.Popups;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Web_Browser_2._0_UWP
{
    public sealed partial class TabContent : UserControl
    {
        public TabContent()
        {
            this.InitializeComponent();
        }



        public string WebAddress
        {
            get { return (string)GetValue(WebAddressProperty); }
            set { SetValue(WebAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WebAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WebAddressProperty =
            DependencyProperty.Register("WebAddress", typeof(string), typeof(TabContent), new PropertyMetadata("https://www.google.com"));

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // Add the search term to the database table - SearchTerms
            DataAccess.AddSearchTermToTable(sender.Text, DateTime.Now, 0);
            // Search string and navigate
            Browser.Source = new Uri("https://www.google.co.uk/search?q=" + sender.Text);
        }
    }
}
