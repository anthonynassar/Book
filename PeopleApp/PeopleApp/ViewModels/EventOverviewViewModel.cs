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
using PeopleApp.Models;
using PeopleApp.Abstractions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;

namespace PeopleApp.ViewModels
{
    public class EventOverviewViewModel : Abstractions.BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();

        public ObservableRangeCollection<Photo> Items { get; set; }
        private List<Models.Object> ObjectList { get; set; }
        public SharingSpace SharingSpace { get; set; }
        public string AlbumPath { get; set; }
        //public ImageSource ImageSource { get; set; }
        public bool UpDownRunning { get; set; }
        public string ErrorMessage { get; set; }
        public int Count { get; set; }
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        public EventOverviewViewModel(SharingSpace sharingSpace, List<Models.Object> objectList)
        {
            SharingSpace = sharingSpace;
            //ObjectList = objectList;
            Items = new ObservableRangeCollection<Photo>();
            

            TakePhotoCommand = new Command(async () => await TakePhoto());
            RefreshCommand = new Command(async () => await Refresh());

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

            // Execute the refresh command to get photos
            RefreshCommand.Execute(null);
        }

        public ICommand TakePhotoCommand { get; }
        public ICommand RefreshCommand { get; }

        async Task TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await DisplayAlert("No Camera", ":( No camera available.", "OK");
                ErrorMessage = ":( No camera available.";
                return;
            }

            // Unique File Name
            var uniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            //var uniqueFileName = string.Format(@"{0}.jpg", DateTime.Now.Ticks);
            //var uniqueFileName = string.Format(@"{0}.jpg", DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper());

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "PeopleApp",
                    Name = uniqueFileName + ".jpg"
                });

            if (file == null)
                return;

            // public path
            AlbumPath = file.AlbumPath;
            // private path
            string privatePath = file.Path;

            string filename = Path.GetFileNameWithoutExtension(AlbumPath);

            // PhotoAlbumPath is already saved on app launch (platform specific code)
            if (!AlbumPath.Equals(Settings.PhotoAlbumPath))
            {
                Settings.PhotoAlbumPath = Path.GetDirectoryName(AlbumPath);
            }

            //ImageSource = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});

            //PhotoModel newPhoto = new PhotoModel();
            var item = new Photo()
            {
                ImageUrl = AlbumPath,
                FileName = filename
            };
            Items.Add(item);
            string imageRemotePath;

            try
            {
                // upload item to cloud
                imageRemotePath = await UploadToCloud(uniqueFileName + ".jpg");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: " + ex);
                await Application.Current.MainPage.DisplayAlert("Image upload failed", ex.Message, "OK");
                return;
            }
            

            try
            {
                // create the object (photo) to upload 
                //var objectId = Utilities.NewGuid();
                Models.Object obj = new Models.Object
                {
                    Id = uniqueFileName,
                    CreationLocation = "",
                    CreationDate = DateTime.Now,
                    UserId = Settings.UserId,
                    SharingSpaceId = SharingSpace.Id,
                    Type = "photo",
                    Uri = imageRemotePath
                    // storedLocally = "true",
                    // storedRemotely = "false",
                    // localPath = "the local path",

                };

                //await _apiServices.PostObjectAsync(obj);
                //await CloudService.AddObject(obj);
                var table = await CloudService.GetTableAsync<Models.Object>();
                await table.CreateItemAsync(obj);

                await CloudService.SyncOfflineCacheAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :" + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Image object post failed", ex.Message, "OK");
                return;
            }

            // extract metadata from single photo
            List<PhotoModel> photoInfo = MetaExtractor.ExtractMetadataPerPhoto(AlbumPath);

            try
            {
                // post in attribute table
                var datatypeList = await _apiServices.GetDatatypesAsync(Settings.CurrentSharingSpace);
                // datatype = { datatypeId, label }
                foreach (var datatype in datatypeList)
                {
                    string value = "";
                    if (datatype.Label.Equals("Time"))
                    {
                        value = photoInfo.FirstOrDefault().map["date"];
                    }
                    else if (datatype.Label.Equals("Location"))
                    {
                        value = photoInfo.FirstOrDefault().map["lat"] + ", " + photoInfo.FirstOrDefault().map["lng"];
                    }
                    else if (datatype.Label.Equals("Social"))
                    {
                        value = photoInfo.FirstOrDefault().map["owner"];
                    }
                    else if (datatype.Label.Equals("Topic"))
                    {
                        value = "keywords";
                    }

                    await _apiServices.PostAttributeAsync(new Models.Attribute { ObjectId = uniqueFileName, Value = value, DatatypeId = datatype.DatatypeId });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :" + ex.Message);
            }
            // useless no use for syncing here
            //finally
            //{
            //    await CloudService.SyncOfflineCacheAsync();
            //}

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

        private async Task<string> UploadToCloud(string filename)
        {
            // Parse the connection string and return a reference to the storage account.
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.StorageConnection);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(Settings.UserId.Replace("|", ""));
            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            // Set permissions for public access
            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Get the user directory within the container
            var directory = container.GetDirectoryReference(Settings.CurrentSharingSpace);
            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(filename);

            byte[] img;
            using (var fileStream = System.IO.File.OpenRead(AlbumPath))
            {
                using (BinaryReader br = new BinaryReader(fileStream))
                {
                    img = br.ReadBytes((int)fileStream.Length);
                }
            }

            // Resize image (do not forget to add the iOS version of it)
            byte[] resizedImageArray = ImageResizer.ResizeImageAndroid(img, 720, 486);
            Stream resizedImage = new MemoryStream(resizedImageArray);
            await blockBlob.UploadFromStreamAsync(resizedImage);

            // Create or overwrite the "xxxx" blob with contents from a local file.
            //using (var fileStream = System.IO.File.OpenRead(AlbumPath))
            //{
            //    await blockBlob.UploadFromStreamAsync(fileStream);
            //}

            // Set the content type of the current blob to image/jpeg
            blockBlob.Properties.ContentType = "image/jpeg";
            await blockBlob.SetPropertiesAsync();

            return blockBlob.Uri.ToString();
        }

        async Task Refresh()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                ObjectList = await _apiServices.GetObjectsBySharingSpace(Settings.CurrentSharingSpace);

                var list = new ObservableRangeCollection<Photo>();
                bool quest = Directory.Exists(Settings.PhotoAlbumPath);

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
                    List<string> fileEntries = new List<string>();
                    //List<string> fileEntries = Directory.GetFiles(Settings.PhotoAlbumPath)
                    //                                .Where<string>(t => Path.GetFileNameWithoutExtension(t).StartsWith("001"))
                    //                                .ToArray<string>()
                    //                                .ToList<string>();

                    // get images by sharingspace
                    // write a simple http get request
                    // var objectList = _apiServices.GetObjectsBySharingSpace(SharingSpace.Id);
                    //int cnt = 0;
                    foreach (var item in ObjectList)
                    {
                        string path = Settings.PhotoAlbumPath + "/" + item.Id + ".jpg";
                        // test if the image exists locally
                        if (File.Exists(path))
                            fileEntries.Add(path);
                        else
                            fileEntries.Add(item.Uri); //fileEntries.Add("https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg"); 

                    }

                    int photoIndex = 0;
                    Count = fileEntries.Count;
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
                    //Items = list;
                    if(Items.Count != list.Count)
                        Items.ReplaceRange(list);
                    else
                        await Application.Current.MainPage.DisplayAlert("Sync completed", "No new photos have been added to this event by other participants", "OK");

                    //Debug.WriteLine("Processed file '{0}'.", fileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error loading items: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Refresh problem", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
