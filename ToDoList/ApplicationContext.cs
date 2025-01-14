using Microsoft.EntityFrameworkCore;

namespace ToDoList
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ToDoItem> Items { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>().HasData(
                    new ToDoItem { Id = 1, Name = "ASP.NET", Description = "Сделай самое простое что только можно" },
                    new ToDoItem { Id = 2, Name = "MS SQL", Description = "Для начала подойдет и локальная db" });
        }
    }
}
