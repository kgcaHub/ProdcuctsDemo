using Products.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CoreDbContext>(options => options.UseInMemoryDatabase("ProductsDb"));

var app = builder.Build();

app.MapGet("/api/products", async (CoreDbContext dbContext) =>
{
    return await dbContext.Products.AsNoTracking().ToListAsync();
});

app.MapPost("/api/products", async (CoreDbContext dbCobtext, CreateProductRequest request) =>
{
    if (ValidationHelper.ValidateOrProblem(request) is IResult problem)
        return problem;


    var newProduct = new Product()
    {
        Name = request.Name,
        Category = request.Category,
        Price = request.Price
    };

    await dbCobtext.Products.AddAsync(newProduct);
    await dbCobtext.SaveChangesAsync();

    return Results.Created();
});

app.Run();
