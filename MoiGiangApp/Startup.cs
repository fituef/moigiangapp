using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoiGiangApp.Startup))]
namespace MoiGiangApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
