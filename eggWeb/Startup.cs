using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eggWeb.Startup))]
namespace eggWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
