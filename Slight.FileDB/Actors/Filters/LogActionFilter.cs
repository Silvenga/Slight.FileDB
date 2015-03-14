using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Slight.FileDB.Actors.Filters {
    public class LogActionFilter : ActionFilterAttribute {

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken) {

            var request = actionExecutedContext.Request;
            //var reposnse = actionExecutedContext.Response;
            await Log(request.Method.Method, request.RequestUri.AbsolutePath, "");

        }

        public static async Task Log(string method, string resource, string status) {

            await Task.Run(
                delegate {

                    var message = string.Format("{0} {1} {2}", method, resource, status);
                    Console.WriteLine(message);
                });
        }
    }
}
