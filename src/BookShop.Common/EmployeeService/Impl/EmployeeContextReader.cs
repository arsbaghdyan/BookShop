using BookShop.Common.EmployeeService.Abstractions;

namespace BookShop.Common.EmployeeService.Impl;

internal class EmployeeContextReader : IEmployeeContextReader
{
    private readonly EmployeeContext _employeeContext;

    public EmployeeContextReader(EmployeeContext employeeContext)
    {
        _employeeContext = employeeContext;
    }

    public long GetEmployeeContextId()
    {
        return _employeeContext.Id;
    }
}
