using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App18.Assets;
using Windows.UI.Popups;


namespace App18
{
    
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
            }
        }

        private void AppBarButton_ViewDocument(object sender, RoutedEventArgs e)
        {
            if (storageList.SelectedIndex != -1)
            {
                string doc = System.IO.File.ReadAllText(storage.localFolder.Path + "/" + storageList.SelectedItem.ToString()+".txt");
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

        private void AppBarButton_GoToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton_DeleteDocument(object sender, RoutedEventArgs e)
        {
            if (storageList.SelectedIndex != -1)
            {
                storage.DeleteDocument(storageList.SelectedItem.ToString());
            }
            else
            {
                var dialog = new MessageDialog("Select file to delete.");
                noFileSelected();
                async void noFileSelected()
                {
                    await dialog.ShowAsync();
                }
            }
            storageList.DataContext = null;
        }

        private void Button_DeleteAll(object sender, RoutedEventArgs e)
        {
            storage.DeleteAllStorage();
        }

        private async void AppBarButton_Import(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker open =
                    new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            storage.ImportFile(file);
        }
    }
}
