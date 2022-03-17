using BaseModule.ActivityManagement.Dto;
using BaseModule.ActivityManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventorySystemMysql.ActionFilters
{
    public class ActivityLogFilters : ActionFilterAttribute
    {
        private readonly ActivityLogServiceInterface _activityService;
  
        public ActivityLogFilters(ActivityLogServiceInterface activityService)
        {
            _activityService = activityService;
        }
        public override  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = ((ControllerBase)context.Controller)
               .ControllerContext.ActionDescriptor.ControllerName;

            var actionName = ((ControllerBase)context.Controller)
                 .ControllerContext.ActionDescriptor.ActionName;

            var actionDescriptorRouteValues = ((ControllerBase)context.Controller)
               .ControllerContext.ActionDescriptor.RouteValues;
            var Area = "";
            if (actionDescriptorRouteValues.ContainsKey("area"))
            {
                var area = actionDescriptorRouteValues["area"];
                if (area != null)
                {
                    Area = Convert.ToString(area);
                }
            }

            var session = context.HttpContext.Session.Id;
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var browser = context.HttpContext.Request.Headers["user-agent"].ToString();
            var pageAccessed = Convert.ToString(context.HttpContext.Request.Path);
            var header = context.HttpContext.Request.GetTypedHeaders();
            var userId = "";
            var userName = "";
            var user = context.HttpContext.User;
            if (user.Claims.Count() >0 )
            {
                userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                userName = user.FindFirst(ClaimTypes.Name).Value;
            }
            Uri uriReferer = header.Referer;
            var uriRef = "";
            if (uriReferer != null)
            {
                uriRef = header.Referer.AbsoluteUri;
            }

            var activityDto = new ActivityLogDto(Area, controllerName, actionName, ipAddress, pageAccessed, session, userName, userId, uriRef, browser);
            await _activityService.Create(activityDto);
            await next();
        }
     
    }
}
