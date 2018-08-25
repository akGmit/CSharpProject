using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App18.Assets;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.IO;
using WikiStorage.Assets;
using WikiStorage;

namespace App18
{
    //FileViewer page for viewing, editing, saving and importing/exporting documents to local storage or to specified system folder
    public sealed partial class FileViewer : Page
    {
        //Create storage class object pointing to app local folder
        StorageClass storage;
        //Settings class objects pointing to app local settings 
        Settings settings = new Settings();

        public FileViewer()
        {
            this.InitializeComponent();
            rootGrid.Background = settings.GetColor();
        }
        //When opening document from Storage Viewer, we load FileViewer
        //Navigating from StorageViewer to FileViewer with document passed as argument
        //Overriding OnNavigagetTo default method which fires when page is navigated to
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Document passed as parameter is casted to string and saved to doc string type variable
            String doc = (String)e.Parameter;
            //Text bloc "editor" is displaying doc contents
            editor.Text = doc;
        }
        //Save document to app local storage 
        private async void AppBarButton_Save(object sender, RoutedEventArgs e)
        {
            //Create new Storage object
            storage = new StorageClass();
            //Create textbox for new file name input
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            //Create content dialog box
            ContentDialog dialog = new ContentDialog();
            //Load inputTextBox to dialog content
            dialog.Content = inputTextBox;
            dialog.Title = "Enter Document Name";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            //If file name text box is not empty save document to local storage with specified name and editor contents
            if (await dialog.ShowAsync() == ContentDialogResult.Primary && inputTextBox.Text != null && inputTextBox.Text != "")
            {
                storage.storeDocument(inputTextBox.Text, editor.Text);
            }
            //If file name is empty display error message
            else
            {
                var errorDialog = new MessageDialog("Please enter file name.");
                noFileName();
                async void noFileName()
                {
                    await errorDialog.ShowAsync();
                }
            }
        }
        //Export editor contents to specified directory
        private async void AppBarButton_Export(object sender, RoutedEventArgs e)
        {
            //Create FileSavePicker object for picking saving/exporting location
            //Default open folder will be Documents
            Windows.Storage.Pickers.FileSavePicker save =
                    new Windows.Storage.Pickers.FileSavePicker();
            save.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            //Available file type choices for saving
            save.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            //StorageFile to create save file
            Windows.Storage.StorageFile file = await save.PickSaveFileAsync();

            if (file != null)
            {

                //Windows.Storage.CachedFileManager.DeferUpdates(file);
                //FileIO object method to write file and its contents   
                await Windows.Storage.FileIO.WriteTextAsync(file, editor.Text);
                //FileUpdateStatus to see if file was saved succesfully    
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                //If/else statement to deal dispay message dialogs according to status
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    ContentDialog msgDialog = new ContentDialog()
                    {
                        Title = "File Saved",
                        Content = "Saved",
                        PrimaryButtonText = "Ok"
                    };

                    await msgDialog.ShowAsync();
                }
                else
                {
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "File save error",
                        Content = "Sorry, I couldn't create file.",
                        PrimaryButtonText = "Ok"
                    };

                    await dialog.ShowAsync();
                }
            }
            else
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = "Sorry, ecountered error, please try again.",
                    PrimaryButtonText = "Ok"
                };

                await dialog.ShowAsync();
            }
        }
        //Import file from speciafied folder
        private async void AppBarButton_ImportFile(object sender, RoutedEventArgs e)
        {

            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            //Show only txt files
            open.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();
            //If file is opened succesfully
            if (file != null)
            {
                //Try catch block for readng file streams
                try
                {
                    //Open stream for reading
                    Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    //Create StreamReader object with random access stream object as argument
                    StreamReader stream = new StreamReader(randAccStream.AsStream());
                    //Read stream to end and store in String type
                    String text = stream.ReadToEnd();
                    //Load string to editor box
                    editor.Text = text;

                }
                //Catch any exceptions if try block unsuccesful
                catch (Exception)
                {
                    //Create and display error dialog
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }
        //Navigation through pages button events
        private void AppBarButton_Settings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void AppBarButton_ViewStorage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StorageViewer));
        }

        private void AppBarButton_GoBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }

}



