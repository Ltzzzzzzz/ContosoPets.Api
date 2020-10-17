using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using ContosoPets.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace ContosoPets.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(
                // 这里配置Async后缀名不省略，不配置这里会导致CreateAtAction报错
                // 如果是在不配置的话，需要添加[ActionName(nameof(xxxAsync))]别名
                options => options.SuppressAsyncSuffixInActionNames = false)
                .AddNewtonsoftJson(
                /* 
                 * [HttpPatch] 需要Microsoft.AspNetCore.Mvc.NewtonsoftJson来解释json，
                 * 并且需要在AddControllers()后添加.AddNewtonsoftJson()
                 */
                //设置序列化时key为驼峰样式
                setup => setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
                );
            // 配置映射
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // 配置dbcontext
            services.AddDbContext<ContosoPetsContext>(options => options.UseInMemoryDatabase("ContosoPets"));
            // 配置跨域
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Any",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:8080")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            // 配置Swagger
            services.AddSwaggerGen(c =>
            {
                // 修改UI中现实的信息
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ContosoPets.Api",
                    Description = "一个简单的CRUD例子",
                    Contact = new OpenApiContact
                    {
                        Name = "Ltzzzzzzz",
                        Email = "495236549@qq.com"
                    }
                });
                // 配置注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //要在应用的根 (http://localhost:<port>/) 处提供 Swagger UI，请将 RoutePrefix 属性设置为空字符串：
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            // 配置跨域
            app.UseCors("Any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
