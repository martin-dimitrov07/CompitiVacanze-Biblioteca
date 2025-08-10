var builder = WebApplication.CreateBuilder(args);

// Add services MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

//if (!app.Environment.IsDevelopment()) //controlla se stai eseguendo il progetto in modalità sviluppo (Development) o produzione (Production)
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts(); //abilita l’HTTP Strict Transport Security (dice al browser di usare sempre HTTPS).
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();