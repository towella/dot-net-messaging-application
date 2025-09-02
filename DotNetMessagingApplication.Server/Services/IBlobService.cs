namespace DotNetMessagingApplication.Server.Services
{
    public interface IBlobService
    {
        Task<string> UploadImage(IFormFile file);
        Task<bool> DeleteBlob(string blobName);
        Task<Stream?> GetImage(string blobName);
    }
}
