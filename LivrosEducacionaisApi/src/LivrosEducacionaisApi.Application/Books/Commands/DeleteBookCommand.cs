using LivrosEducacionaisApi.Application.Results;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Commands;

public record DeleteBookCommand(Guid Id) : IRequest<Result>;
