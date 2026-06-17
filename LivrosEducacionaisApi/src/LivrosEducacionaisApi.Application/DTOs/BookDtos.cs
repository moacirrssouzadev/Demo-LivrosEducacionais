namespace LivrosEducacionaisApi.Application.DTOs;

public record CreateBookDto(string Title, string Author, string Subject, string? Description, string? GradeLevel, DateTime? PublicationDate, string? Status);

public record UpdateBookDto(string Title, string Author, string Subject, string? Description, string? GradeLevel, DateTime? PublicationDate, string Status);

public record BookDto(Guid Id, string Title, string Author, string Subject, string? Description, string? GradeLevel, DateTime? PublicationDate, string Status, string? CoverUrl, int Version, DateTime CreatedAt, DateTime? UpdatedAt);

public record BookVersionDto(Guid Id, int VersionNumber, string Title, string Author, string Subject, string? Description, string? GradeLevel, DateTime? PublicationDate, string Status, string? CoverUrl, DateTime CreatedAt);

public record PaginatedBooksDto(IEnumerable<BookDto> Books, int PageNumber, int PageSize, int TotalCount, int TotalPages);
