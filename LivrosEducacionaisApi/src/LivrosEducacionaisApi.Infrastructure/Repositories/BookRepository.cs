using LivrosEducacionaisApi.Domain.Entities;
using LivrosEducacionaisApi.Domain.Repositories;
using LivrosEducacionaisApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LivrosEducacionaisApi.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _context.Books.CountAsync(b => !b.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken)
    {
        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
