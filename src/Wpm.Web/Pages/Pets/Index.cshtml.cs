using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Wpm.Web.Dal;
using Wpm.Web.Domain;

namespace Wpm.Web.Pages.Pets;

[AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
public class IndexModel : PageModel
{
    private readonly WpmDbContext dbContext;
    private readonly GraphServiceClient graphServiceClient;

    public IEnumerable<Pet>? Pets { get; private set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public IndexModel(WpmDbContext dbContext, GraphServiceClient graphServiceClient)
    {
        this.dbContext = dbContext;
        this.graphServiceClient = graphServiceClient;
    }
    public async Task OnGet()
    {
        var me = await graphServiceClient.Me.Request().GetAsync();
        ViewData["DisplayName"] = me.DisplayName;

        Pets = dbContext.Pets
            .Include(p => p.Breed)
            .ThenInclude(b => b.Species)
            .Where(p => string.IsNullOrWhiteSpace(Search) ? true :
                    p.Name.ToLowerInvariant().Contains(Search))
            .ToList();
    }
}