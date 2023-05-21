using CRUDTasknWeave.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDTasknWeave.Serveces
{ 
    public interface IProductsService
    {
        Task<IEnumerable<Product>>  GetProducts();
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product);
        void UpdateProduct(Product updatedProduct);
        void DeleteProduct(Product deleteProduct);

    }
}
