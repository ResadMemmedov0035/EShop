using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EShop.Application.Repositories;
using EShop.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EShop.Application;
using EShop.Persistence;
using EShop.Application.Mapping.Manual;
using AutoMapper;
using EShop.Application.Features.Products.Commands;
using EShop.Application.Features.Products.DTOs;

namespace EShop.Benchmark
{
    /*
     * Note: Comment [Benchmark] attributes on the methods' above that you won't need to measure.
     */
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmarks
    {
        IMapperReadRepository<Product, Guid> _readRepository = null!;
        IMapper _mapper = null!;

        const string _exampleProductId = "78abaeab-7c35-47ef-b436-72ba81405989";

        [GlobalSetup]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigurationManager config = new();
            config.AddJsonFile("config.json");

            services.AddApplicationServices();
            services.AddPersistenceServices(config);

            IServiceProvider provider = services.BuildServiceProvider();

            _readRepository = provider.GetService<IMapperReadRepository<Product, Guid>>()!;
            _mapper = provider.GetService<IMapper>()!;
        }

        #region Repository Projection Benchmarks
        /*
         * Manual - 719.3 us, 12.65 KB
         * AutoMapper - 744.9 us, 12.39 KB
         * VeryManual - 822.8 us, 14.56 KB
         */
        [Benchmark]
        public async Task<GetProductByIdQueryResponse?> GetMappedById_VeryManual() 
        {
            return await _readRepository.GetAsync<GetProductByIdResponse>(Guid.Parse(_exampleProductId), p
                => new(p.Id, p.Name, p.Quantity, p.UnitPrice, p.Created, p.LastModified,
                p.Images.Select(i => new ProductImageListItemDTO(i.Id, i.FileName, i.Path))));
        }
        [Benchmark]
        public async Task<GetProductByIdResponse?> GetMappedById_Manual()
        {
            return await _readRepository.GetAsync(Guid.Parse(_exampleProductId), ProductMapper.ProjectToGetByIdResponse);
        }
        [Benchmark]
        public async Task<GetProductByIdResponse?> GetMappedById_AutoMapper()
        {
            return await _readRepository.GetAsync<GetProductByIdResponse>(Guid.Parse(_exampleProductId));
        }
        #endregion

        #region Mapping Benchmarks
        /*
         * Manual mapping 4x faster than AutoMapper
         */
        //[Benchmark]
        public Product CreateCommandMapToProduct_Manual()
        {
            CreateProductCommand command = new("Some name", 10, 50);
            return command.MapToProduct();
        }
        //[Benchmark]
        public Product CreateCommandMapToProduct_AutoMapper()
        {
            CreateProductCommand command = new("Some name", 10, 50);
            return _mapper.Map<Product>(command);
        }
        #endregion

        #region Repository Benchmarks
        /*
         * Apparently, Tracking is faster than NoTracking.
         * In spite of Microsoft Docs saying NoTracking is faster.
         * And of course AutoMapper is slower than normal execution.
         */
        //[Benchmark]
        public async Task<List<Product>> GetList_Tracking()
        {
            return await _readRepository.GetListAsync();
        }
        //[Benchmark]
        public async Task<List<Product>> GetList_NoTracking()
        {
            return await _readRepository.GetListAsync(false);
        }

        //[Benchmark]
        public async Task<Product?> GetById_Tracking()
        {
            return await _readRepository.GetAsync(Guid.Parse(_exampleProductId));
        }
        //[Benchmark]
        public async Task<Product?> GetById_NoTracking()
        {
            return await _readRepository.GetAsync(Guid.Parse(_exampleProductId), false);
        }

        //[Benchmark]
        public async Task<List<GetProductListResponseItem>> GetMappedList_AutoMapper_Tracking()
        {
            return await _readRepository.GetListAsync<GetProductListResponseItem>();
        }
        //[Benchmark]
        public async Task<List<GetProductListResponseItem>> GetMappedList_AutoMapper_NoTracking()
        {
            return await _readRepository.GetListAsync<GetProductListResponseItem>(false);
        }

        //[Benchmark]
        public async Task<GetProductByIdResponse?> GetMappedById_AutoMapper_Tracking()
        {
            return await _readRepository.GetAsync<GetProductByIdResponse>(Guid.Parse(_exampleProductId));
        }
        //[Benchmark]
        public async Task<GetProductByIdResponse?> GetMappedById_AutoMapper_NoTracking()
        {
            return await _readRepository.GetAsync<GetProductByIdResponse>(Guid.Parse(_exampleProductId), false);
        }
        #endregion
    }
}
