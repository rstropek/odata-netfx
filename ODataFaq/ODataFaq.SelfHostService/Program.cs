using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security.OAuth;
using ODataFaq.DataModel;
using Owin;
using Simple.OData.Client;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Batch;

[assembly: OwinStartup(typeof(ODataFaq.SelfHostService.Startup))]

namespace ODataFaq.SelfHostService
{
    class Program
	{
		static async Task Main(string[] _)
		{
            const string url = "http://localhost:12345";

            using (WebApp.Start<Startup>(url))
			{
				Console.WriteLine("Listening on port 12345. Press any key to continue.");
				Console.ReadLine();

                //var batch = new ODataBatch(new Uri(url + "/odata/"));
                //batch += c => c
                //    .For<Customer>()
                //    .Set(new Customer() { CustomerId = Guid.NewGuid(), CompanyName = "Kunde 1", CountryIsoCode = "AT" })
                //    .InsertEntryAsync(false);
                //batch += c => c
                //    .For<Customer>()
                //    .Set(new Customer() { CustomerId = Guid.NewGuid(), CompanyName = "Kunde 2", CountryIsoCode = "DE" })
                //    .InsertEntryAsync(false);
                //await batch.ExecuteAsync();
                //Console.ReadLine();
            }
        }
	}

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// Setup routes
			var config = new HttpConfiguration();

			// Removing XML formatter, we just want to support JSON
			config.Formatters.Remove(config.Formatters.XmlFormatter);
			SetupWebApiRoutes(config);
			SetupOdataRoutes(config);

			// Setup simple OAuth2 server for Resource Owner Password Credentials Grant
			SetupOauthServer(app);

			app.UseWebApi(config);
		}

		private static void SetupWebApiRoutes(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "Customer",
				routeTemplate: "api/Customer/{id}",
				defaults: new { controller = "CustomerWebApi", id = RouteParameter.Optional }
			);
			config.Routes.MapHttpRoute(
				name: "CustomerByCountry",
				routeTemplate: "api/CustomerByCountry/{countryIsoCode}",
				defaults: new { controller = "CustomerByCountryWebApi" }
			);
		}

		private static void SetupOdataRoutes(HttpConfiguration config)
		{
			var builder = new ODataConventionModelBuilder();
			var customers = builder.EntitySet<Customer>("Customer");
            var orders = builder.EntitySet<OrderHeader>("Orders");

            // Note global query settings
            // http://odata.github.io/WebApi/#13-01-modelbound-attribute
            config.Select().Count().Filter().OrderBy().Expand().MaxTop(null);

			customers
				.EntityType
				.Collection
				.Function("OrderedBike")
				.ReturnsCollectionFromEntitySet<Customer>("Customer");

            customers
                .EntityType
                .Collection
                .Function("CustomersInAustria")
                .ReturnsCollectionFromEntitySet<Customer>("Customer");

            customers
                .EntityType
                .Function("GetCountry")
                .Returns<string>();

            // Note fluent API instead of model-bound attributes
            // http://odata.github.io/WebApi/#13-01-modelbound-attribute
            builder.EntityType<Customer>()
                .Expand(1, SelectExpandType.Automatic, "Orders");

            var server = new HttpServer(config);
            //config.Routes.MapHttpBatchRoute(
            //    routeName: "batch",
            //    routeTemplate: "odata/batch",
            //    batchHandler: new DefaultHttpBatchHandler(server));

            config.MapODataServiceRoute(
                routeName: "odata",
                routePrefix: "odata",
                model: builder.GetEdmModel(),
                new DefaultODataBatchHandler(server));
        }

        private static void SetupOauthServer(IAppBuilder app)
		{
			// grant_type=password&username=admin&password=admin
			app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
				Provider = new DummyAuthorizationProvider()
			});
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}

		private class DummyAuthorizationProvider : OAuthAuthorizationServerProvider
		{
			public static Task FinishedTask = Task.FromResult(0);

			public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
			{
				// No validation code -> all clients are ok
				context.Validated();
				return FinishedTask;
			}

			public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
			{
				// If username and password are equal, they are ok
				if (context.UserName != context.Password)
				{
					context.Rejected();
					return FinishedTask;
				}

				// Build claims identity
				var identity = new ClaimsIdentity("OAuth2");
				identity.AddClaim(new Claim("User", context.UserName));
				if (context.UserName == "admin")
				{
					identity.AddClaim(new Claim("IsAdmin", "IsAdmin"));
				}

				context.Validated(identity);
				return FinishedTask;
			}
		}
	}
}
