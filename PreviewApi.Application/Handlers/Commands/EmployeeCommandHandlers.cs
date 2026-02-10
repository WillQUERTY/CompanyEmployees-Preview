namespace PreviewApi.Application.Handlers.Commands;

using PreviewApi.Application.Commands.Employee;
using PreviewApi.Application.Common;
using PreviewApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PreviewApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler para CreateEmployeeCommand
/// </summary>
public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(AppDbContext context, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating employee: {Email}", command.Email);

        var employee = new Employee
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            DepartmentId = command.DepartmentId,
            HireDate = command.HireDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee created with ID: {Id}", employee.Id);

        return MapToDto(employee);
    }

    private static EmployeeDto MapToDto(Employee employee) => new()
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
/// Handler para UpdateEmployeeCommand
/// </summary>
public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(AppDbContext context, ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating employee: {Id}", command.Id);

        var employee = await _context.Employees.FindAsync(new object[] { command.Id }, cancellationToken);

        if (employee == null)
        {
            _logger.LogWarning("Employee not found: {Id}", command.Id);
            throw new InvalidOperationException($"Employee with ID {command.Id} not found");
        }

        if (!string.IsNullOrEmpty(command.UpdateData.FirstName))
            employee.FirstName = command.UpdateData.FirstName;

        if (!string.IsNullOrEmpty(command.UpdateData.LastName))
            employee.LastName = command.UpdateData.LastName;

        if (!string.IsNullOrEmpty(command.UpdateData.Email))
            employee.Email = command.UpdateData.Email;

        if (command.UpdateData.DepartmentId.HasValue)
            employee.DepartmentId = command.UpdateData.DepartmentId.Value;

        if (command.UpdateData.HireDate.HasValue)
            employee.HireDate = command.UpdateData.HireDate.Value;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee updated: {Id}", command.Id);

        return MapToDto(employee);
    }

    private static EmployeeDto MapToDto(Employee employee) => new()
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
/// Handler para DeleteEmployeeCommand
/// </summary>
public class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(AppDbContext context, ILogger<DeleteEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting employee: {Id}", command.Id);

        var employee = await _context.Employees.FindAsync(new object[] { command.Id }, cancellationToken);

        if (employee == null)
        {
            _logger.LogWarning("Employee not found: {Id}", command.Id);
            return false;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee deleted: {Id}", command.Id);

        return true;
    }
}
