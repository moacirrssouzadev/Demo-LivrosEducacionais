using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Results;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Commands;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    string Subject,
    string? Description,
    string? GradeLevel,
    DateTime? PublicationDate,
    string Status,
    BookCoverFile? CoverFile = null) : IRequest<Result<BookDto>>;
