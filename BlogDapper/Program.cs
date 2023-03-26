using BlogDapper.Repositorio;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//inyeccion de dependencias de los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IArticuloRepositorio, ArticuloRepositorio>();
builder.Services.AddScoped<IComentarioRepositorio, ComentarioRepositorio>();
builder.Services.AddScoped<IEtiquetaRepositorio, EtiquetaRepositorio>();

//para que funcione la autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //tiempo de expiración
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.Cookie.Name = "CookieAutenticacion";
        options.LoginPath = "/Front/Accesos/Acceso";
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Front/Accesos/ErrorAcceso";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Front}/{controller=Inicio}/{action=Index}/{id?}");

app.Run();
