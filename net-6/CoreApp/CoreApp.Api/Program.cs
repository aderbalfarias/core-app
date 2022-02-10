using CoreApp.Api.Extensions;
using CoreApp.Api.Middlewares;
using CoreApp.Api.Options.Authorization;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
var corsOrigin = builder.Configuration.GetSection("CorsOrigin").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.Services();
builder.Services.Repositories();
builder.Services.Databases(builder.Configuration.GetConnectionString("DemoConnection"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

var authenticationOption = builder.Configuration
    .GetSection(nameof(ApplicationOptions.Authentication))
    .Get<AuthenticationOptions>();

builder.Services.AddSingleton(authenticationOption);

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DemoConnection"));

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
builder.Services.AddSwagger(authenticationOption);

var oidc = builder.Configuration
    .GetSection(nameof(ApplicationOptions.OidcAuthorizationServer))
    .Get<OidcAuthorizationServerOptions>();

builder.Services.AddSingleton(oidc);
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(new string[] { JwtBearerDefaults.AuthenticationScheme, "ADFS" })
        .Build();
});

//builder.Services.AddBearers(Environment, oidc, authenticationOption, new string[] { JwtBearerDefaults.AuthenticationScheme, "ADFS" });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Server", "DENY");

    await next();
});

app.UseAuthentication();

app.Logger.LogInformation($"In {app.Environment.EnvironmentName} environment");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("../swagger/v3/swagger.json", "Core App .NET v6");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMiddleware<ExceptionMiddleware>();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();

    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();
app.UseCors();
app.UseHealthChecks();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();