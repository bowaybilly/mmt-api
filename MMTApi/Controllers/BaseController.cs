using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMTApi.Controllers
{
    //api can also prevent anonymous access by using JWT authentication 
    //dynamic route to allow api versioning
    [ApiController]
    [Route("api/{v1}")]
    public class BaseController : ControllerBase
    {
        

       

       
    }
}
