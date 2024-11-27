var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;  // Ensures cookie is sent in requests (even on browsers with cookie restrictions)
        options.LoginPath = "/Account/Login"; // Redirect to this path when authentication is required
        options.LogoutPath = "/Account/Logout"; // Redirect to this path on logout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set session cookie expiration
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});

// Add other services like session, authorization, etc.
builder.Services.AddDistributedMemoryCache();  // To store session in memory
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseSession();         // middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
