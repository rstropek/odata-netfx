using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
