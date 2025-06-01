using Microsoft.AspNetCore.Http;

namespace ShiftSwift.Application.services.MediaService;

public interface IAzureBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string? folderPath = null);
    Task<Stream> DownloadFileAsync(string fileName);
    Task DeleteFileAsync(string fileName);
}