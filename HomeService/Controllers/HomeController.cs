using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeService.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallVerisureService([FromServices] INodeServices nodeServices)
        {
            var result = await nodeServices.InvokeAsync<string>(@"C:\Users\pa_suja\Source\Repos\HomeService\HomeService\wwwroot\lib\Verisure\verisureApp.js");
                return Content(result);
        }
    }
}
