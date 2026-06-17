using System.ComponentModel.DataAnnotations;

namespace LivrosEducacionais.ViewModels;

public class UpdateBookViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, ErrorMessage = "Título não pode exceder 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Autor é obrigatório")]
    [StringLength(100, ErrorMessage = "Autor não pode exceder 100 caracteres")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Disciplina é obrigatória")]
    [StringLength(100, ErrorMessage = "Disciplina não pode exceder 100 caracteres")]
    public string Subject { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Descrição não pode exceder 1000 caracteres")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Série/Ano não pode exceder 50 caracteres")]
    public string? GradeLevel { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Data de publicação inválida")]
    public DateTime? PublicationDate { get; set; }

    [Required(ErrorMessage = "Status é obrigatório")]
    [StringLength(50, ErrorMessage = "Status não pode exceder 50 caracteres")]
    public string Status { get; set; } = string.Empty;

    public string? CurrentCoverUrl { get; set; }
    public IFormFile? CoverImage { get; set; }
}
