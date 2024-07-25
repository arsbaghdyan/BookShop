using BookShop.Common.EmployeeService.Abstractions;

namespace BookShop.Common.EmployeeService.Impl;

internal class EmployeeContextWriter : IEmployeeContextWriter
{
    private readonly EmployeeContext _employeeContext;

    public EmployeeContextWriter(EmployeeContext employeeContext)
    {
        _employeeContext = employeeContext;
    }

    public void SetEmployeeContextId(long Id)
    {
        _employeeContext.Id = Id;
    }
}
