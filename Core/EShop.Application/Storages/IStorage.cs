namespace EShop.Application.Storages;

public interface IStorage
{
    /* Example:
     * var result = storage.UploadAsync("product-images", file.FileName, file.Stream); // file.FileName => "image.png"
     * result.FileName => "20230417234539-c21459efd42a78gf.png"
     * result.Path => "product-images/20230417234539-c21459efd42a78gf.png"
     */

    public string BaseUrl { get; }

    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage, 
    /// container (or bucket) for cloud storage.
    /// </param>
    Task<FileResult> UploadAsync(string group, FileDetails file);

    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage, 
    /// container (or bucket) for cloud storage.
    /// </param>
    Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<FileDetails> files);

    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage, 
    /// container (or bucket) for cloud storage.
    /// </param>
    Task DeleteAsync(string group, string fileName);

    /// <param name="group">
    /// Destination group for the uploaded file. 
    /// Equivalent to folder directory for local storage, 
    /// container (or bucket) for cloud storage.
    /// </param>
    bool Exists(string group, string fileName);
}
