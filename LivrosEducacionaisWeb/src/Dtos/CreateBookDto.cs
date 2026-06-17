namespace LivrosEducacionais.Dtos;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
}
