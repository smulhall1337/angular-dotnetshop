using System.Runtime.CompilerServices;
using API.Dto.Store;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace API.Controllers.store
{
    // these attributes will now be inherited from BaseApiController
    //[Route("api/v1/[controller]")]  
    //[ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository,
                IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper _mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            this._mapper = _mapper;
        }

        // any api attribute that doesnt specify a route will default to
        // api/v1/Products
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            // var result = await _productRepository.ListAllAsync();
            // for generics, we'll use ListAsync(ISpecification)
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productRepository.CountAsync(countSpec);
            IReadOnlyList<Product> result = await _productRepository.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(result);
            return Ok(new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // tells swagger what this endpoint could potentially return and documents it
        // in this case, we specify the ErrorResponse type. if left out, swagger defaults to the default C# error response type
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            // instead of calling the parameter-less constructor, we'll be calling
            // the constructor that takes a parameter and sets the criteria
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            /*return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            };*/

            if (product == null)
            {
                return NotFound(new ApiException(404));
            }

            // automapper maps our product to productDto automagically
            // .Map<FromType, ToType>(FromTypeVariable);
            return _mapper.Map<Product, ProductDto>(product);
            // autoMapper by default will only map fields that match Exactly
            // in this case, product type and brand will need to be configured.
        }

        // arttributes can specify a route that will be appended to the url of the controller
        // i.e 'api/v1/Products/brands'
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.ListAllAsync());
        }

        // api/v1/Products/types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.ListAllAsync());
        }
    }
}
