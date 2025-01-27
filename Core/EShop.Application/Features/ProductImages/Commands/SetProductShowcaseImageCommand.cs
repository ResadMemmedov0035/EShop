using EShop.Application.Repositories;
using EShop.Application.Rules;
using EShop.Domain.Entities;

namespace EShop.Application.Features.ProductImages.Commands;

public record SetProductShowcaseImageCommand(Guid ProductImageId) : IRequest<SetProductShowcaseImageCommand.Response>
{
    public record Response(Guid ImageId, Guid ProductId);

    internal class Handler : IRequestHandler<SetProductShowcaseImageCommand, Response>
    {
        private readonly IReadRepository<ProductImage, Guid> _productImageReadRepository;
        private readonly IWriteRepository<ProductImage, Guid> _productImageWriteRepository;

        public Handler(IReadRepository<ProductImage, Guid> productImageReadRepository,
            IWriteRepository<ProductImage, Guid> productImageWriteRepository)
        {
            _productImageReadRepository = productImageReadRepository;
            _productImageWriteRepository = productImageWriteRepository;
        }

        public async Task<Response> Handle(SetProductShowcaseImageCommand request, CancellationToken cancellationToken)
        {
            // image intended to be showcase
            ProductImage? image = await _productImageReadRepository.GetAsync(request.ProductImageId);
            image = image.IfNullThrowNotFound();

            // current showcase image
            ProductImage? showcaseImage = await _productImageReadRepository.GetAsync(x
                => x.ProductId == image.ProductId && x.IsShowcase);

            if (showcaseImage is not null)
                showcaseImage.IsShowcase = false;

            image.IsShowcase = true;

            await _productImageWriteRepository.SaveAsync();
            return new(image.Id, image.ProductId);
        }
    }
}
