using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Domain.Common.Authentication.Session;
using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Domain.Repository.Repository;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Domain.Service.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SessionCookie"; //Cookie Name
    options.Cookie.HttpOnly = true;    //XSS 之敵記得
});
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddOptions<SessionAuthenticationOptions>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("demo2", c =>
    {
        c.AddAuthenticationSchemes(SessionAuthenticationHandler.TEST_SCHEM_NAME);
        c.RequireAuthenticatedUser();
    });
});


builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "SomeToken"; //Cookie Name
    options.Cookie.HttpOnly = true;    //XSS 之敵記得
    options.Cookie.SameSite = SameSiteMode.Lax; //CSRF 之敵可以參考一下，但不一定要
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddJwtBearer(options => 
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test"))
    };
})
.AddCustomAuthenticationOptions(option =>
{

});



var app = builder.Build();

app.UseSession();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
