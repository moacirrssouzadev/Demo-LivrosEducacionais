using LivrosEducacionaisApi.Application.Books.Commands;
using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Domain.Entities;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookVersionRepository _bookVersionRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICacheService _cacheService;

    public UpdateBookCommandHandler(
        IBookRepository bookRepository,
        IBookVersionRepository bookVersionRepository,
        IBlobStorageService blobStorageService,
        ICacheService cacheService)
    {
        _bookRepository = bookRepository;
        _bookVersionRepository = bookVersionRepository;
        _blobStorageService = blobStorageService;
        _cacheService = cacheService;
    }

    public async Task<Result<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book == null || book.IsDeleted)
            return Result<BookDto>.Failure("Book not found");

        var coverValidationError = CoverFileRules.Validate(request.CoverFile);
        if (coverValidationError is not null)
            return Result<BookDto>.Failure(coverValidationError);

        string? oldCoverUrl = book.CoverUrl;
        string? newCoverUrl = null;
        if (request.CoverFile is not null)
        {
            newCoverUrl = await _blobStorageService.UploadFileAsync(
                request.CoverFile.FileName,
                request.CoverFile.FileData,
                request.CoverFile.ContentType,
                cancellationToken);
        }

        var version = new BookVersion(book);
        await _bookVersionRepository.AddAsync(version, cancellationToken);

        book.Update(request.Title, request.Author, request.Subject, request.Description, request.GradeLevel, request.PublicationDate, request.Status);
        if (newCoverUrl is not null)
        {
            book.SetCoverUrl(newCoverUrl);
            // Delete old cover if it exists
            if (!string.IsNullOrEmpty(oldCoverUrl))
            {
                await _blobStorageService.DeleteFileAsync(oldCoverUrl, cancellationToken);
            }
        }

        await _bookRepository.UpdateAsync(book, cancellationToken);
        await _cacheService.RemoveAsync($"book:{book.Id}", cancellationToken);

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

        return Result<BookDto>.Success(dto);
    }
}
