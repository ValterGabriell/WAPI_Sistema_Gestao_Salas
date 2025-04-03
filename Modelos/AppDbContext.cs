using Microsoft.EntityFrameworkCore;

namespace WAPI_GS.Modelos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }
    public DbSet<TblPtd> TblUsersSala { get; set; }

    public DbSet<TblSala> TblSalas { get; set; }

    public DbSet<TblProfessor> TblUsers { get; set; }
    public DbSet<TblAuth> TblAuth { get; set; }
    public DbSet<TblTurma> TblTurma { get; set; }
    public DbSet<TblDisciplina> TblDisciplina { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TblDisciplina>().HasOne(e => e.tblTurma).WithOne().HasForeignKey<TblDisciplina>(e => e.TurmaId);
    }
}
