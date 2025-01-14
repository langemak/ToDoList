using ToDoList;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True";
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/items", async (ApplicationContext db) => await db.Items.ToListAsync());

app.MapGet("/api/items/{id:int}", async (int id, ApplicationContext db) =>
{
    ToDoItem? item = await db.Items.FirstOrDefaultAsync(u => u.Id == id);

    if (item == null) return Results.NotFound(new { message = "Item not found" });

    return Results.Json(item);
});

app.MapDelete("/api/items/{id:int}", async (int id, ApplicationContext db) =>
{
    ToDoItem? item = await db.Items.FirstOrDefaultAsync(u => u.Id == id);

    if (item == null) return Results.NotFound(new { message = "Item not found" });

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Json(item);
});

app.MapPost("/api/items", async (ToDoItem item, ApplicationContext db) =>
{
    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return item;
});

app.MapPut("/api/items", async (ToDoItem itemData, ApplicationContext db) =>
{
    var item = await db.Items.FirstOrDefaultAsync(u => u.Id == itemData.Id);

    if (item == null) return Results.NotFound(new { message = "Item not found" });

    item.Description = itemData.Description;
    item.Name = itemData.Name;
    await db.SaveChangesAsync();
    return Results.Json(item);
});

app.Run();
