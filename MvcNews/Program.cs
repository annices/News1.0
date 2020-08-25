using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MvcNews
{
    /// <summary>
    /// This class launches the application.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    } // End class.

} // End namespace.
