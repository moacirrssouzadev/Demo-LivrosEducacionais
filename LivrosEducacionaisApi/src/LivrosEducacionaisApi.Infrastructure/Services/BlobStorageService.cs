using LivrosEducacionaisApi.Application.Interfaces;
using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Serilog;

namespace LivrosEducacionaisApi.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly BlobContainerClient _containerClient;

    private static string SanitizeContainerName(string name)
    {
        name = name.ToLowerInvariant();
        name = Regex.Replace(name, "[^a-z0-9-]", string.Empty);
        name = Regex.Replace(name, "-+", "-");
        name = name.Trim('-');
        
        if (name.Length < 3)
            name = name.PadRight(3, '0');
        if (name.Length > 63)
            name = name.Substring(0, 63).Trim('-');
        
        return name;
    }

    public BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = SanitizeContainerName(containerName);
        _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        Log.Information("BlobStorageService initialized with container: {ContainerName}", _containerName);
    }

    public async Task<string> UploadFileAsync(string fileName, byte[] fileData, string contentType, CancellationToken cancellationToken)
    {
        Log.Information("Starting file upload: {FileName} (Size: {FileSize} bytes)", fileName, fileData.Length);
        
        try
        {
            var blobName = $"{Guid.NewGuid()}_{fileName}";
            Log.Debug("Generated blob name: {BlobName}", blobName);
            
            var blobClient = _containerClient.GetBlobClient(blobName);
            
            using var stream = new MemoryStream(fileData);
            var headers = new BlobHttpHeaders
            {
                ContentType = contentType
            };
            
            Log.Debug("Uploading blob to Azure...");
            await blobClient.UploadAsync(stream, headers, cancellationToken: cancellationToken);
            
            var blobUri = blobClient.Uri.ToString();
            Log.Information("File uploaded successfully: {BlobUri}", blobUri);
            
            return blobUri;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error uploading file to Azure Blob Storage");
            throw;
        }
    }

    public async Task DeleteFileAsync(string blobUrl, CancellationToken cancellationToken)
    {
        Log.Information("Starting file deletion: {BlobUrl}", blobUrl);
        
        try
        {
            var uri = new Uri(blobUrl);
            var blobName = uri.Segments.Last();
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            Log.Information("File deleted successfully: {BlobUrl}", blobUrl);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting file from Azure Blob Storage");
        }
    }
}
