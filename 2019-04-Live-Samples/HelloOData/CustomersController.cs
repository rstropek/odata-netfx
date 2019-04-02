using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HelloOData
{
    [Authorize]
    public class CustomersController : ODataController
    {
        //private static IEnumerable<Customer> DemoCustomers = new Customer[]
        //    {
        //        new Customer { ID = 1, CustomerName = "Foo", Country = "AT" },
        //        new Customer { ID = 2, CustomerName = "Bar", Country = "AT" }
        //    };

        private readonly CustomersContext Context;

        public CustomersController()
        {
            Context = new CustomersContext();
            Context.Database.Log = Console.WriteLine;
        }

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            IQueryable<Customer> result = Context.Customers;
            if (!User.IsInRole("Admin"))
            { 
                result = result.Where(c => c.Country == "AT");
            }

            return result;
        }

        [EnableQuery]
        public SingleResult<Customer> Get([FromODataUri]int key)
        {
            var result = Context.Customers.Where(c => c.ID == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Context.Customers.Add(customer);
            await Context.SaveChangesAsync();
            return Created(customer);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Customer> customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await Context.Customers.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            customer.Patch(entity);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Customer update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ID)
            {
                return BadRequest();
            }
            Context.Entry(update).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        private async Task<bool> CustomerExists(int key) =>
            await Context.Customers.AnyAsync(c => c.ID == key);

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await Context.Customers.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            Context.Customers.Remove(product);
            await Context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<Customer> CustomersFromAustria()
        {
            return Context.Customers.Where(c => c.Country == "AT");
        }
    }
}
