using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wpm.Web.Dal;
using Wpm.Web.Domain;

namespace Wpm.Web.Pages.Pets;

public class CreateModel : PageModel
{
    private readonly WpmDbContext dbContext;

    [BindProperty]
    public Pet? Pet { get; set; }
    public CreateModel(WpmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        dbContext.Pets.Add(Pet);
        dbContext.SaveChanges();
        return RedirectToPage("Pets");
    }
}
