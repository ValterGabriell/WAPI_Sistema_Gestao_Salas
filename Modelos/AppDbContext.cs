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
        modelBuilder.Entity<TblUsersSala>()
       .HasOne(us => us.TblUser)  // TblUsersSala tem um TblUser
       .WithMany() // TblUser pode ter muitos TblUsersSala
       .HasForeignKey(us => us.UserId); // A chave estrangeira para TblUser é UserId

        // Definindo o relacionamento com TblSala
        modelBuilder.Entity<TblUsersSala>()
            .HasOne(us => us.TblSala)  // TblUsersSala tem um TblSala
            .WithMany() // TblSala pode ter muitos TblUsersSala
            .HasForeignKey(us => us.SalaId); // A chave estrangeira para TblSala é SalaId

    }
}
