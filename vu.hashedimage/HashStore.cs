using Microsoft.EntityFrameworkCore;

namespace vu.hashedimage;

public class HashStore: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString =
            "postgresql://postgres:RnyMcaHswVWNIdQpoykDZtQf@venera23-chic-plaza.cloud.layerbase.dev:27802/venera23";
        optionsBuilder.UseNpgsql(connectionString);
    }
    // Constructor required for Dependency Injection configuration
    public HashStore(DbContextOptions<HashStore> options) : base(options)
    {
        
    }
}