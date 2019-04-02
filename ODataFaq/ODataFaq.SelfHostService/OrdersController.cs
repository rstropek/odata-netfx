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
    [ODataRoutePrefix("OrderMgmt/Orders")]
	public class OrdersController : ODataController
	{
		private readonly OrderManagementContext context = new OrderManagementContext();

        public OrdersController()
        {
            context.Database.Log = Console.Write;
        }

        [EnableQuery]
        public IQueryable<OrderHeader> Get()
        {
            return context.Orders;
        }

        [EnableQuery]
        public SingleResult<OrderHeader> Get(Guid key)
        {
            return SingleResult<OrderHeader>.Create(context.Orders.Where(c => c.OrderId== key));
        }

        [EnableQuery]
        //[ODataRoute("Orders({key})/Customer")]
        public SingleResult<Customer> GetCustomer([FromODataUri] Guid key)
        {
            return SingleResult<Customer>.Create(
                context.Orders.Where(c => c.OrderId == key).Select(o => o.Customer));
        }
    }
}
