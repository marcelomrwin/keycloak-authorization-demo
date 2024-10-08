namespace dotnet;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permission;
    private readonly string _scope;

    public PermissionAuthorizeAttribute(string permission, string scope)
    {
        _permission = permission;
        _scope = scope;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpClientFactory =
            context.HttpContext.RequestServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
        var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = await CheckPermission(token, _permission, _scope, httpClientFactory);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }

    private async Task<bool> CheckPermission(string token, string permission, string scope,
        IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"),
            new KeyValuePair<string, string>("response_mode", "decision"),
            new KeyValuePair<string, string>("permission", $"{permission}#{scope}"),
            new KeyValuePair<string, string>("claim_token_format", "urn:ietf:params:oauth:token-type:jwt"),
            new KeyValuePair<string, string>("audience", "rbac_client"),
        });

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response =
            await client.PostAsync("http://localhost:8180/realms/rbac/protocol/openid-connect/token", content);

        if (response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(resultString);
            return result["result"].Value<bool>() == true;
        }
        else
        {
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.ReasonPhrase);
        }

        return false;
    }
}