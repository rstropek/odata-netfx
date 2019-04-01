using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace CustomODataProvider.Provider
{
	public static class ODataConfiguration
	{
		public static void RegisterOData(HttpConfiguration config)
		{
			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter());

			config.MapODataServiceRoute("odata", "odata", GetModel());
            config.Select().Filter().OrderBy().MaxTop(null);
		}

		private static IEdmModel GetModel()
		{
			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<Customer>("Customers");
			return modelBuilder.GetEdmModel();
		}
	}
}
