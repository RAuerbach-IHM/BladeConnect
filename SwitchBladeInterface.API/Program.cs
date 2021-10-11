using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
//using Microsoft.Owin.Hosting;
//using System.Web.Http;
//using System.Web.Http.SelfHost;

namespace SwitchBladeInterface.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:4226/", "http://10.80.40.37:4226/");
                });
        

    }
}
