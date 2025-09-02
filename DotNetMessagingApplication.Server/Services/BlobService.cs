using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace DotNetMessagingApplication.Server.Services
{
	public class BlobService : IBlobService
	{
		private readonly BlobContainerClient _blobContainerClient;

		public BlobService(IConfiguration config)
		{
			var connectionString = config["AzureBlob:ConnectionString"];
			var containerName = config["AzureBlob:ContainerName"];
			_blobContainerClient = new BlobContainerClient(connectionString, containerName);
		}

		public async Task<string> UploadImage(IFormFile file)
		{
			var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			var blobClient = _blobContainerClient.GetBlobClient(blobName);

			using var stream = file.OpenReadStream();
			await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

			return blobClient.Uri.ToString();
		}
        public async Task<bool> DeleteBlob(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
        }
        public async Task<Stream?> GetImage(string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return null;

            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }
    }
}