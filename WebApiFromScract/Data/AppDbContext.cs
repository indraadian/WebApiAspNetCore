using Microsoft.EntityFrameworkCore;
using WebApiFromScract.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiFromScract.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
    }
}
