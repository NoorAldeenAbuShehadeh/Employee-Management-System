using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Model
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }
    }
}
