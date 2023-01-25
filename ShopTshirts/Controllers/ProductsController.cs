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
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net;
using System.Web; 

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
            var result = _context.Products;
            return result;
        }
        [HttpGet]
        [Route("linqlist")]
        public IEnumerable<Products> GetProductsLinq()
        {
            var listitemsrecord = (from product in _context.Products
                                   where product.endDate > DateTime.Now
                                   select product).ToList<Products>();
            return listitemsrecord;
        }
             
        [HttpGet]
        [Route("groupbylist")]
        public IQueryable GetProductsGroupby( )
        {
            /* var groupByCategory =
                                    from product in _context.Products
                                   group product by product.categoryId into newGroup
                                  select newGroup;*/
            var groupByCategory = _context.Products.Where(p => p.endDate > DateTime.Now).GroupBy(p => p.categoryId);
            return groupByCategory;
        }
            
        [HttpGet]
        [Route("joinlist")]
        public IEnumerable<Products> GetProductsJoin()
        {
            var items = (from product in _context.Products
                         join category in _context.Categories on product.categoryId equals category.Id
                         join image in _context.Images on product.productId equals image.productId
                         where product.endDate > DateTime.Now
                         select product).ToList<Products>();
            return items;
        }


        [HttpGet]
        [Route("listbycategory/{id}")]
        public List<Products> GetAllProductsByCategory([FromRoute]int Id)
        {
            List<Products> products = _context.Products.Where(x => x.Categories.Id == Id && (x.endDate > DateTime.Now)).ToList();
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
            

            if ( products == null || products.endDate < DateTime.Now )
            { 
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "this item does not exist" });
            }

            return Ok(products);
        }

        // PUT: api/Products/5
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "EditorPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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
         
        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderNameUrl = "Resources/Images";
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = folderNameUrl + "/" + fileName;
                    
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                       file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                   {
                       return BadRequest();
                   }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet]
        [Route("search")]
        public IActionResult Search( string name)
        {
           
            try
            {
                var result = _context.Products.Where(s =>  s.productName.ToLower().Contains(name.Trim().ToLower()) || s.description.ToLower().Contains(name.Trim().ToLower()));
             
                if (result.Any())
                {
                    return Ok(result);
                }

                return Ok(Enumerable.Empty<Products>());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }
       
        [Route("expiration/{id}")]
        [HttpPost]
        public  IActionResult  AddtoExpiredProducts([FromRoute] int id)
        {
              _context.Products.First(x => x.productId == id).endDate = DateTime.Now.Date.Add(new TimeSpan(-12, 0, 0)); 
              _context.SaveChanges();
            return Ok();
        }
        
       
    }

}