using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Application.Storages;
using EShop.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.Features.ProductImages.Commands;

public record UploadProductImagesCommand(Guid ProductId, IFormFileCollection Files) : IRequest<UploadProductImagesCommand.Response>
{
    public record Response(IEnumerable<string> Paths);

    internal class Handler : IRequestHandler<UploadProductImagesCommand, Response>
    {
        private readonly IReadRepository<Product, Guid> _productReadRepository;
        private readonly IWriteRepository<ProductImage, Guid> _productImageWriteRepository;
        private readonly IFormFileStorageService _formFileStorageService;

        public Handler(IReadRepository<Product, Guid> productReadRepository,
            IWriteRepository<ProductImage, Guid> productImageWriteRepository,
            IFormFileStorageService formFileStorageService)
        {
            _productReadRepository = productReadRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _formFileStorageService = formFileStorageService;
        }

        public async Task<Response> Handle(UploadProductImagesCommand request, CancellationToken cancellationToken)
        {
            foreach (IFormFile file in request.Files)
                FileRules.FileExtensionMustBeImage(file.FileName);

            Product product = (await _productReadRepository.GetAsync(request.ProductId)).IfNullThrowNotFound();

            var images = await _formFileStorageService.UploadAsync("product-images", request.Files);

            var productImages = images.Select(image => new ProductImage
            {
                FileName = image.FileName,
                Path = image.Path,
                ProductId = product.Id,
                Product = product
            });
            await _productImageWriteRepository.CreateAsync(productImages);

            return new(productImages.Select(image => $"{_formFileStorageService.BaseUrl}/{image.Path}"));
        }
    }
}
