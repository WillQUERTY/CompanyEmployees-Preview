namespace PreviewApi.Infrastructure.Data;

using PreviewApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext para la aplicación
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Department Configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Department { Id = 1, Name = "Engineering", Description = "Software Development", IsActive = true },
                new Department { Id = 2, Name = "HR", Description = "Human Resources", IsActive = true }
            );
        });

        // Role Configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasMany(r => r.EmployeeRoles)
                .WithOne(er => er.Role)
                .HasForeignKey(er => er.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Role { Id = 1, Name = "Engineer", Description = "Software Engineer" },
                new Role { Id = 2, Name = "Manager", Description = "Team Manager" },
                new Role { Id = 3, Name = "Admin", Description = "Administrator" }
            );
        });

        // EmployeeRole Configuration (Many-to-Many)
        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(er => new { er.EmployeeId, er.RoleId });
            entity.HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId);
            entity.HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleId);
        });

        // Employee Configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.EmployeeRoles)
                .WithOne(er => er.Employee)
                .HasForeignKey(er => er.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    DepartmentId = 1,
                    HireDate = new DateTime(2022, 9, 14, 10, 15, 0, DateTimeKind.Utc),
                    IsActive = true,
                    // Solo una línea, con 'new'
                    CreatedAt = new DateTime(2024, 3, 17, 9, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    DepartmentId = 2,
                    HireDate = new DateTime(2022, 9, 14, 10, 15, 0, DateTimeKind.Utc),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 17, 9, 0, 0, DateTimeKind.Utc)
                }
            );
        });
    }
}
