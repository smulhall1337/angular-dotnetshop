using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // give basespecification a parameter-less constructor
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
        : base(x =>
                (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))
                && (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId)
                && (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId)
                )
        {
            // this will create an expression to add these methods into the Iqueryable  that can be passed into a 
            // generic repository method
            AddInclude(X => X.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);

            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;

                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id)
            : base(x => x.Id == id)
        {
            // so in our constructor for BaseSpecification, we have one that takes an expression and sets the criteria
            // we're replacing that generic expression with the expression above and setting the includes to whats below
            AddInclude(X => X.ProductType);
            AddInclude(x => x.ProductBrand);
        }

    }
}
