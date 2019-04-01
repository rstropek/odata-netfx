using ODataFaq.DataModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ODataFaq.SelfHostService
{
	public class CustomerByCountryWebApiController : ApiController
	{
		[HttpGet]
		public async Task<IEnumerable<Customer>> Get(string countryIsoCode)
		{
			using (var context = new OrderManagementContext())
			{
				return await context.Customers
					.Where(c => c.CountryIsoCode == countryIsoCode)
					.ToArrayAsync();
			}
		}
	}
}
