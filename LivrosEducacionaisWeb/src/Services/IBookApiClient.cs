using LivrosEducacionais.Dtos;
using LivrosEducacionais.Results;

namespace LivrosEducacionais.Services;

public interface IBookApiClient
{
    Task<Result<PaginatedBooksDto>> GetBooksAsync(int pageNumber = 1, int pageSize = 10);
    Task<Result<BookDto?>> GetBookByIdAsync(Guid id);
    Task<Result<BookDto?>> CreateBookAsync(CreateBookDto book, Stream? coverStream = null, string? coverFileName = null, string? coverContentType = null);
    Task<Result<BookDto?>> UpdateBookAsync(Guid id, UpdateBookDto book, Stream? coverStream = null, string? coverFileName = null, string? coverContentType = null);
    Task<Result> DeleteBookAsync(Guid id);
    Task<Result<List<BookVersionDto>>> GetBookHistoryAsync(Guid id);
}
