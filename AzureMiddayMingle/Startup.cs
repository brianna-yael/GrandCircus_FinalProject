using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureMiddayMingle.Startup))]
namespace AzureMiddayMingle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
