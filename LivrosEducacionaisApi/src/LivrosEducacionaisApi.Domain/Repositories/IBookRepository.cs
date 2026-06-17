using LivrosEducacionaisApi.Domain.Entities;

namespace LivrosEducacionaisApi.Domain.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task AddAsync(Book book, CancellationToken cancellationToken);
    Task UpdateAsync(Book book, CancellationToken cancellationToken);
}
