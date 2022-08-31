var builder = WebApplication.CreateBuilder(args);

// Aadd services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddDbContext<StoreContext>(options =>
{
  var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

  string connStr;

  if (env == "Development")
  {
    // Use connection string from file.
    connStr = builder.Configuration.GetConnectionString("DefaultConnection");
  }
  else
  {
    // Use connection string provided at runtime by Heroku.
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    // Parse connection URL to connection string for Npgsql
    connUrl = connUrl.Replace("postgres://", string.Empty);
    var pgUserPass = connUrl.Split("@")[0];
    var pgHostPortDb = connUrl.Split("@")[1];
    var pgHostPort = pgHostPortDb.Split("/")[0];
    var pgDb = pgHostPortDb.Split("/")[1];
    var pgUser = pgUserPass.Split(":")[0];
    var pgPass = pgUserPass.Split(":")[1];
    var pgHost = pgHostPort.Split(":")[0];
    var pgPort = pgHostPort.Split(":")[1];

    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;Trust Server Certificate=true";
  }

  // Whether the connection string came from the local development configuration file
  // or from the environment variable from Heroku, use it to set up your DbContext.
  options.UseNpgsql(connStr);
});

builder.Services.AddCors();

builder.Services.AddIdentityCore<User>(options =>
{
  options.User.RequireUniqueEmail = true;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<StoreContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"])),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                  };
                });

builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ImageService>();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (builder.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReStore Svc v1"));
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(options =>
{
  options.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
         .WithOrigins("http://localhost:3000");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
  await context.Database.MigrateAsync();
  await DbInitializer.Initialize(context, userManager);
}
catch (Exception ex)
{
  logger.LogError(ex, "Problem migrating data");
}

await app.RunAsync();