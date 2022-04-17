using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SutekiTmp.Controllers
{
    public class LoggedController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public LoggedController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("This is Debug");              //Level 0
            _logger.LogError("This is Error");              //Level 1
            _logger.LogTrace("This is Trace");              //Level 2
            _logger.LogInformation("This is Information");  //Level 3

            var User = new { UserName = "Mobe", Email = "Test@gmail" };
            var ErrorMessage = "Something Error";
            Log.Information("{Storage},{LogType},Processed {User} have {@ErrorMessage} ms.", "Mssql", "MailLog", User, ErrorMessage);
            Log.Information("{Storage},{LogType},Processed {User} have {@ErrorMessage} ms.", "File", "SystemException", User, ErrorMessage);

            return View();
        }
    }
}
