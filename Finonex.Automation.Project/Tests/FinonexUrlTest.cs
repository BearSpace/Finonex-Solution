using Finonex.Automation.Project.Common;
using Finonex.Automation.Project.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;

namespace Finonex.Automation.Project.Tests;

[Category("Production")]
public class FinonexUrlTest : TestBase
{
    private static ILogger Logger => Log.ForContext<FinonexUrlTest>();

    [SetUp]
    public void Setup()
    {
        Logger.Information("Warm up system");
    }

    [Test]
    public async Task UrlUnderTest()
    {
        var page = await BrowserContext.NewPageAsync();

        var configUrl = ConfigurationManager.FullUrl;
        if (configUrl != null)
        {
            await page.GotoAsync(configUrl);

            // Wait for the page to load
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            Logger.Information("Url from config: {Url}", configUrl);
        }

        var uiUrl = page.Url;
        Logger.Information("Url from browser: {Url}", uiUrl);

        Assert.That(uiUrl, Does.Contain("finonex"), "Url doesn't contains url from configuration");
    }
}