
using DementiaHelper.WebApi.model;
using Microsoft.EntityFrameworkCore;

namespace DementiaHelper.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AccountInformation> AccountInformations { get; set; }
        public DbSet<AccountPicture> AccountPictures { get; set; } 


           protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AccountInformation>()
            .HasOne(p => p.Picture)
            .WithOne(i => i.AccountInformation)
            .HasForeignKey<AccountPicture>(b => b.AccountInformationForeignKey);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
