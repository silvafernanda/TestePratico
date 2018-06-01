using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjetoAgenda.Startup))]
namespace ProjetoAgenda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
