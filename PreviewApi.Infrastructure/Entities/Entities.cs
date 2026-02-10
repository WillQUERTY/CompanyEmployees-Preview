namespace PreviewApi.Infrastructure.Entities;

/// <summary>
/// Entidad Department (Departamento)
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } 

    // Relaciones
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

/// <summary>
/// Entidad Role (Rol)
/// </summary>
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } 

    // Relaciones
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}

/// <summary>
/// Entidad EmployeeRole (Relación muchos-a-muchos)
/// </summary>
public class EmployeeRole
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime AssignedAt { get; set; } 
}

/// <summary>
/// Entidad Employee actualizada con relaciones
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}
