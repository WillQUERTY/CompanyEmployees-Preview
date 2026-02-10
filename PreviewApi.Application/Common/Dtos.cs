namespace PreviewApi.Application.Common;

// ═══════════════════════════════════════════════════════════════════
// Authentication DTOs
// ═══════════════════════════════════════════════════════════════════

/// <summary>
/// DTO para login
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = null!;
}

/// <summary>
/// DTO para respuesta de login
/// </summary>
public class LoginResponse
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}

// ═══════════════════════════════════════════════════════════════════
// Employee DTOs
// ═══════════════════════════════════════════════════════════════════

/// <summary>
/// DTO para crear un empleado
/// </summary>
public class CreateEmployeeDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
}

/// <summary>
/// DTO para actualizar un empleado
/// </summary>
public class UpdateEmployeeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }
    public DateTime? HireDate { get; set; }
}

/// <summary>
/// DTO de respuesta del empleado
/// </summary>
public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO extendido para empleado con departamento y roles
/// </summary>
public class EmployeeDetailDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DepartmentDto Department { get; set; } = null!;
    public ICollection<RoleDto> Roles { get; set; } = new List<RoleDto>();
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ═══════════════════════════════════════════════════════════════════
// Department DTOs
// ═══════════════════════════════════════════════════════════════════

/// <summary>
/// DTO para crear un departamento
/// </summary>
public class CreateDepartmentDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO para actualizar un departamento
/// </summary>
public class UpdateDepartmentDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// DTO de respuesta del departamento
/// </summary>
public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ═══════════════════════════════════════════════════════════════════
// Role DTOs
// ═══════════════════════════════════════════════════════════════════

/// <summary>
/// DTO para crear un rol
/// </summary>
public class CreateRoleDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO para actualizar un rol
/// </summary>
public class UpdateRoleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// DTO de respuesta del rol
/// </summary>
public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
