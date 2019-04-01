using ODataFaq.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace ODataFaq.SelfHostService
{
    public class CustomerWebApiController : ApiController
	{
		[HttpGet]
		public async Task<IEnumerable<Customer>> Get()
		{
			using (var context = new OrderManagementContext())
			{
				return await context.Customers.ToArrayAsync();
			}
		}

		[HttpGet]
		public async Task<Customer> Get(Guid id)
		{
			using (var context = new OrderManagementContext())
			{
				return await context.Customers
					.SingleOrDefaultAsync(c => c.CustomerId == id);
			}
		}
	}
}
