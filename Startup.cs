using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApplication.Context;
using WebApplication.Filters;
using WebApplication.Infrastructure;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication
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
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IUserService, DefaultUserService>();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1"
                , new OpenApiInfo{Title = "Mollaz API", Version = "v1"}); });
//            use in-memory database
            //TODO: SWAP FOR REAL DATABASE
            services.AddDbContext<MollazDbContext>(
                options => options.UseInMemoryDatabase("mollazcouturedb"));
            services.Configure<User>(Configuration.GetSection("userInfo"));
            services.Configure<PagingOptions>(Configuration.GetSection("DefaultPagingOptions"));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorResponse = new ApiError(context.ModelState);
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Static", new CacheProfile()
                {
                    Duration = 86400
                });
                options.Filters.Add<JsonExceptionFilter>();
                options.Filters.Add<RequireHttpsOrCloseAttribute>();
            });
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyApp",
                policy => policy
                    .AllowAnyOrigin());
//                    .withOrigins("https://example.com"));
            });
            services.AddResponseCaching();
            AddIdentityCoreServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOpenApi();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mollaz API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
//                app.UseSwaggerUi3WithApiExplorer(options =>
//                {
//                    options.GeneratorSettings.
//                })
                
            }
            app.UseResponseCaching();
            app.UseCors("AllowMyApp");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        private static void AddIdentityCoreServices(IServiceCollection services)
        {
            var builder = services.AddDefaultIdentity<UserEntity>();
            builder = new IdentityBuilder(builder.UserType
                , typeof(UserRoleEntity), builder.Services);
            
            builder.AddRoles<UserRoleEntity>()
                .AddEntityFrameworkStores<MollazDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<UserEntity>>();

            
        }
    }
}
