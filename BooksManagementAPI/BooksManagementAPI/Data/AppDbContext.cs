using BooksManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
    }
}
