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
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Web_Browser_2._0_UWP.Classes;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Web_Browser_2._0_UWP
{
    public sealed partial class TabContent : UserControl
    {

        List<string> searchTermsLocal = new List<string>();

        public TabViewItem CurrentTab;

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

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (sender.Text != string.Empty)
            {
                // Add the search term to the database table - SearchTerms
                DataAccess.AddSearchTermToTable(sender.Text, DateTime.Now, 0);
                // Search string and navigate
                Browser.Source = new Uri("https://www.google.co.uk/search?q=" + sender.Text);
                GetSearchTermsList();
                
            }
        }

        private void SearchUrlBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var searches = searchTermsLocal;
            List<string> filteredSearchTerms = new List<string>();

            foreach (string searchTerm in searches)
            {
                //TODO ? Change StartsWith to Contains
                if (searchTerm.ToLower().StartsWith(sender.Text.ToLower()))
                {
                    filteredSearchTerms.Add(searchTerm);
                }
            }

            sender.ItemsSource = filteredSearchTerms; 
        }

        private void SearchUrlBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Search string and navigate
            Browser.Source = new Uri("https://www.google.co.uk/search?q=" + sender.Text);
        }

        private void SearchUrlBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GetSearchTermsList();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if(Browser.CanGoForward)
                Browser.GoForward();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(Browser.CanGoBack)
                Browser.GoBack();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Browser.Source = new Uri("https://www.google.co.uk/");
        }

        private void NewTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.AddTab();
        }

        private void GetSearchTermsList()
        {
            //Debug.WriteLine("Focus Got");
            // Clear the search term list
            searchTermsLocal.Clear();
            // Gets the search terms from the database.
            searchTermsLocal = DataAccess.GetAllSearchedTerms();
        }

        private void Browser_NavigationCompleted(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            CurrentTab.HeaderTemplate = Application.Current.Resources["TabHeaderTemplate"] as DataTemplate;

            TabDetails tabDetails = new TabDetails();
            tabDetails.SiteUrl = Browser.CoreWebView2.Source;
            tabDetails.TabHeaderTitle = Browser.CoreWebView2.DocumentTitle;

            BitmapImage bmi = new BitmapImage(new Uri("https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=" + tabDetails.SiteUrl + "&size=16"));
            tabDetails.TabHeaderFavicon = bmi;

            CurrentTab.DataContext = tabDetails;

            // Add string to url bar
            SearchUrlBox.Text = Browser.CoreWebView2.Source;

            StopButton.Visibility = Visibility.Collapsed;
        }

        private void DownloadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Browser.CoreWebView2.OpenDefaultDownloadDialog();
        }

        private void DevtoolsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Browser.CoreWebView2.OpenDevToolsWindow();
        }

        private void refButton_Click(object sender, RoutedEventArgs e)
        {
            Browser.CoreWebView2.Reload();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Browser.CoreWebView2.Stop();
        }

        private void Browser_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            StopButton.Visibility = Visibility.Visible;
        }

        private void Browser_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            Browser.CoreWebView2.NewWindowRequested += Browser_NewWindowRequested;
            Browser.Source = new Uri(WebAddress);
        }

        private void Browser_NewWindowRequested(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            MainPage.Current.AddTab(args.Uri);
        }

        private void HistoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HistoryFlyoutMenu.ShowAt(MenuButton);
            HistoryFlyoutMenu.Placement = FlyoutPlacementMode.TopEdgeAlignedRight;
        }
    }
}
