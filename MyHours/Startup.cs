using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyHours.Startup))]
namespace MyHours
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
