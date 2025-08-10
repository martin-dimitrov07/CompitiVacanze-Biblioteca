var builder = WebApplication.CreateBuilder(args);

// Add services MVC
builder.Services.AddControllersWithViews();

//Cookie authentication for login
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";        // dove mandare se non loggato
        options.LogoutPath = "/Account/Logout";      // opzionale
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // durata sessione
        options.SlidingExpiration = true;            // prolunga la sessione se attivo
    });

var app = builder.Build();

//if (!app.Environment.IsDevelopment()) //controlla se stai eseguendo il progetto in modalità sviluppo (Development) o produzione (Production)
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts(); //abilita l’HTTP Strict Transport Security (dice al browser di usare sempre HTTPS).
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Abilita l'autenticazione con i cookie
app.UseAuthorization();

//app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();