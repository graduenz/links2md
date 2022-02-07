using HtmlAgilityPack;

Console.WriteLine("Fetching data...");

// TODO: Add args option instead of hardcoded links?

// Just paste the links separated by a line break
var links = @"https://microservices.io/
https://refactoring.guru/
https://roadmap.sh/
https://dev.to/apkoponen/100-tips-on-software-developer-productivity-36if
https://charm.sh/
https://monorepo.tools/
https://dev.to/this-is-learning/from-developer-to-solutions-architect-a-simple-guide-2b91
https://dev.to/dotnet/authentication-in-asp-net-core-59k8
https://www.thereformedprogrammer.net/building-asp-net-core-and-ef-core-hierarchical-multi-tenant-apps/
https://simonwillison.net/2022/Jan/31/release-notes/
https://showcode.app/
https://www.mockable.io/
https://excalidraw.com/
https://conventionalcomments.org/"
// Then split it into a string array
.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

// Fetch data from the links to build the result
using var http = new HttpClient();
var result = new List<string>();

await Parallel.ForEachAsync(links, async (link, ctok) =>
{
    var response = await http.GetAsync(link);
    
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Link {link} went bad with status code {response.StatusCode}");
        return;
    }

    var html = await response.Content.ReadAsStringAsync();

    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    // Get page title, if empty use the link itself
    var title = htmlDoc.DocumentNode.SelectSingleNode("//head//title")?.InnerText ?? link;

    // Sets markdown formatted link with page title
    result.Add($"[{title}]({link})");
});

// Outputs formatted markdown links
result.ForEach(Console.WriteLine);