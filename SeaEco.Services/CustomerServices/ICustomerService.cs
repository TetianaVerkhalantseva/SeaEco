using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerNamesDto>> GetCustomerNames();
    Task<Kunde?> GetCustomerInfoById(int customerId);
    Task<Kunde?> GetAllProjectDetailsById(int customerId);
    Task<EditCustomerResult> AddCustomer(EditCustomerDto customerDto);
    Task<EditCustomerResult> UpdateCustomer(EditCustomerDto customerDto, int customerId);
    Task<EditCustomerResult> DeleteCustomer(int customerId);
}