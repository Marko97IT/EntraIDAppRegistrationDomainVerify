using Microsoft.AspNetCore.HttpOverrides;

namespace EntraIDAppWhitelist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthorization();

            var app = builder.Build();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.MapGet("/.well-known/microsoft-identity-association.json", (HttpContext httpContext) =>
            {
                httpContext.Response.ContentType = "application/json";
                return new
                {
                    associatedApplications = new[]
                    {
                        new
                        {
                            applicationId = app.Configuration.GetValue<string>("ApplicationId")
                        }
                    }
                };
            });
            app.Run();
        }
    }
}