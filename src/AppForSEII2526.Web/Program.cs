using AppForSEII2526.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.Web.Components;
using AppForSEII2526.Web.Components.Account;
using AppForSEII2526.Web.Data;
using AppForSEII2526.Web.API;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
//builder.Services.AddScoped<OfertaStateContainer>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<AppForSEII2526.Web.Data.ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<AppForSEII2526.Web.Data.ApplicationUser>, IdentityNoOpEmailSender>();

string? URI2API = builder.Configuration.GetValue(typeof(string), "AppForSEII2526_API") as string;
//Descomentar cuando tengamos AZULE funcionando ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//the environment variable is defined in Portal Azure
builder.Services.AddScoped<AppForSEII2526APIClient>(sp => new AppForSEII2526APIClient(URI2API, new HttpClient()));

builder.Services.AddScoped<AlquilerStateContainer>();
//SAELICES abajo
builder.Services.AddScoped<ReparacionStateContainer>();

//JAVITRON Compra
builder.Services.AddScoped<CompraStateContainer>();

//Telmo Oferta
builder.Services.AddScoped<OfertaStateContainer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
