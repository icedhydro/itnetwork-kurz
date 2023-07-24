using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EvidencePojisteni.Models;

namespace EvidencePojisteni.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EvidencePojisteni.Models.Insured> Insured { get; set; }
        public DbSet<EvidencePojisteni.Models.Insurance> Insurance { get; set; }
        public DbSet<EvidencePojisteni.Models.InsEvent> Event { get; set; }

    }
}