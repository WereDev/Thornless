using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Thornless.UI.Web.ViewModels;

namespace Thornless.UI.Web.AppStart
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure =>
            {
                configure.Run(async handler =>
                {
                    var contextFeature = handler.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var logger = handler.RequestServices.GetService<ILogger>();
                        logger.LogError(contextFeature.Error, $"Error in URL: {handler.Request.Path.Value}");
                        var apiResponse = new ApiResponseViewModel
                        {
                            Errors = new ErrorDetailsModel[]
                            {
                                new ErrorDetailsModel
                                {
                                    ErrorCode = 500,
                                    ErrorMessage = contextFeature.Error.Message,
                                },
                            },
                        };

                        var jsonResponse = JsonConvert.SerializeObject(apiResponse);

                        handler.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await handler.Response.WriteAsync(jsonResponse);
                    }
                });
            });
        }
    }
}
