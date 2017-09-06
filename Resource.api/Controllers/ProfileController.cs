﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Resource.api.Filters;

namespace Resource.api.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            var user = User as ClaimsPrincipal;
            return Ok("You're Authorized");
        }
    }
}
