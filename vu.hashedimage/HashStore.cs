using Microsoft.EntityFrameworkCore;

namespace vu.hashedimage;

public class HashStore: DbContext
{
    public DbSet<BizarreCaptcha> Captchas { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString =
            "Host=venera23-chic-plaza.cloud.layerbase.dev;Port=5432;Database=venera23;Username=postgres;Password=RnyMcaHswVWNIdQpoykDZtQf;";
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    public virtual BizarreCaptcha FindByHash(byte[] hash)
    {
        var captcha = Captchas.Find(1,null,hash);
            
        return captcha;
    }
    
    public virtual void Save(BizarreCaptcha obj)
    {
        Database.EnsureCreated();
        Captchas.Add(obj);
        SaveChanges();
    }


    public virtual async Task<bool> DeleteTables()
    {
        bool res = await Database.EnsureDeletedAsync();
        var types = Model.GetEntityTypes();
        foreach (var t in types)
        {
            var name = t.GetTableName();
            
        }

        return res;

    }

}