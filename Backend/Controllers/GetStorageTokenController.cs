using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Security.Claims;
using Backend.Helpers;
using System.Collections.Generic;
using System.Collections;

namespace Backend.Controllers
{
    [Authorize]
    [MobileAppController]
    public class GetStorageTokenController : ApiController
    {
        private const string connString = "CUSTOMCONNSTR_MS_AzureStorageAccountConnectionString";

        public GetStorageTokenController()
        {
            //IDictionary dict = Environment.GetEnvironmentVariables();
            ConnectionString = Environment.GetEnvironmentVariable(connString);
            StorageAccount = CloudStorageAccount.Parse(ConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public string ConnectionString { get; }

        public CloudStorageAccount StorageAccount { get; }

        public CloudBlobClient BlobClient { get; }

        private const string containerName = "userdata";

        [HttpGet]
        //[Route("api/getstoragetoken/UpAccess")]
        public async Task<StorageTokenViewModel> GetUpAccessAsync(string sharingSpaceId, string objectId)
        {
            // The userId is the SID without the sid: prefix
            //var claimsPrincipal = User as ClaimsPrincipal;
            //var userId = claimsPrincipal
            //    .FindFirst(ClaimTypes.NameIdentifier)
            //    .Value.Substring(4);

            string userId = Settings.GetUserId(User).Replace("|","");

            // Errors creating the storage container result in a 500 Internal Server Error
            var container = BlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            // Get the user directory within the container
            var directory = container.GetDirectoryReference(userId);
            var sharingSpaceDirectory = directory.GetDirectoryReference(sharingSpaceId);
            //var blobName = Guid.NewGuid().ToString("N");
            var blobName = objectId;
            var blob = sharingSpaceDirectory.GetBlockBlobReference(blobName + ".jpg");

            // Create a policy for accessing the defined blob
            var blobPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60),
                Permissions = SharedAccessBlobPermissions.Read
                            | SharedAccessBlobPermissions.Write
                            | SharedAccessBlobPermissions.Create
            };

            return new StorageTokenViewModel
            {
                Name = blobName,
                Uri = blob.Uri,
                SasToken = blob.GetSharedAccessSignature(blobPolicy)
            };
        }

        [HttpGet]
        [Route("api/getstoragetoken/DownAccess")]
        public async Task<StorageTokenViewModel> GetDownAccessAsync(string filename)
        {
            string[] array = filename.Split('/');
            var userId = array[0];
            var sharingSpaceId = array[1];
            var objectId = array[2];

            // Errors creating the storage container result in a 500 Internal Server Error
            var container = BlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            // Get the user directory within the container
            var directory = container.GetDirectoryReference(userId);
            var sharingSpaceDirectory = directory.GetDirectoryReference(sharingSpaceId);
            //var blobName = Guid.NewGuid().ToString("N");
            var blobName = objectId;
            var blob = sharingSpaceDirectory.GetBlockBlobReference(blobName);

            // Create a policy for accessing the defined blob
            var blobPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60),
                Permissions = SharedAccessBlobPermissions.Read
            };

            return new StorageTokenViewModel
            {
                Name = blobName,
                Uri = blob.Uri,
                SasToken = blob.GetSharedAccessSignature(blobPolicy)
            };
        }

        [HttpGet]
        [Route("api/getstoragetoken/UpXmlAccess")]
        public async Task<StorageTokenViewModel> GetUpXmlAccessAsync(string sharingSpaceId)
        {
            string userId = Settings.GetUserId(User).Replace("|", "");

            // Errors creating the storage container result in a 500 Internal Server Error
            var container = BlobClient.GetContainerReference("tempdata");
            await container.CreateIfNotExistsAsync();

            // Get the user directory within the container
            var sharingSpaceDirectory = container.GetDirectoryReference(sharingSpaceId);
            var userDirectory = sharingSpaceDirectory.GetDirectoryReference(userId);
            //var blobName = Guid.NewGuid().ToString("N");
            var blobName = "temp.xml";
            var blob = userDirectory.GetBlockBlobReference(blobName);

            // Create a policy for accessing the defined blob
            var blobPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60),
                Permissions = SharedAccessBlobPermissions.Write
                            | SharedAccessBlobPermissions.Create
            };

            return new StorageTokenViewModel
            {
                Name = blobName,
                Uri = blob.Uri,
                SasToken = blob.GetSharedAccessSignature(blobPolicy)
            };
        }
    }

    public class StorageTokenViewModel
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
    }
}
