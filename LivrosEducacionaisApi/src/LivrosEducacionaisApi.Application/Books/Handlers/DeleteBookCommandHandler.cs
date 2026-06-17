using LivrosEducacionaisApi.Application.Books.Commands;
using LivrosEducacionaisApi.Application.Results;
using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Domain.Repositories;
using MediatR;

namespace LivrosEducacionaisApi.Application.Books.Handlers;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICacheService _cacheService;

    public DeleteBookCommandHandler(IBookRepository bookRepository, IBlobStorageService blobStorageService, ICacheService cacheService)
    {
        _bookRepository = bookRepository;
        _blobStorageService = blobStorageService;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book == null)
            return Result.Failure("Book not found");

        if (!string.IsNullOrEmpty(book.CoverUrl))
        {
            await _blobStorageService.DeleteFileAsync(book.CoverUrl, cancellationToken);
        }

        book.Delete();
        await _bookRepository.UpdateAsync(book, cancellationToken);
        await _cacheService.RemoveAsync($"book:{book.Id}", cancellationToken);

        return Result.Success();
    }
}
