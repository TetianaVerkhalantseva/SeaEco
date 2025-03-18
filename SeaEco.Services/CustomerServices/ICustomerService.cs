using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerNamesDto>> GetCustomerNames();
    Task<Kunde?> GetCustomerById(int customerId);
    Task<AddCustomerResult> AddCustomer(AddCustomerDto customerDto);
}