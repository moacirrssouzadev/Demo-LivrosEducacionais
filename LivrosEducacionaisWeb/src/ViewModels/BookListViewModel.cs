using System.ComponentModel.DataAnnotations;

namespace LivrosEducacionais.ViewModels;

public class BookListViewModel
{
    public List<BookItemViewModel> Books { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class BookItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? CoverUrl { get; set; }
    public int Version { get; set; }
}
