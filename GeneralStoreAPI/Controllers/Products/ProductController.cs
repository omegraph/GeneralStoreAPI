using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers.Products
{
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        // POST (create product)
        //  api/Product
        [HttpPost]
        public async Task<IHttpActionResult> PostProduct([FromBody] Product model)
        {
                        
            // If the model is valid
           if(ModelState.IsValid)
            {
                // Store the model in the database
                _context.Products.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("New product was created!");
            }

            // The model is not valid, go ahead and reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Product
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        //GET BY SKU        
        [HttpGet]
        [Route("api/Product/{sku}")]

        public async Task<IHttpActionResult> GetById([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }
    }
}
