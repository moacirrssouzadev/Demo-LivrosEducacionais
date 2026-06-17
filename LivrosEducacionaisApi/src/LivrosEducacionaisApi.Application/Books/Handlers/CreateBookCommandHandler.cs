using LivrosEducacionaisApi.Application.Books.Commands;
using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Domain.Entities;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookVersionRepository _bookVersionRepository;
    private readonly IBlobStorageService _blobStorageService;

    public CreateBookCommandHandler(IBookRepository bookRepository, IBookVersionRepository bookVersionRepository, IBlobStorageService blobStorageService)
    {
        _bookRepository = bookRepository;
        _bookVersionRepository = bookVersionRepository;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var coverValidationError = CoverFileRules.Validate(request.CoverFile);
        if (coverValidationError is not null)
            return Result<BookDto>.Failure(coverValidationError);

        var book = new Book(request.Title, request.Author, request.Subject, request.Description);

        if (request.CoverFile is not null)
        {
            var coverUrl = await _blobStorageService.UploadFileAsync(
                request.CoverFile.FileName,
                request.CoverFile.FileData,
                request.CoverFile.ContentType,
                cancellationToken);

            book.SetCoverUrl(coverUrl);
        }

        await _bookRepository.AddAsync(book, cancellationToken);
        
        var version = new BookVersion(book);
        await _bookVersionRepository.AddAsync(version, cancellationToken);

        var dto = new BookDto(
            book.Id,
            book.Title,
            book.Author,
            book.Subject,
            book.Description,
            book.CoverUrl,
            book.Version,
            book.CreatedAt,
            book.UpdatedAt
        );

        return Result<BookDto>.Success(dto);
    }
}
