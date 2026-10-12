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
    
    public virtual BizarreCaptcha FindByHash(string hash)
    {
        var captcha = Captchas.Where(h => h.StringHash  == hash).FirstOrDefault();
            
        return captcha;
    }
    
    public virtual void Save(BizarreCaptcha obj)
    {
        Database.EnsureCreated();
        var entity = FindByHash(obj.StringHash);
        if (entity is null)
        {
            Captchas.Add(obj);
            SaveChanges();
        }
        else
        {
            throw new Exception("Hash already exists");
        }
    }

    public virtual void Delete(BizarreCaptcha obj)
    {
        Remove(obj);
        SaveChanges();
    }

    public virtual void UpdateEntity(BizarreCaptcha obj)
    {
        Update(obj);
        SaveChanges();
    }

    public virtual List<BizarreCaptcha> GetAllRecords()
    {
        return Captchas.ToList<BizarreCaptcha>();
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