namespace LivrosEducacionaisApi.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string fileName, byte[] fileData, string contentType, CancellationToken cancellationToken);
    Task DeleteFileAsync(string blobUrl, CancellationToken cancellationToken);
}
