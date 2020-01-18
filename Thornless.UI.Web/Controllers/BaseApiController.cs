using Microsoft.AspNetCore.Mvc;
using Thornless.UI.Web.ViewModels;

namespace Thornless.UI.Web.Controllers
{
    public abstract class BaseApiController : Controller
    {
        protected JsonResult ApiResponse(object data)
        {
            var apiResponse = new ApiResponseViewModel
            {
                Data = data,
            };
            
            return Json(apiResponse);
        }
    }
}
