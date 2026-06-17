namespace LivrosEducacionaisApi.Domain.Entities;

public class BookVersion
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public int VersionNumber { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? GradeLevel { get; private set; }
    public DateTime? PublicationDate { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string? CoverUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private BookVersion() { }

    public BookVersion(Book book)
    {
        Id = Guid.NewGuid();
        BookId = book.Id;
        VersionNumber = book.Version;
        Title = book.Title;
        Author = book.Author;
        Subject = book.Subject;
        Description = book.Description;
        GradeLevel = book.GradeLevel;
        PublicationDate = book.PublicationDate;
        Status = book.Status;
        CoverUrl = book.CoverUrl;
        CreatedAt = DateTime.UtcNow;
    }
}
