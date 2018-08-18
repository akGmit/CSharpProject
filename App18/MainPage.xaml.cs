using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using Windows.UI.Popups;
using App18.Assets;

namespace App18
{

    public sealed partial class MainPage : Page
    {

        StorageClass storage = new StorageClass();

        public MainPage()
        {
            this.InitializeComponent();
            wikiView.Navigate(new Uri("http://www.wikipedia.com"));
        }

        private void AppBarButton_SaveDocument(object sender, RoutedEventArgs e)
        {         
           getHtml();           
        }

        private void AppBarButton_ViewStorage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StorageViewer));
        }

        private async void getHtml()
        {
            string html = await wikiView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var text = doc.GetElementbyId("bodyContent");
            var fileName = doc.GetElementbyId("firstHeading");

            if (text != null)
            {
                storage.storeDocument(fileName.InnerText, text.InnerText);
                var dialog = new MessageDialog("Document saved to storage.");
                await dialog.ShowAsync();
            }
            else
            {
                var dialog = new MessageDialog("Document not suitable for saving.");
                await dialog.ShowAsync();
            }                       
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FileViewer), "Document herer");
        }
    }
}
