namespace LivrosEducacionais.ViewModels;

public class BookHistoryViewModel
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public List<BookVersionItemViewModel> Versions { get; set; } = new();
}

public class BookVersionItemViewModel
{
    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
