var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a contenedor
builder.Services.AddControllers(); // Agrega soporte para controladores

var app = builder.Build();

// Configurar tubería de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configurar enrutamiento para controladores
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        // Renderiza la página Razor "Index.cshtml" ubicada en /Pages
        await context.Response.WriteAsync("Pages/index.cshtml");
    });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
