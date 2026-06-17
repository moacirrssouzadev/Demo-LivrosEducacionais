using LivrosEducacionaisApi.Domain.Entities;
using LivrosEducacionaisApi.Domain.Repositories;
using LivrosEducacionaisApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LivrosEducacionaisApi.Infrastructure.Repositories;

public class BookVersionRepository : IBookVersionRepository
{
    private readonly ApplicationDbContext _context;

    public BookVersionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookVersion>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await _context.BookVersions
            .Where(v => v.BookId == bookId)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BookVersion bookVersion, CancellationToken cancellationToken)
    {
        await _context.BookVersions.AddAsync(bookVersion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
