namespace PreviewApi.Controllers;

using PreviewApi.Application.Commands.Employee;
using PreviewApi.Application.Common;
using PreviewApi.Application.Queries.Employee;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API Controller para gestión de empleados
/// Demuestra el patrón CQRS (Command Query Responsibility Segregation)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los empleados
    /// [QUERY] GetAllEmployeesQuery -> GetAllEmployeesQueryHandler
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(CancellationToken cancellationToken)
    {
        _logger.LogInformation("GET /api/employees");
        var query = new GetAllEmployeesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtener empleado por ID
    /// [QUERY] GetEmployeeByIdQuery -> GetEmployeeByIdQueryHandler
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GET /api/employees/{Id}", id);
        var query = new GetEmployeeByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Crear nuevo empleado
    /// [COMMAND] CreateEmployeeCommand -> CreateEmployeeCommandHandler
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("POST /api/employees - Email: {Email}", dto.Email);
        var command = new CreateEmployeeCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
    }

    /// <summary>
    /// Actualizar empleado
    /// [COMMAND] UpdateEmployeeCommand -> UpdateEmployeeCommandHandler
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PUT /api/employees/{Id}", id);
        var command = new UpdateEmployeeCommand(id, dto);
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Eliminar empleado
    /// [COMMAND] DeleteEmployeeCommand -> DeleteEmployeeCommandHandler
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DELETE /api/employees/{Id}", id);
        var command = new DeleteEmployeeCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
