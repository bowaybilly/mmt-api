using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMTApi.Controllers
{
    /// <summary>
    /// Base controller that provides common functionality for all other controllers
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public class BaseController : ControllerBase
    {
    //dynamic route to allow api versioning
    //api can also prevent anonymous access by using JWT authentication 
        
    }
}
