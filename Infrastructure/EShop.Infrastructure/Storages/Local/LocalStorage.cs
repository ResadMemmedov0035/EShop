using EShop.Application.Storages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace EShop.Infrastructure.Storages.Local
{
    public class LocalStorage : Storage, IStorage
    {
        private readonly IWebHostEnvironment _environment;

        public LocalStorage(IWebHostEnvironment environment, IServer server)
        {
            _environment = environment;
            BaseUrl = server.Features.Get<IServerAddressesFeature>()?.Addresses.First() ?? string.Empty;
        }

        public string BaseUrl { get; }

        public async Task<FileResult> UploadAsync(string group, FileDetails file)
        {
            var result = await UploadAsync(group, new[] { file });
            return result.First();
        }

        public async Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<FileDetails> files)
        {
            string directory = GetGroupDirectory(group); // .../wwwroot/{group}

            var tasks = files.Select(async file =>
            {
                string name = RenameFile(file.FileName);
                await CopyFileAsync(file.Stream, Path.Combine(directory, name)); // .../wwwroot/{group}/123234-asdfas.png
                await file.Stream.FlushAsync();
                return new FileResult(name, $"{group}/{name}");
            });
            return await Task.WhenAll(tasks);
        }

        public Task DeleteAsync(string group, string fileName)
        {
            File.Delete(Path.Combine(_environment.WebRootPath, group, fileName));
            return Task.CompletedTask;
        }

        public bool Exists(string group, string fileName)
            => File.Exists(Path.Combine(_environment.WebRootPath, group, fileName));

        private static async Task CopyFileAsync(Stream file, string path)
        {
            using FileStream destination = new(path, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(destination);
        }

        private string GetGroupDirectory(string group)
        {
            string directory = Path.Combine(_environment.WebRootPath, group); 
            Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
