using ASPNET.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ASPNET.Common.AuthorizeAttributeFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowRemoteAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        

        public bool AllowRemote { get; set; }
        public string RouteController { get; set; }
        public string RouteAction { get; set; }

        
        public AllowRemoteAuthorizeAttribute(bool allowRemote, string RouteController, string RouteAction)
        {

            if (RouteController == null)
                throw new ArgumentNullException(nameof(RouteController));

            if (RouteAction == null)
                throw new ArgumentNullException(nameof(RouteAction));


            this.AllowRemote = allowRemote;
            this.RouteController = RouteController;
            this.RouteAction = RouteAction;

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //throw new NotImplementedException();
            if (!AllowRemote && !context.HttpContext.Request.IsLocal())
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                //throw new UnauthorizedAccessException();
                context.Result = 
                new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", RouteController },   // Change "Account" to your login controller name
                    { "action", RouteAction }       // Change "Login" to your login action method name
                });

                //context.HttpContext.Response.Redirect(string.Empty);
            }
        }



    }
}
