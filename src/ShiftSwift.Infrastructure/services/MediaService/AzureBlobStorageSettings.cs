namespace ShiftSwift.Infrastructure.services.MediaService;

public class AzureBlobStorageSettings
{
    public const string SectionName = "AzureBlobStorage";
    public string ConnectionString { get; init; } = null!;
    public string ContainerName { get; init; } = null!;
}