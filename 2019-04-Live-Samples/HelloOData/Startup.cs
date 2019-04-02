using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

[assembly: OwinStartup(typeof(HelloOData.Startup))]

namespace HelloOData
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var identity = new GenericIdentity("Tom");
                Thread.CurrentPrincipal = new GenericPrincipal(identity, new[] { "Admin" });
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = Thread.CurrentPrincipal;
                }

                context.Authentication.User = (ClaimsPrincipal)Thread.CurrentPrincipal;

                await next();
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            RegisterOData(config);
        }

        public void RegisterOData(HttpConfiguration config)
        {
            // 1) EDM bauen
            var builder = new ODataConventionModelBuilder();
            var customersEntitySet = builder.EntitySet<Customer>("Customers");

            customersEntitySet.EntityType.Collection
                .Function("CustomersFromAustria")
                .ReturnsCollectionFromEntitySet<Customer>("Customers");

            // 2) Routing für Controller aufsetzen
            config.MapODataServiceRoute(
                "odata",
                "odata",
                builder.GetEdmModel());

            config.Select().MaxTop(20).OrderBy().Filter();
        }
    }
}
