using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PeopleApp.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PeopleApp.Models.ViewsRelated;
using PeopleApp.Core;
using PeopleApp.Services;

namespace PeopleApp.ViewModels
{
    public class EventOverviewViewModel
    {
        ApiServices _apiServices = new ApiServices();

        public ObservableCollection<Photo> Items { get; set; }

        public string SharingSpaceId { get; set; }

        public string AlbumPath { get; set; }

        public ImageSource ImageSource { get; set; }

        public bool UpDownRunning { get; set; }

        public string ErrorMessage { get; set; }

        public int Count { get; set; }

        public ICommand TakePhotoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        //await DisplayAlert("No Camera", ":( No camera available.", "OK");
                        ErrorMessage = ":( No camera available.";
                        return;
                    }

                    // Unique File Name
                    //var myUniqueFileName = string.Format(@"{0}.jpg", Guid.NewGuid());
                    var myUniqueFileName = string.Format(@"{0}.jpg", DateTime.Now.Ticks);
                    //var myUniqueFileName = string.Format(@"{0}.jpg", DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper());

                    var file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Directory = "PeopleApp",
                            Name = myUniqueFileName
                        });

                    if (file == null)
                        return;

                    // public path
                    AlbumPath = file.AlbumPath;
                    // private path
                    string privatePath = file.Path;

                    string filename = Path.GetFileNameWithoutExtension(AlbumPath);

                    if (!AlbumPath.Equals(Settings.PhotoAlbumPath))
                    {
                        Settings.PhotoAlbumPath = Path.GetDirectoryName(AlbumPath);
                    }

                    ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });

                    //PhotoModel newPhoto = new PhotoModel();
                    var item = new Photo()
                    {
                        ImageUrl = AlbumPath,
                        FileName = filename
                    };
                    Items.Add(item);

                    // create the object (photo) to upload 
                    Models.Object obj = new Models.Object
                    {
                        CreationLocation = "",
                        CreationDate = DateTime.Now,
                        UserId = Settings.UserId,
                        SharingSpaceId = SharingSpaceId,
                        Type = "photo"
                        // storedLocally = "true",
                        // storedRemotely = "false",
                        // localPath = "the local path",
                        // remotePath = ""
                    };
                    await _apiServices.PostObjectAsync(obj); // post this at the end with attriutes
                    // write a function that extracts metadata from a photo (async) and then save it in the table of attributes
                    List<PhotoModel> photoInfo = MetaExtractor.ExtractMetadataPerPhoto(AlbumPath);

                    // time
                    // get id of time dimension apiservices and then datatype

                    var date = photoInfo.FirstOrDefault().map["date"];
                    // post in attribute table
                    // location
                    // get id of location dimension apiservices and then datatype
                    var lat = photoInfo.FirstOrDefault().map["lat"];
                    var lng = photoInfo.FirstOrDefault().map["lng"];
                    // post in attribute table
                    //social
                    //var owner = photoInfo.FirstOrDefault().map["owner"];

                });
            }
        }

        public ICommand ProcessCommand
        {
            get
            {
                return new Command(() =>
                {
                    //activityIndicator.IsRunning = true;
                    UpDownRunning = true;
                    //string path = Path.GetDirectoryName(PathLabel.Text);
                    //string path = Path.GetDirectoryName(AlbumPath);
                    string path = Settings.PhotoAlbumPath;
                    if (path == null)
                    {
                        Debug.WriteLine("Path is null");
                        return;
                    }
                    MetaExtractor me = new MetaExtractor(path);
                    string xmlData = me.Extract();
                    Debug.WriteLine(xmlData);

                    // temporarly turn off this functionality and maybe change its place to after taking a photo
                    //await UploadToCloud();

                    UpDownRunning = false;
                });
            }
        }

        //public ICommand ItemTappedCommand
        //{
        //    get
        //    {
        //        return new Command( () =>
        //        {
        //            Debug.WriteLine("I am being tapped!!");

        //        });
        //    }
        //}

        private async Task UploadToCloud()
        {
            // Parse the connection string and return a reference to the storage account.
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.StorageConnection);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("user1");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Set permissions for public access
            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myphoto2.jpg");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(AlbumPath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

        public EventOverviewViewModel()
        {
            Items = new ObservableCollection<Photo>();
            var list = new ObservableCollection<Photo>();

            SharingSpaceId = Settings.CurrentSharingSpace;

            //// get photos from directory
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var documents2Directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            documentsDirectory += "/PeopleApp/";

            //string[] files = System.IO.Directory.GetFiles(documentsDirectory, "*.jp*g");
            //foreach (var item in files)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //if (files.Length == 0)
            //{
            //    Console.Error.WriteLine("No matching files found.");
            //    Environment.Exit(1);
            //}

            bool quest = Directory.Exists(Settings.PhotoAlbumPath);


            //bool isEmpty = !Directory.EnumerateFiles(Settings.PhotoAlbumPath).Any();

            if (!quest)
            {
                string[] images = {
                "https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
                "https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
                "https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
                "https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
                "https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
                "https://farm2.staticflickr.com/1227/1116750115_b66dc3830e.jpg",
                "https://farm2.staticflickr.com/1227/1116750115_b66dc3830e.jpg",
                "https://farm1.staticflickr.com/44/117598011_250aa8ffb1.jpg",
                "https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
                "https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg",
            };

                int number = 0;
                for (int n = 0; n < 2; n++)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        number++;
                        var item = new Photo()
                        {
                            ImageUrl = images[i],
                            FileName = string.Format("image_{0}.jpg", number),
                        };

                        list.Add(item);
                    }
                }

                Items = list;
            }
            else
            {
                string[] fileEntries = Directory.GetFiles(Settings.PhotoAlbumPath);
                int photoIndex = 0;
                Count = fileEntries.Length;
                foreach (string fileName in fileEntries)
                {
                    photoIndex++;
                    var item = new Photo()
                    {
                        ImageUrl = fileName,
                        FileName = Path.GetFileName(fileName),
                        Index = photoIndex
                    };

                    list.Add(item);
                }
                Items = list;
                //Debug.WriteLine("Processed file '{0}'.", fileName);
            }
        }
    }
}
