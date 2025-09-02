namespace DotNetMessagingApplication.Server.Services
{
    public interface IBlobService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteBlobAsync(string blobName);

    }
}
