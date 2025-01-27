using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EShop.Application.Storages;
using Microsoft.Extensions.Options;

namespace EShop.Infrastructure.Storages.Cloudinary
{
    public class CloudinaryStorage : Storage, IStorage
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryStorage(IOptions<CloudinaryOptions> options)
        {
            _cloudinary = new(new Account(options.Value.Cloud, options.Value.PublicKey, options.Value.PrivateKey));
            BaseUrl = _cloudinary.Api.UrlImgUp.BuildUrl();
        }

        public string BaseUrl { get; }

        public async Task<FileResult> UploadAsync(string group, FileDetails file)
        {
            string fileName = RenameFile(file.FileName);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, file.Stream),
                AssetFolder = group,
                PublicId = fileName.Split('.')[0],
                UseAssetFolderAsPublicIdPrefix = true
            };

            await _cloudinary.UploadAsync(uploadParams);

            return new(fileName, $"{group}/{fileName}");
        }

        public async Task<IEnumerable<FileResult>> UploadAsync(string group, IEnumerable<FileDetails> files)
        {
            List<FileResult> results = new();

            foreach (var file in files)
                results.Add(await UploadAsync(group, file));

            return results;
        }

        public async Task DeleteAsync(string group, string fileName)
        {
            await _cloudinary.DeleteResourcesAsync(fileName.Split('.')[0]);
        }

        public bool Exists(string group, string fileName)
        {
            var result = _cloudinary.GetResource(fileName.Split('.')[0]);
            return result is not null;
        }
    }
}
