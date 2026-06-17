namespace LivrosEducacionaisApi.Api.Features.v1.Books.Requests;

public class UpdateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IFormFile? CoverImage { get; set; }
}
