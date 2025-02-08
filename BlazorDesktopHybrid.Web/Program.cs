using BlazorDesktopHybrid.Web;
using BlazorDesktopHybrid.Web.Components;
using BlazorDesktopHybrid;
using BlazorDesktopHybrid.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ILocalStorage, BrowserUserLocalStorage>();
builder.Services.AddSharedServices();
builder.Services.AddAuthenticationWorkaround();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorDesktopHybrid.Components._Imports).Assembly);

app.Run();