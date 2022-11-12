using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SmartCookbook.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<string> _languages = new List<string>
    {
        "en",
        "pt"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var culture = new CultureInfo("pt");

        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = context.Request.Headers["Accept-Language"].ToString().Split(",");
            culture = new CultureInfo(language[0]);
        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
