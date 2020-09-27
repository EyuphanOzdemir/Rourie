using System.Diagnostics;
using RourieWebAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace RourieWebAPI.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("500")] //this infamous application (server) error comes here 
        public IActionResult AppError()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //for logging purposes
            StringBuilder error = new StringBuilder();
            error.Append("\nSPECIAL LOG: EXCEPTION:");
            error.Append("\n"+exceptionHandlerPathFeature.Path);
            error.Append("\n"+exceptionHandlerPathFeature.Error.Message);
            error.Append("\n"+exceptionHandlerPathFeature.Error.StackTrace);

            // LogError() method logs the exception under Error category in the log
            logger.LogError(error.ToString());

            return View();
        }

        [Route("404")] //famous 404 error comes here
        public IActionResult PageNotFound()
        {
            logger.LogWarning("SPECIAL LOG: 404 error occured. Path = "+ HttpContext.Request.Path + HttpContext.Request.QueryString.Value);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Explanation="404!", QueryString=HttpContext.Request.QueryString.Value});
        }

        //the other error types can be handled similarly
    }
}