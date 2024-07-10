
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using tts_service.Services;
using tsubasa;
using Microsoft.EntityFrameworkCore;
using tts_service.Db;
using tts_service.Services.Chat;

namespace tts_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Logging.AddConsole();
            // Add services to the container.
            builder.Services.AddDbContext<ChatContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection"), b => b.MigrationsAssembly("tts_service")));
            builder.Services.AddSingleton(new JwtHelper(builder.Configuration));
            builder.Services.AddControllers();
            builder.Services.AddCors(option => option.AddPolicy("cors", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });



            builder.Services.AddSwaggerGen(c =>
            {
                //添加Jwt验证设置,添加请求头信息
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });

                //放置接口Auth授权按钮
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Value Bearer {token}",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, //是否验证Issuer
                    ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
                    ValidateAudience = true, //是否验证Audience
                    ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)), //SecurityKey
                    ValidateLifetime = true, //是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            // }
            app.MapSwagger().RequireAuthorization();
            app.UseCors("cors");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            EnsureStaticFileDirectories();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Defines.StaticFilesPath),
                RequestPath = "/StaticFiles"
            });

            ChatService.Init();

            app.MapControllers();

            app.Run();
        }

        private static void EnsureStaticFileDirectories()
        {
            Logger.ConsoleLog(Defines.StaticFilesPath);
            Logger.ConsoleLog(Defines.StaticImagesPath);
            Logger.ConsoleLog(Defines.StaticVideosPath);
            EnsureDirectory(Defines.StaticFilesPath);
            EnsureDirectory(Defines.StaticImagesPath);
            EnsureDirectory(Defines.StaticVideosPath);
        }

        private static void EnsureDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
