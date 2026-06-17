namespace LivrosEducacionais.Dtos;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? GradeLevel { get; set; }
    public DateTime? PublicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CoverUrl { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
