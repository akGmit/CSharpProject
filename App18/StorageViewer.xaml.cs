using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App18.Assets;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App18
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StorageViewer : Page
    {
        StorageClass storage = new StorageClass();

        public StorageViewer()
        {
            this.InitializeComponent();
            ListStorageFiles();
        }

        async void ListStorageFiles()
        {
            IReadOnlyList<StorageFile> storageFiles = await storage.localFolder.GetFilesAsync();

            foreach (StorageFile file in storageFiles)
            {
                storageList.Items.Add(file.DisplayName);
                //storageList.Items.Add(file.DisplayName);
            }
        }

        private void storageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            text.Text = storageList.SelectedItem.ToString();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (storageList.SelectedIndex != -1)
            {
                string doc = System.IO.File.ReadAllText(storage.localFolder.Path + "/" + storageList.SelectedItem.ToString());

                text.Text = doc;

                Frame.Navigate(typeof(FileViewer), doc);
            }
            else
            {
                var dialog = new MessageDialog("Select file to view.");
                noFileSelected();
                async void noFileSelected()
                {
                    await dialog.ShowAsync();
                }
            }
        }
    }
}
