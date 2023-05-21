using Microsoft.Extensions.Configuration;
using Serilog;

namespace Finonex.Automation.Project.Core;

public static class ConfigurationManager
{
    private const string ErrorMessage = "{0} is an invalid URL";
    private static string? Protocol { get; set; }
    private static string? Url { get; set; }
    private static string? Port { get; set; }
    public static string? FullUrl { get; private set; }

    public static string BrowserType { get; private set; }

    public static void CreateConfigurationContext()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        /*properties - TODO: move properties to dto, use attributes [JsonProperty]*/

        /*Url builder*/
        Protocol = configuration.GetValue<string>("AppSettings:protocol") ?? throw new InvalidOperationException();
        Url = configuration.GetValue<string>("AppSettings:url") ??
              throw new InvalidOperationException("Url could not null");
        Port = configuration.GetValue<string>("AppSettings:port") ??
               throw new InvalidOperationException("Url could not null");

        FullUrl = BuildUrl(Protocol, Url, int.Parse(Port));

        /*Browser properties*/
        BrowserType = configuration.GetValue<string>("AppSettings:browserType") ??
                      throw new InvalidOperationException("Url could not null");
    }

    private static string BuildUrl(string protocol, string url, int port)
    {
        url = string.Concat(protocol, "://", url, ":", port);

        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            var uri = new Uri(url);
            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                Log.Logger.Information("Valid URL");
            else
                throw new ArgumentException(string.Format(ErrorMessage, url));
        }
        else
        {
            throw new ArgumentException(string.Format(ErrorMessage, url));
        }

        return url;
    }
}