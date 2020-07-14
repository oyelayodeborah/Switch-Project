using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BezSwitch.Utilities;
//using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(BezSwitch.Startup))]

namespace BezSwitch
{
    public class Startup
    {
        Common _env = new Common();
        
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
            config
                .EnableSwagger(c => { c.SingleApiVersion("v1", "SwitchAPIServices"); c.IncludeXmlComments(_env.GetSwaggerXMLDirectory()); c.RootUrl(req => _env.GetRootUrl()); })
                .EnableSwaggerUi(c => { c.DisableValidator(); });

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
