namespace LivrosEducacionaisApi.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? CoverUrl { get; private set; }
    public int Version { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Book() { }

    public Book(string title, string author, string subject, string? description = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Subject = subject;
        Description = description;
        Version = 1;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string author, string subject, string? description)
    {
        Title = title;
        Author = author;
        Subject = subject;
        Description = description;
        Version++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCoverUrl(string coverUrl)
    {
        CoverUrl = coverUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
