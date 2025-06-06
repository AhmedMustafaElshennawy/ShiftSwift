using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.Media;

namespace ShiftSwift.Application.services.MediaService;

public interface IMediaService
{
    Task<string?> SaveAsync(MediaFileDto file, MediaTypes mediaTypes);
    void Delete(string fileName, MediaTypes mediaType);
    Task DeleteAsync(string fileName, MediaTypes mediaType);
    Task<string?> UpdateAsync(MediaFileDto file, MediaTypes mediaType, string oldUrl);
    string? GetUrl(string? fileName, MediaTypes mediaType);
}