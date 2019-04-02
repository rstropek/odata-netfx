using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using ODataFaq.DataModel;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ODataFaq.SelfHostService
{
    //[Authorize]
    [ODataRoutePrefix("Customer")]
	public class CustomerController : ODataController
	{
		private readonly OrderManagementContext context = new OrderManagementContext();

        public CustomerController()
        {
            context.Database.Log = Console.Write;
        }

        //[EnableQuery]
        //public IHttpActionResult Get()
        //{
        //    if (!string.IsNullOrWhiteSpace(((ClaimsPrincipal)Thread.CurrentPrincipal).Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value))
        //    {
        //        return Ok(context.Customers);
        //    }

        //    return Unauthorized();
        //}

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            return context.Customers.Where(c => c.CountryIsoCode == "AT");
        }

        [EnableQuery]
		[ODataRoute("Default.OrderedBike")]
		[HttpGet]
		public IQueryable<Customer> OrderedBike()
		{
			return from c in this.context.Customers
				   where c.Orders.Count(o => o.OrderDetails.Count(od => od.Product.CategoryCode == "BIKE") > 0) > 0
				   select c;
		}

        [EnableQuery]
        [ODataRoute("Default.CustomersInAustria")]
        [HttpGet]
        public IQueryable<Customer> CustomersInAustria()
        {
            return from c in this.context.Customers
                   where c.CountryIsoCode == "AT"
                   select c;
        }

        [HttpPost]
		public async Task<IHttpActionResult> Post([FromBody] Customer customer)
		{
			context.Customers.Add(customer);
			await context.SaveChangesAsync();
			return Created(customer);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.context.Dispose();
				GC.SuppressFinalize(this);
			}
		}
	}
}
