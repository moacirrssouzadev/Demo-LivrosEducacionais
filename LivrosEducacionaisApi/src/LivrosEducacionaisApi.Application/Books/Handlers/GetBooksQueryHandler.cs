using LivrosEducacionaisApi.Application.Books.Queries;
using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<PaginatedBooksDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICacheService _cacheService;

    public GetBooksQueryHandler(IBookRepository bookRepository, ICacheService cacheService)
    {
        _bookRepository = bookRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<PaginatedBooksDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"books:page:{request.PageNumber}:size:{request.PageSize}";
        var cachedResult = await _cacheService.GetAsync<PaginatedBooksDto>(cacheKey, cancellationToken);
        if (cachedResult != null)
            return Result<PaginatedBooksDto>.Success(cachedResult);

        var books = await _bookRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _bookRepository.GetTotalCountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var dtos = books.Select(b => new BookDto(
            b.Id,
            b.Title,
            b.Author,
            b.Subject,
            b.Description,
            b.CoverUrl,
            b.Version,
            b.CreatedAt,
            b.UpdatedAt
        )).ToList();

        var result = new PaginatedBooksDto(dtos, request.PageNumber, request.PageSize, totalCount, totalPages);
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(2), cancellationToken);

        return Result<PaginatedBooksDto>.Success(result);
    }
}
