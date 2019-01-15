using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTC.FSP.Web.Startup))]
namespace MTC.FSP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
