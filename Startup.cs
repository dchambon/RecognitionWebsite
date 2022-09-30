using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CentricProjectTeam4.Startup))]
namespace CentricProjectTeam4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
