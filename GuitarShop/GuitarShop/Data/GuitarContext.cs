using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class GuitarContext : IdentityDbContext<AcessPerson, RolePerson, Guid>
{
    public GuitarContext(DbContextOptions<GuitarContext> options) : base(options)
    {
        
    }
    public DbSet<Guitar> Guitars { get; set; }
    public DbSet<Type> Types { get; set; }
    
}