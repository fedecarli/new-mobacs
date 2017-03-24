﻿using Softpark.Models;
using System.Web.Http;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// Base Api Controller
    /// </summary>
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Domain models
        /// </summary>
        protected DomainContainer Domain => DomainContainer.Current;
    }
}