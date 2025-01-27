using EShop.Application.Storages;
using Microsoft.AspNetCore.Http;

namespace EShop.Infrastructure.Storages
{
    public class FormFileStorageService : IFormFileStorageService
    {
        private readonly IStorage _storage;

        public FormFileStorageService(IStorage storage)
            => _storage = storage;

        public string BaseUrl => _storage.BaseUrl;

        public Task<FileResult> UploadAsync(string group, IFormFile file)
            => _storage.UploadAsync(group, new FileDetails(file.FileName, file.OpenReadStream()));

        public Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<IFormFile> files)
            => _storage.UploadAsync(group, files.Select(file => new FileDetails(file.FileName, file.OpenReadStream())));

        public Task<FileResult> UploadAsync(string group, FileDetails file)
            => _storage.UploadAsync(group, file);

        public Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<FileDetails> files)
            => _storage.UploadAsync(group, files);

        public Task DeleteAsync(string group, string fileName)
            => _storage.DeleteAsync(group, fileName);

        public bool Exists(string group, string fileName)
            => _storage.Exists(group, fileName);
    }
}
