var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(); // Agrega el servicio de sesión.
builder.Services.AddHttpClient(); // Agrega el servicio de HttpClientFactory.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession(); // Habilita el uso de la sesión.

app.MapRazorPages();

app.Run();
