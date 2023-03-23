using Blazored.Toast;
using StackExchange.Redis;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();

builder.Services
    .AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(builder.Configuration["Redis"]));
builder.Services.AddBlazoredToast();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("smps.common.api", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["CommmonApi"] ?? "http://smps.common.api");
    });
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IManagementService, ManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
