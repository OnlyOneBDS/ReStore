using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReStore.Svc.Data;
using ReStore.Svc.Entities;
using ReStore.Svc.Middleware;
using ReStore.Svc.Services;

namespace ReStore.Svc
{
  public class Startup
  {
    private readonly string AllowAllPolicy = "_allowAllPolicy";

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(name: AllowAllPolicy, policy =>
        {
          policy.WithOrigins("http://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
      });

      services.AddControllers();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReStore Svc", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "JWT Auth Header",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Scheme = "Bearer",
          Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              In = ParameterLocation.Header,
              Name = "Bearer",
              Reference = new OpenApiReference
              {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme,
              },
              Scheme = "oauth2"
            },
            new List<string>()
          }
        });
      });

      services.AddDbContext<StoreContext>(options =>
      {
        options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });

      services.AddIdentityCore<User>(options =>
              {
                options.User.RequireUniqueEmail = true;
              })
              .AddRoles<Role>()
              .AddEntityFrameworkStores<StoreContext>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSettings:TokenKey"])),
                  ValidateAudience = false,
                  ValidateIssuer = false,
                  ValidateIssuerSigningKey = true,
                  ValidateLifetime = true
                };
              });

      services.AddAuthorization();
      services.AddScoped<TokenService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionMiddleware>();

      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReStore Svc v1"));
      }

      //app.UseHttpsRedirection();

      app.UseRouting();

      // app.UseCors(options =>
      // {
      //   options.AllowAnyHeader()
      //          .AllowAnyMethod()
      //          .AllowCredentials()
      //          .WithOrigins("http://localhost:3000");
      // });

      app.UseCors(AllowAllPolicy);
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}