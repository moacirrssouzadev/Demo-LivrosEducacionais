namespace LivrosEducacionais.Dtos;

public class PaginatedBooksDto
{
    public List<BookDto> Books { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
