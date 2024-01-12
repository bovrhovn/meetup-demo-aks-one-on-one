using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SimpleUrlList.Shared;
using SimpleUrlList.Web.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.AuthOptionsSectionName))
    .ValidateDataAnnotations();
builder.Services.AddOptions<AppOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.AppOptionsSectionName))
    .ValidateDataAnnotations();

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = new PathString("/User/Login"));
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    options.Conventions.AddPageRoute("/Info/Index", ""));

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/" + ConstantRouteHelper.HealthRoute, new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous();
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});
app.Run();