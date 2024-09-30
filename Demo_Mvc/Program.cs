
namespace Demo_Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://*:5005");
                });
        //.ConfigureWebHost(host =>
        //{
        //    host.UseKestrel(options =>
        //    {
        //        // 表示监听所有IP，使用443端口
        //        options.Listen(IPAddress.Any, 443, o =>
        //        {
        //            o.UseHttps("域名文件地址.pfx", "域名文件的密码");
        //        });
        //    });
        //});
    }
}