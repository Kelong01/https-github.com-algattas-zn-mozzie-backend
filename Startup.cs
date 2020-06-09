using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MozzieAiSystems.Configs;
using MozzieAiSystems.JWT;
using MozzieAiSystems.Models;
using AutoMapper;
using log4net;
using log4net.Config;
using log4net.Repository;
using MozzieAiSystems.Azure;
using MozzieAiSystems.Utility;

namespace MozzieAiSystems
{
    public class Startup
    {
        public static ILoggerRepository Repository { get; set; }
        //private const string SecretKey = "";
        //private readonly  SymmetricSecurityKey _signingKey = ;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("NETCoreRepository");
            //指定xml配置
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection("JwtSetting");
            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtAppSettingOptions["SecurityKey"]));

            services.AddScoped<IAzureBlobStorage>(factory =>
            {
                return new AzureBlobStorage(new AzureBlobSetings(

                    storageAccount: Configuration["Blob_StorageAccount"],

                    storageKey: Configuration["Blob_StorageKey"],

                    containerName: Configuration["Blob_ContainerName"],
                    Configuration["Blob_SASToken"]
                ));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddControllers(options =>
            //    options.Filters.Add(new HttpResponseExceptionFilter()));

            //添加cors 服务 配置跨域处理            
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();//指定处理cookie
                });
            });
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.ValidFor = TimeSpan.FromMinutes(int.Parse(jwtAppSettingOptions["ExpireMinutes"]));
                    options.SigningCredentials = new SigningCredentials(signingKey,
                        SecurityAlgorithms.HmacSha256);
                });

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                configureOptions.SaveToken = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mozzie Api", Version = "v1"});
                c.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, "MozzieAiSystems.xml"));

                var security = new Dictionary<string, IEnumerable<string>> {{"Bearer", new string[] { }}};
                c.AddSecurityRequirement(new OpenApiSecurityRequirement());
                c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    Description = "Format:Bearer {auth_token}",
                    Name="Authorization",
                    In = ParameterLocation.Header
                });
            });

            services.AddDbContext<MozzieContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MozzieDatabase"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //配置Cors
            app.UseCors("any");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mozzie API V1");
            });
        }
    }
}
