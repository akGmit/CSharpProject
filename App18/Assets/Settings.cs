using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace WikiStorage.Assets
{
    //Settings class to deal with user settings at local app settings 

    class Settings
    {
        //Container for app settings 
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //SolidColorBrush class object to store settings color
        SolidColorBrush color = new SolidColorBrush();
        
        //Constructor to load default or saved settings
        public Settings()
        {
            //If color settings is empty/null load default White color
            if(localSettings.Values["BackgroundColor"] == null)
            {
                localSettings.Values["BackgroundColor"] = "White";
                color = new SolidColorBrush(Windows.UI.Colors.White);
            }
            //If local settings contain user saved settings, load themto color object
            else
            {
                color = GetBackgroundColor();
            }
        }
        //Return color object as set by user
        public SolidColorBrush GetColor()
        {
            return color;
        }
        //Method to determine color object from localSettings values 
        private SolidColorBrush GetBackgroundColor()
        {
            String colorName = (String)localSettings.Values["BackgroundColor"];

            if (colorName == "White")
            {          
                color = new SolidColorBrush(Windows.UI.Colors.White);
            }
            else if(colorName == "Gray")
            {
                color = new SolidColorBrush(Windows.UI.Colors.Gray);
            }
            else if (colorName == "Lavender")
            {
                color = new SolidColorBrush(Windows.UI.Colors.Lavender);
            }
            else if (colorName == "Olive")
            {
                color = new SolidColorBrush(Windows.UI.Colors.Olive);
            }
            else if (colorName == "Linen")
            {
                color = new SolidColorBrush(Windows.UI.Colors.Linen);
            }

            return color;
        }
        //Method to set local settings value
        public void SetBackgroundColor(String clr)
        {
            localSettings.Values["BackgroundColor"] = clr;
        }
    }
}
