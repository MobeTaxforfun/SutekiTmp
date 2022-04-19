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
        options.Cookie.HttpOnly = true;    //XSS ���İO�o
    });
    builder.Services.AddControllersWithViews();
    //HttpContex ���B�~�����k
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    //Repository �`�J
    builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<ILoginService, LoginService>();
    builder.Services.AddTransient<IPromisionRepository, PromisionRepository>();
    builder.Services.AddTransient<IRoleMenuPromisionRepository, RoleMenuPromisionRepository>();
    builder.Services.AddTransient<IMeunRepository, MeunRepository>();
    //Custom��k�`�J
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
        options.Cookie.HttpOnly = true;    //XSS ���İO�o
        options.Cookie.SameSite = SameSiteMode.Lax; //CSRF ���ĥi�H�ѦҤ@�U�A�����@�w�n
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
        //���~�d�I��
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

    app.DemandService(builder.Services);    // �M�פ���d�ߤ�k
    app.DemandController();                 // �M�פ���d�ߤ�k

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "���涥�q���~");
}
finally
{
    Log.CloseAndFlush();
}