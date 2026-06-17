using LivrosEducacionaisApi.Application.Books.Queries;
using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICacheService _cacheService;

    public GetBookByIdQueryHandler(IBookRepository bookRepository, ICacheService cacheService)
    {
        _bookRepository = bookRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"book:{request.Id}";
        var cachedDto = await _cacheService.GetAsync<BookDto>(cacheKey, cancellationToken);
        if (cachedDto != null)
            return Result<BookDto>.Success(cachedDto);

        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book == null || book.IsDeleted)
            return Result<BookDto>.Failure("Book not found");

        var dto = new BookDto(
            book.Id,
            book.Title,
            book.Author,
            book.Subject,
            book.Description,
            book.GradeLevel,
            book.PublicationDate,
            book.Status,
            book.CoverUrl,
            book.Version,
            book.CreatedAt,
            book.UpdatedAt
        );

        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5), cancellationToken);
        return Result<BookDto>.Success(dto);
    }
}
