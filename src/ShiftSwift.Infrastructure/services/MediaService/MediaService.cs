using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ShiftSwift.Application.services.MediaService;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.Media;

namespace ShiftSwift.Infrastructure.services.MediaService;

internal sealed class MediaService(
    IWebHostEnvironment hostingEnvironment,
    IConfiguration configuration) : IMediaService
{
    private readonly string _mediaBasePath = hostingEnvironment.WebRootPath;
    private readonly string _baseUrl = configuration["MediaBaseUrl"]?.TrimEnd('/') ?? string.Empty;
    private string GetMediaFolder(MediaTypes mediaType) => mediaType == MediaTypes.Image ? "images" : "videos";

    public void Delete(string fileName, MediaTypes mediaType)
    {
        var folder = GetMediaFolder(mediaType);
        var fullPath = Path.Combine(_mediaBasePath, folder, Path.GetFileName(fileName));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task DeleteAsync(string fileName, MediaTypes mediaType)
    {
        var folder = GetMediaFolder(mediaType);
        var fullPath = Path.Combine(_mediaBasePath, folder, Path.GetFileName(fileName));

        if (File.Exists(fullPath))
        {
            await Task.Run(() => File.Delete(fullPath));
        }
    }

    public string? GetUrl(string? fileName, MediaTypes mediaType)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        var folder = GetMediaFolder(mediaType);
        return $"{_baseUrl}/{folder}/{fileName.TrimStart('/')}";
    }

    public async Task<string?> SaveAsync(MediaFileDto media, MediaTypes mediaType)
    {
        var folder = GetMediaFolder(mediaType);
        var fullFolderPath = Path.Combine(_mediaBasePath, folder);
        Directory.CreateDirectory(fullFolderPath);

        var fileExtension = Path.GetExtension(media.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(fullFolderPath, fileName);

        await File.WriteAllBytesAsync(fullPath, Convert.FromBase64String(media.Base64));
        return fileName;
    }

    public async Task<string?> UpdateAsync(MediaFileDto media, MediaTypes mediaType, string oldUrl)
    {
        if (media is null)
            return oldUrl;

        if (!string.IsNullOrEmpty(oldUrl))
            Delete(oldUrl, mediaType);

        return await SaveAsync(media, mediaType);
    }
}