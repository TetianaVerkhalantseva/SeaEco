using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.Contexts;
using SeaEco.Services.UserServices;

namespace SeaEco.Services.CustomerServices;

public class CustomerService: ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CustomerNamesDto>> GetCustomerNames()
    {
        var customers = await _db.Kundes.ToListAsync();

        return customers.Select(c => new CustomerNamesDto
        {
            CustomerId = c.Kundeid,
            CustomerName = c.Oppdragsgiver
        }).ToList();
    }
    
    public async Task<Kunde?> GetCustomerById(int customerId)
    {
        var customer = await _db.Kundes.FirstOrDefaultAsync(c => c.Kundeid == customerId);

        if (customer == null || customer.Kundeid == 0)
        {
            return null;
        }
        
        return customer;
    }
}

