using Microsoft.EntityFrameworkCore;

namespace Common.Database.Entity.Sqlite;
public class SqliteContext : DbContext
{
    public DbSet<Config> Configs { get; set; }

    private string ConnStr;
    public SqliteContext(string connStr)
    {
        ConnStr = connStr;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnStr);
        //optionsBuilder.UseSqlServer(@"Server=DESKTOP-EDM16LK\SQLEXPRESS;Database=SMARTLOGISTICS;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}