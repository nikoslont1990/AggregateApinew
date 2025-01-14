using Microsoft.OpenApi.Models;

namespace AggregateApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                options.EnableAnnotations();  // Enable the Swagger annotations
            });
            builder.Services.AddHttpClient();
            var app = builder.Build();
            app.UseSwagger();  // Generate Swagger JSON

            app.UseSwaggerUI(c =>  // Use Swagger UI
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;  // Optional: Set Swagger UI to appear at the root
            });
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
           
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
