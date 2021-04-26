using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webAppV1.Startup))]
namespace webAppV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
