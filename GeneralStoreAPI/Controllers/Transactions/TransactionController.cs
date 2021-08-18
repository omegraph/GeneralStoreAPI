using GeneralStoreAPI.Models;
using GeneralStoreAPI.Models.POCOs.Transactions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        // POST (create a transaction)
        //  api/transaction
        [HttpPost]
        public async Task<IHttpActionResult> PostTransaction([FromBody] Transaction transaction)
        {
            Customer customer = await _context.Customers.FindAsync(transaction.CustomerId);
            Product product = await _context.Products.FindAsync(transaction.ProductSKU);
                        
            // Check if the product is available
            if (product.IsInStock is false)
                return BadRequest("Sorry this item is currently out of stock!");

            // Check if the quantity is enough
            if (product.NumberInInventory < transaction.ItemCount)
            {
                return BadRequest("Not enough quantity for this transaction");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add and Save the transaction
            _context.Transactions.Add(transaction);

            // Remove the Products that were bought
            product.NumberInInventory = product.NumberInInventory - transaction.ItemCount;

            // Save the changes
            await _context.SaveChangesAsync();
            return Ok("Your transaction completed successfully!");
        }

        // GET ALL TRANSACTIONS
        // api/Transaction
        [HttpGet]
        public async Task<IHttpActionResult> GetAllTransactions()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        // GET TRANSACTION BY ID
        // api/Transaction/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactionById([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }
    }
}
