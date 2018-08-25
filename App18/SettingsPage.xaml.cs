using App18;
using System;
using System.Collections.Generic;
using WikiStorage.Assets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Settings page for user settings

namespace WikiStorage
{

    public sealed partial class SettingsPage : Page
    {
        //List variable for available colors
        List<String> colors = new List<String>();
        //Settings class object to deal with user selected settings
        Settings settings = new Settings();

        public SettingsPage()
        {
            this.InitializeComponent();
            //Call to method to create color list and populate combo box with color items
            CreateColorList();
            rootGrid.Background = settings.GetColor();
        }

        private void CreateColorList()
        {
            colors.Add("White");
            colors.Add("Gray");
            colors.Add("Lavender");
            colors.Add("Olive");
            colors.Add("Linen");
            //Add colors as combo box items to combo box
            foreach (String color in colors)
            {
                colorsList.Items.Add(color);
            }
        }
        //On color selection set color using settings object
        private void colorsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Passing color name as argument to settings.SetBackgroundColor method
            settings.SetBackgroundColor(colorsList.SelectedItem.ToString());
        }
        
        //Navigation buttons events
        private void AppBarButton_GoToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
