using FashionTimes.Shared.Entities;

namespace FashionTimes.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;
        public List<ProductType> ProductTypes { get; set; }
        Task GetProductTypes();
        Task AddProductType(ProductType productType);
        Task UpdateProductType(ProductType productType);

        ProductType CreateNewProductType();
    }
}
