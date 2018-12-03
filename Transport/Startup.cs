using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Transport.Startup))]
namespace Transport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
