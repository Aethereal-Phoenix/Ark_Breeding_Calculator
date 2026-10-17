using Calculator_API.Data;
using Calculator_API.IRepository;
using Calculator_API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace Calculator_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var databaseConnectionString = builder.Configuration["Calculator:DatabaseConnectionString"];

            // Add services to the container

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Calculator Services",
                    Version = "v1"
                });
            });

            // Register EF Core DbContext
            builder.Services.AddDbContext<CalculatorContext>(options => options.UseSqlite(databaseConnectionString));

            // Register DAL implements against their interfaces
            // AddScoped means one instance per HTTP request - correct for database operations
            builder.Services.AddScoped<IDino, DinoDAL>();

            var app = builder.Build();

            // Configure HTTP request pipeline
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
