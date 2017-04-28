using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APT.Startup))]
namespace APT
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
