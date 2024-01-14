using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SimpleUrlList.Api.Authentication;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Shared;
using SimpleUrlList.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.AuthOptionsSectionName))
    .ValidateDataAnnotations();
builder.Services.AddHealthChecks();
var sqlOptions = builder.Configuration.GetSection(SettingsNameHelper.DataOptionsSectionName).Get<DataOptions>();
var sqlConnectionString = sqlOptions!.ConnectionString;
builder.Services.AddScoped<IUserService, SulUserService>(_ => new SulUserService(sqlConnectionString));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(_ => new CategoryRepository(sqlConnectionString));
builder.Services.AddScoped<ILinkGroupRepository, LinkGroupRepository>(_ => new LinkGroupRepository(sqlConnectionString));
builder.Services.AddScoped<ILinkRepository, LinkRepository>(_ => new LinkRepository(sqlConnectionString));
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(conf =>
{
    conf.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Api key to access the CA api's",
        Type = SecuritySchemeType.ApiKey,
        Name = AuthOptions.ApiKeyHeaderName,
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };
    conf.AddSecurityRequirement(requirement);
});

builder.Services.AddScoped<ApiKeyAuthFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/" + ConstantRouteHelper.HealthRoute, new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous();
    endpoints.MapControllers();
});

app.Run();
