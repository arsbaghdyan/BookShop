﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Common.ClientService.Impl;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.EmployeeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Services.Impl;

internal class EmployeeService : IEmployeeService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;

    public EmployeeService(BookShopDbContext bookShopDbContext,
        ILogger<EmployeeService> logger,
        IMapper mapper,
        IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<EmployeeModel> EmployeeLoginAsync(string email, string password)
    {
        var employee = await _bookShopDbContext.Employees
           .FirstOrDefaultAsync(p => p.Email == email);

        if (employee != null)
        {
            var hashedPassword = HashPassword(password);
            if (employee.Password == hashedPassword)
            {
                return _mapper.Map<EmployeeModel>(employee);
            }
        }

        return null;
    }

    public async Task<EmployeeModel> RegisterAsync(EmployeeRegisterModel employeeRegisterModel)
    {
        var employeeToAdd = _mapper.Map<EmployeeEntity>(employeeRegisterModel);
        employeeToAdd.Password = HashPassword(employeeRegisterModel.Password);

        _bookShopDbContext.Employees.Add(employeeToAdd);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Employee with {employeeToAdd.Id} Id added successfully.");

        return _mapper.Map<EmployeeModel>(employeeToAdd);
    }

    public async Task<EmployeeModel> UpdateAsync(EmployeeUpdateModel employeeUpdateModel)
    {
        var employeeToUpdate = await _bookShopDbContext.Employees
            .FirstOrDefaultAsync(c => c.Id == 10);

        if (employeeToUpdate == null)
        {
            throw new Exception("Employee not found");
        }

        employeeToUpdate.FirstName = employeeUpdateModel.FirstName;
        employeeToUpdate.LastName = employeeUpdateModel.LastName;
        employeeToUpdate.Email = employeeUpdateModel.Email;
        employeeToUpdate.Address = employeeUpdateModel.Address;
        employeeToUpdate.Position = employeeUpdateModel.Position;
        employeeToUpdate.Salary = employeeUpdateModel.Salary;


        if (!string.IsNullOrEmpty(employeeUpdateModel.Password))
        {
            employeeToUpdate.Password = HashPassword(employeeUpdateModel.Password);
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Employee with  Id modified successfully.");

        return _mapper.Map<EmployeeModel>(employeeToUpdate);
    }

    public async Task RemoveAsync()
    {
        await _bookShopDbContext.Employees
            .ExecuteDeleteAsync();

        _logger.LogInformation($"Employee with  Id removed successfully.");
    }

    private string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var passwordHash = SHA256.HashData(passwordBytes);

        return Convert.ToHexString(passwordHash);
    }
}
