
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
        //        // ��ʾ��������IP��ʹ��443�˿�
        //        options.Listen(IPAddress.Any, 443, o =>
        //        {
        //            o.UseHttps("�����ļ���ַ.pfx", "�����ļ�������");
        //        });
        //    });
        //});
    }
}