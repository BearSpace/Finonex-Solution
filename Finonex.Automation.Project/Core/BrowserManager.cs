using Microsoft.Playwright;

namespace Finonex.Automation.Project.Core;

public static class BrowserManager
{
    public static async Task<IBrowser> CreateBrowser(IPlaywright playwright)
    {
        ConfigurationManager.CreateConfigurationContext();
        var browserType = ConfigurationManager.BrowserType;
        IBrowser browser;

        switch (browserType)
        {
            case "chrome":
                var chromeBrowserTypeLaunchOptions = GetChromiumConfiguration();
                browser = await playwright.Chromium.LaunchAsync(chromeBrowserTypeLaunchOptions);
                break;
            case "webkit":
                var webkitBrowserTypeLaunchOptions = GetWebkitConfiguration();
                browser = await playwright.Webkit.LaunchAsync(webkitBrowserTypeLaunchOptions);
                break;
            case "firefox":
                var firefoxBrowserTypeLaunchOptions = GetFirefoxConfiguration();
                browser = await playwright.Firefox.LaunchAsync(firefoxBrowserTypeLaunchOptions);
                break;
            default:
                throw new NotSupportedException($"The provided browser {browserType} is not supported!");
        }

        return browser;
    }

    /*Browser configuration for Chromium*/
    private static BrowserTypeLaunchOptions GetChromiumConfiguration()
    {
        var options = new BrowserTypeLaunchOptions
        {
            Headless = false,
            Args = new[]
            {
                "--start-fullscreen",
                "--enable-automation",
                "--no-sandbox",
                "--disable-infobars",
                "--disable-dev-shm-usage",
                "--disable-browser-side-navigation",
                "--disable-gpu",
                "--disable-web-security",
                "--ignore-certificate-errors",
                "--ignore-ssl-errors",
                "--disable-extensions",
                "--verbose"
            }
        };

        return options;
    }

    private static BrowserTypeLaunchOptions GetFirefoxConfiguration()
    {
        throw new NotImplementedException();
    }

    private static BrowserTypeLaunchOptions GetWebkitConfiguration()
    {
        throw new NotImplementedException();
    }

    /*WebDriver installation*/
    public static void InstallForOsx()
    {
        var exitCode = Program.Main(new[] { "install" });
        if (exitCode != 0) throw new Exception($"Playwright exited with code {exitCode}");
    }
}