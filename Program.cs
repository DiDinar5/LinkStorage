using Microsoft.EntityFrameworkCore;
using LinkStorage.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
//using System.Web.Configuration;
namespace LinkStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DbLinkStorageContext>(opt =>
    opt.UseNpgsql("Server=localhost;Port=5432;Database=SmartLinkDB;User Id=postgres;Password=12345678"));
            //builder.Services.AddDbContext<DbLinkStorageContext>(opt => opt.UseNpgsql(ConfigurationExtensions.GetConnectionString("DbLinkStorageContext")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
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