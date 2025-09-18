using Coditech.Admin.Utilities;
using Microsoft.AspNetCore.WebUtilities;
namespace Coditech.Admin.Middleware
{
    public class EncryptedQueryMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptedQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("data"))
            {
                try
                {
                    var encrypted = context.Request.Query["data"];
                    var decrypted = EncryptionHelper.Decrypt(encrypted!);

                    var dict = QueryHelpers.ParseQuery(decrypted);

                    var queryCollection = new QueryCollection(dict.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new Microsoft.Extensions.Primitives.StringValues(kvp.Value.ToArray())
                    ));

                    context.Request.Query = queryCollection;
                }
                catch { }
            }

            context.Response.OnStarting(() =>
            {
                if (context.Response.StatusCode == 302 && context.Response.Headers.ContainsKey("Location"))
                {
                    var location = context.Response.Headers["Location"].ToString();
                    var uri = new Uri(location, UriKind.RelativeOrAbsolute);
                    if (uri.IsAbsoluteUri && !string.IsNullOrEmpty(uri.Query))
                    {
                        var query = uri.Query.TrimStart('?');
                        var encrypted = EncryptionHelper.Encrypt(query);
                        var newUrl = uri.GetLeftPart(UriPartial.Path) + "?data=" + Uri.EscapeDataString(encrypted);
                        context.Response.Headers["Location"] = newUrl;
                    }
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
