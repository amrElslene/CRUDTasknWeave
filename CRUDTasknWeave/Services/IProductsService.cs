using CRUDTasknWeave.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDTasknWeave.Serveces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        Product CreateProduct(Product product);
        void UpdateProduct(Product updatedProduct);
        void DeleteProduct(Product deleteProduct);

    }
}
