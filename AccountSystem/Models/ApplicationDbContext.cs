using AccountSystem.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Incident -> Account relationship
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Account)
            .WithMany(a => a.Incidents)
            .HasForeignKey(i => i.AccountId);

        // Account -> Contact relationship
        modelBuilder.Entity<Account>()
            .HasMany(a => a.Contacts)
            .WithOne(c => c.Account)
            .HasForeignKey(c => c.AccountId);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Name)
            .IsUnique();

        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.Email)
            .IsUnique();
    }
}
