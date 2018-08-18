using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App18.Assets;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace App18
{
    
    public sealed partial class FileViewer : Page
    {
        //String document;
        StorageClass storage;
        String fileNameSaved;

        public FileViewer()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {          
                base.OnNavigatedTo(e);

                String doc = (String)e.Parameter;

                editor.Text = doc;
        }

        private void AppBarButton_GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void AppBarButton_Save(object sender, RoutedEventArgs e)
        {
            storage = new StorageClass();

           
                TextBox inputTextBox = new TextBox();
                inputTextBox.AcceptsReturn = false;
                inputTextBox.Height = 32;
                ContentDialog dialog = new ContentDialog();
                dialog.Content = inputTextBox;
                dialog.Title = "Enter Document Name";
                dialog.IsSecondaryButtonEnabled = true;
                dialog.PrimaryButtonText = "Ok";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary && inputTextBox.Text != null && inputTextBox.Text != "")
            {
                storage.storeDocument(inputTextBox.Text, editor.Text);
            }
            else
            {
                var errorDialog = new MessageDialog("Please enter file name.");
                noFileName();
                async void noFileName()
                {
                    await errorDialog.ShowAsync();
                }
            }
               
            /*String name = await FileNameDialogAsync("Enter document name");

            async Task<string> FileNameDialogAsync(string title)
            {
                TextBox inputTextBox = new TextBox();
                inputTextBox.AcceptsReturn = false;
                inputTextBox.Height = 32;
                ContentDialog dialog = new ContentDialog();
                dialog.Content = inputTextBox;
                dialog.Title = title;
                dialog.IsSecondaryButtonEnabled = true;
                dialog.PrimaryButtonText = "Ok";
                if (await dialog.ShowAsync() == ContentDialogResult.Primary) 
                    return inputTextBox.Text;
                else
                    return null;
            }

            if (name != "" || name != null)
            {
                storage.storeDocument(name, editor.Text);
            }
            else
            {               
                var dialog = new MessageDialog("Please enter new file name.");
                noFileName();
                async void noFileName()
                {
                    await dialog.ShowAsync();
                }
            }*/
        }

        private async void AppBarButton_Export(object sender, RoutedEventArgs e)
        {
           
            
            // Open a text file.
            Windows.Storage.Pickers.FileSavePicker save =
                    new Windows.Storage.Pickers.FileSavePicker();
            save.SuggestedStartLocation = 
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                save.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

            Windows.Storage.StorageFile file = await save.PickSaveFileAsync();

            if (file != null)
            {
                
                Windows.Storage.CachedFileManager.DeferUpdates(file);
               
                await Windows.Storage.FileIO.WriteTextAsync(file, editor.Text);
                
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
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

        private async void AppBarButton_ImportFile(object sender, RoutedEventArgs e)
        {
            
                // Open a text file.
                Windows.Storage.Pickers.FileOpenPicker open =
                    new Windows.Storage.Pickers.FileOpenPicker();
                open.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                open.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

                if (file != null)
                {
                    try
                    {
                        Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                        // Load the file into the Document property of the RichEditBox.
                        //editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.None, randAccStream);
                    StreamReader stream = new StreamReader(randAccStream.AsStream());
                    string text = stream.ReadToEnd();
                    editor.Text = text;

                }
                    catch (Exception)
                    {
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
    }

}



