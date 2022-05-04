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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Web_Browser_2._0_UWP
{
    public sealed partial class TabContent : UserControl
    {

        List<string> searchTermsLocal = new List<string>();

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
            
            
        }

        private void GetSearchTermsList()
        {
            //Debug.WriteLine("Focus Got");
            // Clear the search term list
            searchTermsLocal.Clear();
            // Gets the search terms from the database.
            searchTermsLocal = DataAccess.GetAllSearchedTerms();
        }

        
    }
}
