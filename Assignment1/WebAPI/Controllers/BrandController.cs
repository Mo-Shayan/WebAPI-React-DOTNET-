using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;

        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<Brand>> GetBrands()
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brands = await _dbContext.Brands.ToListAsync();

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null) 
            {
                return NotFound();
            }
            return Ok(brand);          
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrand), new { id = brand.ID }, brand);
        }

        [HttpPut]
        public async Task<ActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

            private bool BrandAvailable(int id)
            {
                return (_dbContext.Brands?.Any(b => b.ID == id)).GetValueOrDefault();
            }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand =  await _dbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            _dbContext.Brands.Remove(brand);
            await  _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
