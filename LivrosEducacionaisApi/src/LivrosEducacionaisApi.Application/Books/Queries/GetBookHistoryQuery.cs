using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Results;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Queries;

public record GetBookHistoryQuery(Guid BookId) : IRequest<Result<IEnumerable<BookVersionDto>>>;
