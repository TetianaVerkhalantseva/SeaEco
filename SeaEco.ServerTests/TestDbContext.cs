using Microsoft.EntityFrameworkCore;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.ServerTests;

public class TestDbContext : AppDbContext
{
    public TestDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}