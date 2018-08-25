using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App18.Assets;
using Windows.UI.Popups;
using WikiStorage.Assets;
using WikiStorage;

namespace App18
{
    //StorageViewer page to view local app storage 
    public sealed partial class StorageViewer : Page
    {
        StorageClass storage = new StorageClass();
        Settings settings = new Settings();

        public StorageViewer()
        {
            this.InitializeComponent();
            //Method call to populate listbox with items at local storage
            ListStorageFiles();
            rootGrid.Background = settings.GetColor();
        }
        //Method to view local storage items as Listbox items in listbox
        async void ListStorageFiles()
        {
            //Create read only list of local storage files using StorageClass object methods
            IReadOnlyList<StorageFile> storageFiles = await storage.localFolder.GetFilesAsync();
            //Populate list with items
            foreach (StorageFile file in storageFiles)
            {
                storageList.Items.Add(file.DisplayName);
            }
        }
        //View document metthod
        private void AppBarButton_ViewDocument(object sender, RoutedEventArgs e)
        {
            //If item is selected from listbox 
            if (storageList.SelectedIndex != -1)
            {
                //Using File class to read document contents to String variable
                String doc = System.IO.File.ReadAllText(storage.localFolder.Path + "/" + storageList.SelectedItem.ToString()+".txt");
                //Navigating to FileViewer page with document passed as argument
                Frame.Navigate(typeof(FileViewer), doc);
            }
            //If file was not selected display dialog box with message
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

        //Delete document button event
        private void AppBarButton_DeleteDocument(object sender, RoutedEventArgs e)
        {
            //If file chosen, delete that file from local storage
            if (storageList.SelectedIndex != -1)
            {
                storage.DeleteDocument(storageList.SelectedItem.ToString());
            }
            //If no file chosen from list box display message
            else
            {
                var dialog = new MessageDialog("Select file to delete.");
                noFileSelected();
                async void noFileSelected()
                {
                    await dialog.ShowAsync();
                }
            }
            
        }
        //Delete all button
        //Flyout will be displayed described in XAML code
        private void Button_DeleteAll(object sender, RoutedEventArgs e)
        {
            //Using StorageClass object to delete all files from local storage
            storage.DeleteAllStorage();
        }
        //Import file to stroarge from specified location
        private async void AppBarButton_Import(object sender, RoutedEventArgs e)
        {
            //Create FileOpenPicker object to display open file dialog
            Windows.Storage.Pickers.FileOpenPicker open =
                    new Windows.Storage.Pickers.FileOpenPicker();
            //Suggested default start location Documents
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            //Only show txt files
            open.FileTypeFilter.Add(".txt");
            //Open file
            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();
            //If file for importing opened succesfuly save it to locacl storage
            if (file != null)
            {
                storage.ImportFile(file);
                var dialog = new MessageDialog("File imported to local storage.");
                noFileSelected();
                async void noFileSelected()
                {
                    await dialog.ShowAsync();
                }
            }
            //If file wasnt opened succesfully display message
            else
            {
                var dialog = new MessageDialog("Choose file to import.");
                noFileSelected();
                async void noFileSelected()
                {
                    await dialog.ShowAsync();
                }
            }
        }
        //Navigation button events
        private void AppBarButton_Settings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void AppBarButton_GoToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
