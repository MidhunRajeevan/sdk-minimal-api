using sdkstyleDemo.model;
var builder = WebApplication.CreateBuilder(args);

// ✅ Add Swagger and API Explorer services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Enable Swagger always (not just in Development)
app.UseSwagger();
app.UseSwaggerUI();

// Sample in-memory data
List<Product> products =
[
    new Product(1, "Laptop", 1500),
    new Product(2, "Phone", 800),
    new Product(3, "Tablet", 400),
    new Product(4, "Monitor", 300),
    new Product(5, "Keyboard", 100)
];

app.MapGet("/products", () =>
    Results.Ok(products));

app.MapGet("/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/products", (Product newProduct) =>
{
    var nextId = products.Max(p => p.Id) + 1;
    var product = newProduct with { Id = nextId };
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();
