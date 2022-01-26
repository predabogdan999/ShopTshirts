using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTshirts;
using ShopTshirts.Models;

namespace ShopTshirts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        [Route("list")]
        public IEnumerable<Products> GetProducts()
        {
            return _context.Products;
        }

        [HttpGet]
        [Route("listbycategory/{id}")]
        public List<Products> GetAllProductsByCategory([FromRoute]int Id)
        {

                List<Products> products = _context.Products.Where(x => x.Categories.Id == Id).ToList();
                return products;
            

        }

        // GET: api/Products/5
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> GetProducts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // PUT: api/Products/5
        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> PutProducts([FromRoute] int id, [FromBody] ProductModel products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _context.Products.FirstOrDefault(p => p.productId == id);

            product = products.ToEntity(product);
            var category = _context.Categories.FirstOrDefault(c => c.Id == products.categoryId);
            product.Categories = category;


            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        [Route("CreateRecord")]
        public async Task<IActionResult> PostProducts([FromBody] ProductModel products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Products();
            product = products.ToEntity(product);
            var category = _context.Categories.FirstOrDefault(c => c.Id == products.categoryId);
            product.Categories = category;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Products/5
        [HttpPost]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProducts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.productId == id);
        }
    }
}