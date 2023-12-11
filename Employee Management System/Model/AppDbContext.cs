using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Model
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary keys for tables

            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.UserEmail);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.Name);

            modelBuilder.Entity<Salary>()
                .HasKey(s => s.EmployeeEmail);

            modelBuilder.Entity<Leave>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.Id);

            // Configure relationships between tables

            // Employee - User *** one - one relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserEmail)
                .IsRequired(false);// because not all user is employee ==> there is admin

            // Department - Manager (Employee) *** one - one relationship
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithOne() // the manager can manage only one depertment
                .HasForeignKey<Department>(d => d.ManagerEmail)
                .IsRequired(false);//can be null

            // Department - Employees *** one - many relationship
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentName);

            // Salary - Employee *** one - one relationship
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithOne(e => e.Salary)
                .HasForeignKey<Salary>(s => s.EmployeeEmail);

            // Leave - Employee *** many - one relationship
            modelBuilder.Entity<Leave>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeEmail);

            // Attendance - Employee *** many - one relationship
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeEmail);

            base.OnModelCreating(modelBuilder);
        }

    }
}
