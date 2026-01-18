using Microsoft.EntityFrameworkCore;
using rest_api_labb_minimalapi.Data;

namespace rest_api_labb_minimalapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<RestLabbDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION_STRING")));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();



            app.Run();
        }
    }
}
