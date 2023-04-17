using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using rent_products_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<UserType> _roles;
        public AuthorizeAttribute(params UserType[] roles)
        {
            _roles = roles ?? new UserType[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = (BaseUser)context.HttpContext.Items["User"];
            if (account == null || (_roles.Any() && !_roles.Contains(account.UserType)))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
