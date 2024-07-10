
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
                //���Jwt��֤����,�������ͷ��Ϣ
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

                //���ýӿ�Auth��Ȩ��ť
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Value Bearer {token}",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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
                    ValidateIssuer = true, //�Ƿ���֤Issuer
                    ValidIssuer = configuration["Jwt:Issuer"], //������Issuer
                    ValidateAudience = true, //�Ƿ���֤Audience
                    ValidAudience = configuration["Jwt:Audience"], //������Audience
                    ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)), //SecurityKey
                    ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
                    ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
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
