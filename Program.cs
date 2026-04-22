using LibraryInventory.Web.Components;
using LibraryInventory.Business; // Business katmaný referansý
using LibraryInventory.Data;     // Data katmaný referansý
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ADIM: BLAZOR BÝLEÞENLERÝNÝ VE ETKÝLEÞÝMLÝ MODU KAYDET
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Butonlarýn çalýþmasý için bu þart!

// 2. ADIM: DATABASE (EF CORE) BAÐLANTISINI YAP
// Ödev kuralý: SQL Server kullanýlmasý zorunludur.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryInventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true"));

// 3. ADIM: BUSINESS LAYER SERVÝSÝNÝ KAYDET (Dependency Injection)
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// 4. ADIM: MIDDLEWARE VE ENDPOINT AYARLARI
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Butonlarýn ve sayfalarýn etkileþimli (Interactive) çalýþmasýný saðlayan ana ayar:
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();