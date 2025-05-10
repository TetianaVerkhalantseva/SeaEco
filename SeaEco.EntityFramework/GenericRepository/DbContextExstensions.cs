using Microsoft.EntityFrameworkCore;

namespace SeaEco.EntityFramework.GenericRepository;

public static class DbContextExtensions
{
    public static void TruncateAllTablesPostgres(this DbContext context)
    {
        var tables = context.Model.GetEntityTypes()
            .Select(e => new
            {
                Schema = e.GetSchema() ?? "public",
                Name   = e.GetTableName()
            })
            .Distinct()
            .Select(t => $"\"{t.Schema}\".\"{t.Name}\"");

        var joined = string.Join(", ", tables);

        
        context.Database.ExecuteSqlRaw(
            $"TRUNCATE TABLE {joined} RESTART IDENTITY CASCADE;"
        );
    }
}