using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using App18;
using HtmlAgilityPack;

namespace App18.Assets
{
    class StorageClass
    {
        public StorageFolder localFolder;
  
        public StorageClass()
        {
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        public void storeDocument(String fileName, String text)
        {
            CreateFile(fileName, text);
        }

        private async void CreateFile(String fileName, String text)
        {
            StorageFile newFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            await Windows.Storage.FileIO.WriteTextAsync(newFile, text);
        }
    }
}
