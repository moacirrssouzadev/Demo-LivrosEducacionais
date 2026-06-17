using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Results;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Queries;

public record GetBooksQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PaginatedBooksDto>>;
