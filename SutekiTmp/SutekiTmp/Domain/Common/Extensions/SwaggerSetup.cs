using Microsoft.OpenApi.Models;

namespace SutekiTmp.Domain.Common.Extensions
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ToDo API1",
                        Description = "An ASP.NET Core Web API for managing ToDo items",
                        TermsOfService = new Uri("https://swagger.io/"),
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = new Uri("https://swagger.io/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = new Uri("https://swagger.io/")
                        }
                    });
                options.SwaggerDoc(
                    name: "v2",
                    info: new OpenApiInfo
                    {
                        Version = "v2",
                        Title = "ToDo API2",
                        Description = "An ASP.NET Core Web API for managing ToDo items",
                        TermsOfService = new Uri("https://swagger.io/"),
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = new Uri("https://swagger.io/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = new Uri("https://swagger.io/")
                        }
                    });
            });
        }
    }
}
