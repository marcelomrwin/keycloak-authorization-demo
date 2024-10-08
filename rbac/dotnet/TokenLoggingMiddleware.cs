using Microsoft.AspNetCore.Authentication;

namespace dotnet;

public class TokenLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = await context.GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"Token: {token}");
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var tk = authHeader.Substring("Bearer ".Length).Trim();
            Console.WriteLine($"Token: {tk}");
        }

        await next(context);
    }
}