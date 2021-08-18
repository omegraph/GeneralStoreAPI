using GeneralStoreAPI.Models;
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
    public class CustomerController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();

        // Post (Create customer)
        // api/customer
        [HttpPost]
        public async Task<IHttpActionResult> CreateCustomer([FromBody] Customer model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            // if the model is valid
            if (ModelState.IsValid)
            {
                // Store the model in the database
                _context.Customers.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("The new customer was created!");

            }

            // if the model is not valid, reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Customer
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        //GET BY ID
        // api/Customer/{id}
        [HttpGet]

        public async Task<IHttpActionResult> GetById([FromUri]int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            
            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        // PUT (update)
        // api/Customer/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCustomer([FromUri] int id, [FromBody] Customer updatedCustomer)
        {
            // Check the ids if they match
            if(id != updatedCustomer?.Id)
            {
                return BadRequest("Ids do not match.");
            }

            // Check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the customer in the database
            Customer customer = await _context.Customers.FindAsync(id);

            // If the customer does not exist then do something
            if (customer is null)
                return NotFound();

            // Update the properties
            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;

            // Save the changes
            await _context.SaveChangesAsync();

            return Ok("The customer was updated!");
        }

        // DELETE (delete customer)
        // api/Customer/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCustomer([FromUri] int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if (customer is null)
                return NotFound();

            _context.Customers.Remove(customer);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The customer was deleted.");
            }

            return InternalServerError();
        }
    }
}
