using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HelloOData
{
    [RoutePrefix("api/customers")]
    public class CustomerWebApiController : ApiController
    {
        [HttpGet]
        [Route]
        public IEnumerable<Customer> GetAllCustomers() =>
            new Customer[]
            {
                new Customer { ID = 1, CustomerName = "Foo", Country = "AT" },
                new Customer { ID = 2, CustomerName = "Bar", Country = "AT" }
            };
    }
}
