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

        if (customer == null || customerId == 0)
        {
            return null;
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
            Orgnr = customerDto.Orgnr,
            Postadresse = customerDto.Postadresse,
            Kommune = customerDto.Kommune,
            Fylke = customerDto.Fylke
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
        customer.Orgnr = customerDto.Orgnr;
        customer.Postadresse = customerDto.Postadresse;
        customer.Kommune = customerDto.Kommune;
        customer.Fylke = customerDto.Fylke;

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

