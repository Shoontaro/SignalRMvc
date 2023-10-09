using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using PersistentConMvc.Connection;

[assembly: OwinStartup(typeof(PersistentConMvc.Startup))]
namespace PersistentConMvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<ChatConnection>("/chat");
        }
    }
}