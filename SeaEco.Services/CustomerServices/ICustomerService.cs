using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerNamesDto>> GetCustomerNames();
    Task<Kunde?> GetCustomerInfoById(Guid Id);
    Task<Kunde?> GetAllProjectDetailsById(Guid Id);
    Task<EditCustomerResult> AddCustomer(EditCustomerDto customerDto);
    Task<EditCustomerResult> UpdateCustomer(EditCustomerDto customerDto, Guid Id);
    Task<EditCustomerResult> DeleteCustomer(Guid Id);
}