using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProjectTeamThree.Startup))]
namespace HospitalProjectTeamThree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
