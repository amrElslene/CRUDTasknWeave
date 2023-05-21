using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRUDTasknWeave.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using CRUDTasknWeave.Data;
using CRUDTasknWeave.Serveces;

namespace CRUDTasknWeave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator,Manager")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productsService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product =await _productsService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("product not Valid");
            }
            await _productsService.CreateProduct(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _productsService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;

            _productsService.UpdateProduct(product);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product =await _productsService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _productsService.DeleteProduct(product);

            return NoContent();
        }
    }
}
