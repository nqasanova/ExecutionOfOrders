namespace StoreApiIntegration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            app.MapControllerRoute(
                  name: "default",
                  pattern: "{area=exists}/{controller=home}/{action=index}");

            app.Run();
        }
    }
}