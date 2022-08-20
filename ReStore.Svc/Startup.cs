using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReStore.Svc.Data;
using ReStore.Svc.Middleware;

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
      });

      services.AddDbContext<StoreContext>(options =>
      {
        options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });
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

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}