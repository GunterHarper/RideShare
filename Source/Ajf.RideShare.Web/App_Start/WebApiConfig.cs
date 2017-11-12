﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Ajf.RideShare.Web
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new AddCustomHeaderActionFilterAttribute());
            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            return config;
        }
    }

    public class AddCustomHeaderActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            switch (actionExecutedContext.Request.Headers.Host)
            {
                case "localhost":
                    actionExecutedContext.ActionContext.Response.Headers.Add("Access-Control-Allow-Origin", actionExecutedContext.Request.Headers.Host);
                    break;
            }
            //actionExecutedContext.ActionContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            actionExecutedContext.ActionContext.Response.Headers.Add("Access-Control-Allow-Methods","GET, POST, PATCH, PUT, DELETE, OPTIONS");
            actionExecutedContext.ActionContext.Response.Headers.Add("Access-Control-Allow-Headers","Origin, Content-Type, X-Auth-Token");
        }
    }
}
