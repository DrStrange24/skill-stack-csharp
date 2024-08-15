using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        Task SaveChangesAsync();
    }
}
