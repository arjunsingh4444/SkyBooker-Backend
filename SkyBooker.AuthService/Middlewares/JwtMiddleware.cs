namespace SkyBooker.AuthService.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer "))
        {
            context.Request.Headers["Authorization"] = "Bearer " + token;
        }

        await _next(context);
    }
}