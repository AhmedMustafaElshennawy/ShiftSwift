using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShiftSwift.Application.services.MediaService;

namespace ShiftSwift.Infrastructure.services.MediaService;

internal sealed class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly AzureBlobStorageSettings _settings;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobStorageService(IOptions<AzureBlobStorageSettings> options, BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
        _settings = options.Value;
        _blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string? folderPath = null)
    {
        // Generate blob name with optional folder path
        var fileName = string.IsNullOrEmpty(folderPath)
            ? file.FileName
            : $"{folderPath}/{file.FileName}";

        // Ensure unique filename
        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var uniqueFileName =
                $"{Path.GetFileNameWithoutExtension(file.FileName)}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            fileName = string.IsNullOrEmpty(folderPath)
                ? uniqueFileName
                : $"{folderPath}/{uniqueFileName}";
            blobClient = _blobContainerClient.GetBlobClient(fileName);
        }

        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"File {fileName} not found in container {_settings.ContainerName}");
        }

        var download = await blobClient.DownloadAsync();
        return download.Value.Content;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}