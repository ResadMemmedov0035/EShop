using EShop.Application.Storages;

namespace EShop.Infrastructure.Storages.Azure
{
    // TODO: One day get an Azure account and implement this
    // reference: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
    public class AzureStorage : Storage, IStorage
    {
        public string BaseUrl { get; } = "";

        public Task<FileResult> UploadAsync(string group, FileDetails file)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<FileDetails> files)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string fileName, string relativePath)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string fileName, string relativePath)
        {
            throw new NotImplementedException();
        }
    }
}
