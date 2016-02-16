using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HealthcareAnalytics.Startup))]
namespace HealthcareAnalytics
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
