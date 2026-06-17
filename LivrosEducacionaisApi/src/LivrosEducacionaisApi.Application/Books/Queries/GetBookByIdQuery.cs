using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Results;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Queries;

public record GetBookByIdQuery(Guid Id) : IRequest<Result<BookDto>>;
