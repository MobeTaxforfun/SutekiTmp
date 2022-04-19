using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Domain.Common.Authentication.Session;
using SutekiTmp.Domain.Common.Authorization;
using SutekiTmp.Domain.Common.Authorization.Requirement;
using SutekiTmp.Domain.Common.Extensions;
using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Domain.Repository.Repository;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Domain.Service.Service;
using SutekiTmp.Middleware;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var isController = Matching.FromSource("SutekiTmp.Controllers");
// Add Logger 
Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
//.MinimumLevel.Debug()
//.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//.MinimumLevel.Override("System", LogEventLevel.Warning)
.WriteTo.Console()
.WriteTo.Logger(wt => wt.Enrich.FromLogContext().Filter.ByIncludingOnly("Storage = 'File' and LogType = 'SystemException'").WriteTo.File("logs/SystemException.txt", rollingInterval: RollingInterval.Day))
.WriteTo.Logger(wt => wt.Enrich.FromLogContext().Filter.ByIncludingOnly("Storage = 'Mssql' and LogType = 'MailLog'").WriteTo.Console())

.Enrich.FromLogContext()
.CreateLogger();

try
{

    builder.Host.UseSerilog(Log.Logger, dispose: true);
    // Add services to the container.
    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "suteki"; //Cookie Name
        options.Cookie.HttpOnly = true;    //XSS 之敵記得
    });
    builder.Services.AddControllersWithViews();
    //HttpContex 的額外獲取方法
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    //Repository 注入
    builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<ILoginService, LoginService>();
    builder.Services.AddTransient<IPromisionRepository, PromisionRepository>();
    builder.Services.AddTransient<IRoleMenuPromisionRepository, RoleMenuPromisionRepository>();
    builder.Services.AddTransient<IMeunRepository, MeunRepository>();
    //Custom方法注入
    builder.Services.AddOptions<SessionAuthenticationOptions>();
    builder.Services.AddSingleton<IAuthorizationHandler, MenuHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, MenuPerssionHanlder>();
    builder.Services.AddSingleton<IAuthorizationHandler, PermissionsHandler>();

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
    .AddSessionAuthenticationnOptions(option =>
    {
        option.SessionKeyName = "UserName";
        option.SessionKeyId = "UId";
    })
    .AddCustomAuthenticationOptions(option =>
     {

     });


    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Premission", policy =>
        {
            policy.Requirements.Add(new PermissionsRequirement());
        });
        options.AddPolicy("Menu", policy =>
        {
            policy.Requirements.Add(new MenuRequirement());
        });
    });

    builder.Services.AddSwaggerSetup();

    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.Use(async (context, next) =>
    {
        await next();
        //錯誤攔截器
        switch(context.Response.StatusCode)
        {
            case 400:
                context.Response.Redirect("/Error/HttpError400"); //400 Bad request
                return;
            case 401:
                context.Response.Redirect("/Error/HttpError401"); //401 Unauthorized
                return;
            case 403:
                context.Response.Redirect("/Error/HttpError400"); //403 Forbidden
                return;
        }
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", "APIDocumentV1");
        options.SwaggerEndpoint($"/swagger/v2/swagger.json", "APIDocumentV2");
        options.RoutePrefix = String.Empty;
    });

    app.UseSession();
    app.UseAuthentication();
    app.UseAuthorization();

    app.DemandService(builder.Services);    // 專案元件查詢方法
    app.DemandController();                 // 專案元件查詢方法

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "執行階段錯誤");
}
finally
{
    Log.CloseAndFlush();
}