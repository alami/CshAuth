using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.Auth.Web.Data
{
    public class AuthDbContext : IdentityDbContext<PlayBallUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("public");
            // builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }
    }
}