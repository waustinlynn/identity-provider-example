using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Resource.api.Controllers
{
    public class AngSsoController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("You made it to the api");
        }
    }
}
