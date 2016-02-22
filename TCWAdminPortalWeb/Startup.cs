using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5FullApp.Startup))]
namespace MVC5FullApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}