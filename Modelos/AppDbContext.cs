using Microsoft.EntityFrameworkCore;

namespace WAPI_GS.Modelos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }
    public DbSet<TblUsersSala> TblUsersSala { get; set; }

    public DbSet<TblSala> TblSalas { get; set; }

    public DbSet<TblUser> TblUsers { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TblUsersSala>()
           .HasKey(us => new { us.UserId, us.SalaId }); 
    }
}
