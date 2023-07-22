using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using toDoAPI.Models;

namespace toDoAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=DESKTOP-VRF2530\\MSSQL2022;Initial Catalog=toDoListDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            optionsBuilder.UseSqlServer(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User{ Id = 1, Name = "Kavindu Hasintha", Email = "kavindu@gmail.com", Password = "kavindu"},
                new User{ Id = 2, Name = "Ajith Hewage", Email = "ajith@gmail.com", Password = "ajith"}
            });
            /*
            modelBuilder.Entity<Todo>().HasData(new Todo[]
            {
                new Todo {Id = 1, TaskName = "Task 1", StartDate = DateTime.Now, DueDate = DateTime.Now.AddDays(5),
                    User = new User(){ Name = "John Smith", Email = "john@gmail.com", Password = "john"}},
                new Todo{Id = 2, TaskName = "Task 2", StartDate = DateTime.Now, DueDate = DateTime.Now.AddDays(3),
                    User = new User(){ Name = "Joe Root", Email = "joe@gmail.com", Password = "root"}}
            });
            */
        }
    }
}
