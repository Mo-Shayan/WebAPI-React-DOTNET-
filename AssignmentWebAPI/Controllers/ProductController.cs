using AssignmentWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_productContext.Products == null) {
                return NotFound(); }

            return await _productContext.Products.ToListAsync();
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
