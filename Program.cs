using Serilog;

namespace NashtechMiddleware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            Log.Logger = new LoggerConfiguration()
               .WriteTo.File("logs/logging.txt")
               .CreateLogger();
            app.UseMiddleware<LoggingMiddleware>();

            app.Run();
        }
    }
}
