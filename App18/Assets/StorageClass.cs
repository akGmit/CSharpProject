using System;
using System.Collections.Generic;
using Windows.Storage;

namespace App18.Assets
{
    //StorageClass to deal with local storage fnuctions
    class StorageClass
    {   
        //StorageFolder class object
        public StorageFolder localFolder;
  
        public StorageClass()
        {
            //StoragFolder objeect instantiated tp app local folder
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }
        //Public method to store documet
        public void storeDocument(String fileName, String text)
        {
            //call to private method to store document with arguments filename and text content
            CreateFile(fileName, text);
        }
        //Public delete document method
        public void DeleteDocument(String fileName)
        {
            Delete(fileName);
        }
        //Public deleteall method
        public void DeleteAllStorage()
        {
            DeleteAll();
        }
        //Private method to delete all local storage contents
        private async void DeleteAll()
        {
            //List of files at local storage
            IReadOnlyList<StorageFile> files = await localFolder.GetFilesAsync();
            //Delete each file in list from local storage
            foreach (StorageFile file in files)
            {
                Delete(file.DisplayName);
            }
        }
        //Delete method to delete single file
        private async void Delete(String fileName)
        {
            //Open file from local storage file for deletion
            StorageFile deleteFile = await localFolder.GetFileAsync(fileName+".txt");
            //If file not null, opened succesfully
            if (deleteFile != null)
                //Delete file
                await deleteFile.DeleteAsync();
        }
        //Create file at local storage
        private async void CreateFile(String fileName, String text)
        {
            StorageFile newFile = await localFolder.CreateFileAsync(fileName+".txt", CreationCollisionOption.GenerateUniqueName);
            await Windows.Storage.FileIO.WriteTextAsync(newFile, text);
        }
        //Import file passed as argument to this method
        public async void ImportFile(StorageFile newFile)
        {
            //Copying file to local storage, if file name exists - create unique filename
            await newFile.CopyAsync(localFolder, newFile.Name, NameCollisionOption.GenerateUniqueName);         
        }
            
    }
}
