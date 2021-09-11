using Backend.Application.Authorization;
using Backend.Application.Implements;
using Backend.Application.Interfaces;
using Backend.Data.DbContext;
using Backend.Data.Entity;
using Backend.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;

namespace Backend
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
            services.AddSignalR();

            //AutoMapper

            services.AddAutoMapper(typeof(Startup));
            //DI
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddCors();

            //Controller
            services.AddControllersWithViews();
            // DB COntexxt

            services.AddDbContext<NewsAppDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("NewsApp")));
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<NewsAppDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(jwt =>
               {
                   var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Key"]);
                   jwt.SaveToken = true;
                   jwt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                       IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                       ValidateIssuer = true,
                       ValidIssuer = Configuration["JwtConfig:Issuer"],
                       ValidAudience = Configuration["JwtConfig:Audience"],
                       ValidateAudience = true,
                       ValidateLifetime = true
                   };
               });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsApp.BackendApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Scheme="oauth2",
                        Name="Bearer",
                        In=ParameterLocation.Header
                    },
                    new List<string>()
                    }
                });
            });

            //Authorization
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<SignalHub>("/signalhub");
            });
        }
    }
}