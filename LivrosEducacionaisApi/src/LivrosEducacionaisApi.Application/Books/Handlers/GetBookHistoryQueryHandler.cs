using LivrosEducacionaisApi.Application.Books.Queries;
using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class GetBookHistoryQueryHandler : IRequestHandler<GetBookHistoryQuery, Result<IEnumerable<BookVersionDto>>>
{
    private readonly IBookVersionRepository _bookVersionRepository;

    public GetBookHistoryQueryHandler(IBookVersionRepository bookVersionRepository)
    {
        _bookVersionRepository = bookVersionRepository;
    }

    public async Task<Result<IEnumerable<BookVersionDto>>> Handle(GetBookHistoryQuery request, CancellationToken cancellationToken)
    {
        var versions = await _bookVersionRepository.GetByBookIdAsync(request.BookId, cancellationToken);
        var dtos = versions.Select(v => new BookVersionDto(
            v.Id,
            v.VersionNumber,
            v.Title,
            v.Author,
            v.Subject,
            v.Description,
            v.CoverUrl,
            v.CreatedAt
        )).OrderByDescending(v => v.VersionNumber).ToList();

        return Result<IEnumerable<BookVersionDto>>.Success(dtos);
    }
}
