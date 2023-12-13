using AssignmentWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AssignmentWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _productContext;
        public ProductController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string Sorder)
        {
            if (_productContext.Products == null) {
                return NotFound(); }

            IQueryable<Product> query = _productContext.Products;

            switch (Sorder)
            {
                case "asc":
                    query = query.OrderBy(p => p.Price);
                    break;

                case "desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                // Default to ascending if sortOrder is not specified or invalid
                default:
                    query = query.OrderBy(p => p.ProductID);
                    break;
            }
            return await query.ToListAsync();
        }
        
        [HttpGet("SearchProducts/")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string searchTerm)
        {
            if (_productContext.Products == null)
            {
                return NotFound();
            }
            IQueryable<Product> query = _productContext.Products;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Add a condition to filter products based on the search term
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Type.Contains(searchTerm) || p.Color.Contains(searchTerm));
            }
            return Ok(await query.ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            if (_productContext.Products == null)
            {
                return NotFound();
            }
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
            
        } 

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _productContext.Products.Add(product);
            await _productContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if(id != product.ProductID)
            {
                return BadRequest();
            }
            _productContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _productContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if(_productContext.Products == null)
            {
                return NotFound();
            }
            var product = await _productContext.Products.FindAsync(id);
            if(product == null) { return NotFound();}
            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();

            return Ok();
        }



    }
}
