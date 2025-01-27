using Microsoft.AspNetCore.Http;

namespace EShop.Application.Storages;

public interface IFormFileStorageService : IStorage
{
    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage,
    /// container (or bucket) for cloud storage.
    /// </param>
    Task<FileResult> UploadAsync(string group, IFormFile file);

    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage, 
    /// container (or bucket) for cloud storage.
    /// </param>
    Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<IFormFile> files);
}
