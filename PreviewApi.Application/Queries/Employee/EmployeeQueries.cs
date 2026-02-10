namespace PreviewApi.Application.Queries.Employee;

using PreviewApi.Application.Common;

/// <summary>
/// Query para obtener todos los empleados
/// </summary>
public class GetAllEmployeesQuery : IQuery<IEnumerable<EmployeeDto>>
{
}

/// <summary>
/// Query para obtener un empleado por ID
/// </summary>
public class GetEmployeeByIdQuery : IQuery<EmployeeDto?>
{
    public int Id { get; set; }

    public GetEmployeeByIdQuery(int id)
    {
        Id = id;
    }
}
