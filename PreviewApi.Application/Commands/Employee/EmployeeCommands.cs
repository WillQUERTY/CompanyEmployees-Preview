namespace PreviewApi.Application.Commands.Employee;

using PreviewApi.Application.Common;

/// <summary>
/// Command para crear un nuevo empleado
/// </summary>
public class CreateEmployeeCommand : ICommand<EmployeeDto>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }

    public CreateEmployeeCommand(CreateEmployeeDto dto)
    {
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Email = dto.Email;
        DepartmentId = dto.DepartmentId;
        HireDate = dto.HireDate;
    }
}

/// <summary>
/// Command para actualizar un empleado
/// </summary>
public class UpdateEmployeeCommand : ICommand<EmployeeDto>
{
    public int Id { get; set; }
    public UpdateEmployeeDto UpdateData { get; set; } = null!;

    public UpdateEmployeeCommand(int id, UpdateEmployeeDto updateData)
    {
        Id = id;
        UpdateData = updateData;
    }
}

/// <summary>
/// Command para eliminar un empleado
/// </summary>
public class DeleteEmployeeCommand : ICommand<bool>
{
    public int Id { get; set; }

    public DeleteEmployeeCommand(int id)
    {
        Id = id;
    }
}
