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
using HtmlAgilityPack;
using System.Threading.Tasks;
using App18.Assets;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App18
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

       
    }
}
