using Finonex.Automation.Project.Common;
using Finonex.Automation.Project.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;

namespace Finonex.Automation.Project.Tests;

[Category("Production")]
public class FinonexGetContactTest : TestBase
{
    private IPage _page = null!;
    private static ILogger Logger => Log.ForContext<FinonexGetContactTest>();

    [SetUp]
    public async Task Setup()
    {
        Logger.Information("Warm up system");
        _page = await BrowserContext.NewPageAsync();

        if (ConfigurationManager.FullUrl != null) await _page.GotoAsync(ConfigurationManager.FullUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task GetContactPhone()
    {
        /*Navigate to Contact page*/
        await _page.ClickAsync("#menu-item-39 > a");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await _page.QuerySelectorAsync(
            "#primary > div.contactUsMapsSection > div > div > div.contactUsMapsLeft.col-lg-4 > div.contactUsPlaces > ul > li:nth-child(1) > div > div.contactUsPlacePhone > a"); // Assuming <h1> is the target element
        if (element != null)
        {
            var text = await element.TextContentAsync() ?? throw new InvalidOperationException("Element not found");
            Logger.Information("Phone number: {Phone}", text);
        }
    }
}