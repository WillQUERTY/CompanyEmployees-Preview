namespace PreviewApi.Application.Handlers.Queries;

using PreviewApi.Application.Queries.Employee;
using PreviewApi.Application.Common;
using PreviewApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler para GetAllEmployeesQuery
/// </summary>
public class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAllEmployeesQueryHandler> _logger;

    public GetAllEmployeesQueryHandler(AppDbContext context, ILogger<GetAllEmployeesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employees");

        var employees = await _context.Employees
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return employees.Select(MapToDto).ToList();
    }

    private static EmployeeDto MapToDto(Infrastructure.Entities.Employee employee) => new()
    {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email,
        DepartmentId = employee.DepartmentId,
        HireDate = employee.HireDate,
        IsActive = employee.IsActive,
        CreatedAt = employee.CreatedAt
    };
}

/// <summary>
/// Handler para GetEmployeeByIdQuery
/// </summary>
public class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(AppDbContext context, ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching employee: {Id}", query.Id);

        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);

        if (employee == null)
        {
            _logger.LogWarning("Employee not found: {Id}", query.Id);
            return null;
        }

        return MapToDto(employee);
    }

    private static EmployeeDto MapToDto(Infrastructure.Entities.Employee employee) => new()
    {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email,
        DepartmentId = employee.DepartmentId,
        HireDate = employee.HireDate,
        IsActive = employee.IsActive,
        CreatedAt = employee.CreatedAt
    };
}
