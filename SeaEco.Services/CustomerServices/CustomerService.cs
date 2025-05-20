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
            Id = c.Id,
            CustomerName = c.Oppdragsgiver
        }).ToList();
    }
    
    public async Task<CustomerDto?> GetCustomerInfoById(Guid Id)
    {
        var customer = await _db.Kundes
            .Where(c => c.Id == Id)
            .FirstOrDefaultAsync();

        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Oppdragsgiver = customer.Oppdragsgiver,
            Kontaktperson = customer.Kontaktperson,
            Telefon = customer.Telefon
        };
    }

    public async Task<Kunde?> GetAllProjectDetailsById(Guid Id)
    {
        var customer = await _db.Kundes
            .Where(c=> c.Id == Id)
            .Include(c => c.BProsjekts)
            .FirstOrDefaultAsync();

        if (customer != null)
        {
            customer.BProsjekts = customer.BProsjekts
                .Select(p => new BProsjekt
                {
                    Id = p.Id,
                    PoId = p.PoId,
                    Prosjektstatus = p.Prosjektstatus
                }).ToList();
        }
        
        return customer;
    }

    public async Task<EditCustomerResult> AddCustomer(EditCustomerDto customerDto)
    {
        var customer = new Kunde()
        {
            Id = Guid.NewGuid(),
            Oppdragsgiver = customerDto.Oppdragsgiver,
            Kontaktperson = customerDto.Kontaktperson,
            Telefon = customerDto.Telefon,
        };

        try
        {
            await _db.Kundes.AddAsync(customer);
            await _db.SaveChangesAsync();
            return new EditCustomerResult
            {
                IsSuccess = true,
                Message = $"Customer {customer.Id}: {customer.Oppdragsgiver} was successfully added!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($@"Error: {e.Message}");
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = "An error occured while adding customer."
            };
        }
    }

    public async Task<EditCustomerResult> UpdateCustomer(EditCustomerDto customerDto, Guid Id)
    {
        var customer = await _db.Kundes.FirstOrDefaultAsync(c => c.Id == Id);

        if (customer == null)
        {
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = $"Customer with ID {Id} does not exist."
            };
        }
        
        customer.Oppdragsgiver = customerDto.Oppdragsgiver;
        customer.Kontaktperson = customerDto.Kontaktperson;
        customer.Telefon = customerDto.Telefon;

        try
        {
            _db.Kundes.Update(customer);
            await _db.SaveChangesAsync();
            return new EditCustomerResult
            {
                IsSuccess = true,
                Message = $"Customer with ID {customer.Id} was successfully updated!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($@"Error: {e.Message}");
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = "An error occured while updating customer."
            };
        }
    }

    public async Task<EditCustomerResult> DeleteCustomer(Guid Id)
    {
        var customer = await _db.Kundes.FirstOrDefaultAsync(c => c.Id == Id);
        if (customer == null)
        {
            return new EditCustomerResult
            {
                IsSuccess = false,
                Message = $"Customer with ID {Id} does not exist."
            };
        }
        
        _db.Kundes.Remove(customer);
        await _db.SaveChangesAsync();
        return new EditCustomerResult
        {
            IsSuccess = true,
            Message = $"Customer {Id} was successfully deleted!"
        };
    }
}
