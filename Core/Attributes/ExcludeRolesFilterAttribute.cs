using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Attributes;

public class ExcludeRolesFilterAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public ExcludeRolesFilterAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (_roles.Any(role => user.IsInRole(role)))
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
    }
}