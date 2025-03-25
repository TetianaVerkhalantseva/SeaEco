using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.Services.CustomerServices;

public class CustomerService: ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    // Get all the customer ids and names
    public async Task<List<CustomerNamesDto>> GetCustomerNames()
    {
        var customers = await _db.Kundes.ToListAsync();

        return customers.Select(c => new CustomerNamesDto
        {
            CustomerId = c.Kundeid,
            CustomerName = c.Oppdragsgiver
        }).ToList();
    }
    
    // Get information except projects for a customer
    public async Task<Kunde?> GetCustomerInfoById(int customerId)
    {
        var customer = await _db.Kundes
            .Where(c => c.Kundeid == customerId)
            .FirstOrDefaultAsync();

        if (customer == null || customerId == 0)
        {
            return null;
        }
        
        return customer;
    }

    // Get all the project ids, statuses and dates for a customer
    public async Task<Kunde?> GetAllProjectDetailsById(int customerId)
    {
        var customer = await _db.Kundes
            .Where(c=> c.Kundeid == customerId)
            .Include(c => c.BProsjekts)
            .FirstOrDefaultAsync();

        if (customer != null)
        {
            customer.BProsjekts = customer.BProsjekts
                .Select(p => new BProsjekt
                {
                    Prosjektid = p.Prosjektid,
                    PoId = p.PoId,
                    Status = p.Status,
                    Datoregistrert = p.Datoregistrert
                }).ToList();
        }
        
        return customer;
    }

    public async Task<EditCustomerResult> AddCustomer(EditCustomerDto customerDto)
    {
        var customer = new Kunde()
        {
            Oppdragsgiver = customerDto.Oppdragsgiver,
            Kontaktperson = customerDto.Kontaktperson,
            Telefonnummer = customerDto.Telefonnummer,
        };

        try
        {
            await _db.Kundes.AddAsync(customer);
            await _db.SaveChangesAsync();
            return new EditCustomerResult
            {
                IsSuccess = true,
                Message = $"Customer {customer.Kundeid}: {customer.Oppdragsgiver} was successfully added!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = "An error occured while adding customer."
            };
        }
    }

    public async Task<EditCustomerResult> UpdateCustomer(EditCustomerDto customerDto, int customerId)
    {
        var customer = await _db.Kundes.FirstOrDefaultAsync(c => c.Kundeid == customerId);

        if (customer == null || customerId == 0)
        {
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = $"Customer with ID {customerId} does not exist."
            };
        }
        
        customer.Oppdragsgiver = customerDto.Oppdragsgiver;
        customer.Kontaktperson = customerDto.Kontaktperson;
        customer.Telefonnummer = customerDto.Telefonnummer;

        try
        {
            _db.Kundes.Update(customer);
            await _db.SaveChangesAsync();
            return new EditCustomerResult
            {
                IsSuccess = true,
                Message = $"Customer with ID {customer.Kundeid} was successfully updated!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = "An error occured while updating customer."
            };
        }
    }

    public async Task<EditCustomerResult> DeleteCustomer(int customerId)
    {
        var customer = await _db.Kundes.FirstOrDefaultAsync(c => c.Kundeid == customerId);
        if (customer == null || customerId == 0)
        {
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = $"Customer with ID {customerId} does not exist."
            };
        }
        
        _db.Kundes.Remove(customer);
        await _db.SaveChangesAsync();
        return new EditCustomerResult
        {
            IsSuccess = true,
            Message = $"Customer {customerId} was successfully deleted!"
        };
    }
}

