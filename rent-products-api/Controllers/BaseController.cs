using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rent_products_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public BaseUser Account => (BaseUser)HttpContext.Items["User"];
    }
}
