using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Resource.api.Filters
{
    public class IdpAuth : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                base.OnAuthorization(actionContext);

                var user = Thread.CurrentPrincipal as ClaimsPrincipal;
                var something = string.Empty;
            }catch(Exception ex)
            {
                //actionContext.Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }
        }
    }
}