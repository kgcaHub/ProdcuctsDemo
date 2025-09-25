using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Products.Web.Pages;

public class IndexModel(IHttpClientFactory httpClientFactory) : PageModel
{
    private readonly HttpClient _api = httpClientFactory.CreateClient("ProductsApi");

    public List<ProductDto> Products { get; private set; } = [];

    [BindProperty] public string Name { get; set; } = "";
    [BindProperty] public decimal Price { get; set; }
    [BindProperty] public string Category { get; set; } = "";

    public async Task OnGetAsync()
    {
        Products = await _api.GetFromJsonAsync<List<ProductDto>>("products") ?? [];
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
            ModelState.AddModelError(nameof(Name), "Name is required.");
        if (string.IsNullOrWhiteSpace(Category))
            ModelState.AddModelError(nameof(Category), "Category is required.");
        if (Price <= 0)
            ModelState.AddModelError(nameof(Price), "Price has to be more than 0");
        if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

        var req = new CreateProductRequest(Name, Price, Category);
        var resp = await _api.PostAsJsonAsync("products", req);

        if (!resp.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "API error al crear el producto.");
            await OnGetAsync();
            return Page();
        }

        // Evita repost
        return RedirectToPage();
    }
}

public record ProductDto(int Id, string Name, decimal Price, string Category);
public record CreateProductRequest(string Name, decimal Price, string Category);

