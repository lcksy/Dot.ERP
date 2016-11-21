using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dot.ERP.Startup))]
namespace Dot.ERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
