using AggregateApi.Application.Implementation;
using AggregateApi.Application.Interfaces;
using Microsoft.OpenApi.Models;
using static System.Net.WebRequestMethods;

namespace AggregateApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();
                builder.Services.AddHttpClient<WeatherClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Apis:Weather:BaseUrl"]);
            });

            builder.Services.Configure<WeatherApiOptions>(
            builder.Configuration.GetSection("Apis:Weather"));

            builder.Services.Configure<NewsApiOptions>(
            builder.Configuration.GetSection("Apis:News"));builder.Services.Configure<WeatherApiOptions>(
            builder.Configuration.GetSection("Apis:Weather"));

            builder.Services.Configure<NewsApiOptions>(
            builder.Configuration.GetSection("Apis:News"));
            builder.Services.AddHttpClient<NewsClient>(client =>
            {
            client.BaseAddress = new Uri(builder.Configuration["Apis:News:BaseUrl"]);
            });

            builder.Services.AddHttpClient<TwitterClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Apis:Twitter:BaseUrl"]);
            });

            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IAggregateService, AggregateService>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                options.EnableAnnotations();  // Enable the Swagger annotations
            });
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
