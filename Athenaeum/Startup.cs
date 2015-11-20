using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Athenaeum.Startup))]
namespace Athenaeum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
