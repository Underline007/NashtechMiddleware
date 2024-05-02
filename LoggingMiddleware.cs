using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NashtechMiddleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Read request bodyd infomation
                var requestBodyContent = await ReadBodyFromRequest(context.Request);
                Log.Information("------HTTP Request Information------" +
                    "\nSchema: {Scheme}" +
                    "\nHost: {Host}" +
                    "\nPath: {Path}" +
                    "\nQueryString: {QueryString}" +
                    "\nRequest Body: {RequestBody}\n",
                context.Request.Scheme, 
                context.Request.Host, 
                context.Request.Path, 
                context.Request.QueryString, 
                requestBodyContent);
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing request");
                throw;
            }
        }
        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
