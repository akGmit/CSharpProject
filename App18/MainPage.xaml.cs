using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using Windows.UI.Popups;
using App18.Assets;
using WikiStorage.Assets;
using Windows.UI.Xaml.Media;
using WikiStorage;

namespace App18
{
    //Main page of app
    //WebView class window with wikipedia start page loaded

    public sealed partial class MainPage : Page
    {
        //Create storage class object pointing to app local folder
        StorageClass storage = new StorageClass();
        //Settings class objects pointing to app local settings 
        Settings settings = new Settings();

        public MainPage()
        {
            this.InitializeComponent();
            //Webview default website on app run
            wikiView.Navigate(new Uri("http://www.wikipedia.com"));
            //Get rootGrid color from local settings using Settings object
            rootGrid.Background = settings.GetColor();
        }
        //Button event to save document to local storage
        private void AppBarButton_SaveDocument(object sender, RoutedEventArgs e)
        {  
           //Call to method to get Html data in text
           getHtml();           
        }
        //Method to scrape text from html using HTMLAgility pack
        private async void getHtml()
        {
            //Using webview class method to execute sript on html document which will get all html data
            String html = await wikiView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
            //HtmlDoument object to load HTML from string
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            //variable to store html element data by it id, which in this case is "bodyContent", this text will be the info we save to storage
            var text = doc.GetElementbyId("bodyContent");
            //variable to store file name of scrapped text, we get it by element id which is "firstHeading"
            var fileName = doc.GetElementbyId("firstHeading");
            //If text is not null
            if (text != null)
            {
                //we use Storage class to store document with text and its name 
                storage.storeDocument(fileName.InnerText, text.InnerText);
                //Message to inform user of succesful saving to storage
                var dialog = new MessageDialog("Document saved to storage.");
                await dialog.ShowAsync();
            }
            //If text is null, when something went wrong and we display message 
            else
            {
                var dialog = new MessageDialog("Document not suitable for saving.");
                await dialog.ShowAsync();
            }                       
        }
        //Button events to navigate through app pages
        private void AppBarButton_FileViewer(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FileViewer), "Document here");
        }

        private void AppBarButton_Settings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void AppBarButton_ViewStorage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StorageViewer));
        }
    }
}
