using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using WalletComponent.Common.StorageOptions;

namespace WalletComponent.Providers.Default
{
    public class AzureStorageProvider : IStorageProvider
    {
        CloudBlobClient cloudBlobClient = null;
        public AzureStorageProvider(IOptions<CloudOptions> options)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(options.Value.AzureConnectionString);
            this.cloudBlobClient = account.CreateCloudBlobClient();
        }

        public async Task UploadStreamAsync(Stream stream, string path)
        {
            CloudBlockBlob blob = await GetBlobReferenceAsync(path).ConfigureAwait(false);
            await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);
        }

        private async Task<CloudBlockBlob> GetBlobReferenceAsync(string path)
        {
            string containerName = path.Substring(0, path.IndexOf("/"));
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
            bool IsExists = await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            string blobName = path.Substring(path.IndexOf("/") + 1);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);
            return cloudBlockBlob;
        } 
    }
}
