using libros.Models;
using Microsoft.EntityFrameworkCore;

namespace libros.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Libro> Libros { get; set; }
    }
}