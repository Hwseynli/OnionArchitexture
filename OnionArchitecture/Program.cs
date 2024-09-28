using OnionArchitecture.Application;
using OnionArchitecture.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnionArchitecture.Persistence;
using OnionArchitecture.Infrastructure.Settings;
using OnionArchitecture.Infrastructure.SeedDatas;
using OnionArchitecture.Persistence.Context;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddPersistenceRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
//var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//var jsonFilePath = $"appsettings.{environment}.json";

var environment = builder.Environment;
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("content-disposition", "Content-Disposition"));
});

var jwtSettings = configuration.GetSection("JWTSettings");
var secretKey = jwtSettings["Secret"];

builder.Services.AddMvc(options =>
{
    options.MaxModelBindingCollectionSize = int.MaxValue;

});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AddServerHeader = false;
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();
//Logger

var serilogLogger = new LoggerConfiguration()
    .WriteTo.File(configuration["Serilog:WriteTo:1:Args:path"],
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();
builder.Host.UseSerilog(serilogLogger);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
builder.Services.AddScoped<TestSeedDbContext>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnionArchitecture.API V1");
        c.DocumentTitle = "OnionArchitecture API";
        c.DocExpansion(DocExpansion.List);
    });

//logger
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

////For seedDatas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TestDbContext>();
    var logger = services.GetRequiredService<ILogger<TestSeedDbContext>>();
    var seed = services.GetRequiredService<TestSeedDbContext>();
    await seed.SeedAsync(context, logger);
}
if (environment.IsStaging() && environment.IsProduction())
{
    app.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(configuration["FileSettings:Path"]),
        RequestPath = new PathString("/uploads")
    });

}

app.Run();
