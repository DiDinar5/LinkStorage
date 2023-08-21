using Microsoft.EntityFrameworkCore;
using LinkStorage.DataBase;
using LinkStorage.Repository.IRepository;
using LinkStorage.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using NLog.Web;
using NLog;
namespace LinkStorage
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                //Nlog: Setup Nlog for Dependency Injection 
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddDbContext<DbLinkStorageContext>(opt =>
                {
                    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
                });
                builder.Services.AddScoped<IUserRepository, UserRepository>();//DI.Cервис создаются единожды для каждого запроса.

                var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

                builder.Services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
                builder.Services.AddControllers(options =>
                {

                }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                    "Enter 'Bearer [space] and then your token in the text input below. \r\n\r\n" +
                    "Example: \"Bearer 12345qwerty\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id =  "Bearer"
                                    },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                        new List<string>()
                    }
                });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    options.IncludeXmlComments(xmlPath);//сгенерированный документ передается в сваггер
                });
                var app = builder.Build();


                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();
                app.Run();
            }
            catch(Exception ex)
            {
                logger.Error(ex); 
            }
        }

    }
}