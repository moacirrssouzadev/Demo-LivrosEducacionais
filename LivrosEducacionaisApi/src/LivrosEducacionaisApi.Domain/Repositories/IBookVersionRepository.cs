using LivrosEducacionaisApi.Domain.Entities;

namespace LivrosEducacionaisApi.Domain.Repositories;

public interface IBookVersionRepository
{
    Task<IEnumerable<BookVersion>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task AddAsync(BookVersion bookVersion, CancellationToken cancellationToken);
}
