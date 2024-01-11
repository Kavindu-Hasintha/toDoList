using toDoAPI.Models;

namespace toDoAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ForgetPassword> ForgetPasswords { get; set; }
        public DbSet<Email> Emails { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Email>()
                .HasOne(e => e.User)
                .WithMany(u => u.Emails)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
