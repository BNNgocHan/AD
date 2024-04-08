using Microsoft.AspNetCore.Mvc;

namespace TrainingFPT.Controllers
{
    public class DemoController : Controller
    {
        //Demo/Index
        public string Index()
        {   
            return "Hello world";
        }

        //Demo/Test
        public string Test()
        {
            return "ASP.Net core MVC";
        }

        //Demo/IT0501
        public IActionResult IT0503()
        {
            //trả về 1 giao diện view
            return View();
        }
    }
}
