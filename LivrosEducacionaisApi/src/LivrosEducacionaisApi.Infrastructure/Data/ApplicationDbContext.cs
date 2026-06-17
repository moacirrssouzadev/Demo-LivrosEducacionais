using LivrosEducacionaisApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LivrosEducacionaisApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookVersion> BookVersions { get; set; }
}
