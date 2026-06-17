using LivrosEducacionaisApi.Api.Features.v1.Books.Requests;
using LivrosEducacionaisApi.Application.Books.Commands;
using LivrosEducacionaisApi.Application.Books.Queries;
using LivrosEducacionaisApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LivrosEducacionaisApi.Api.Features.v1.Books;

[ApiController]
[Route("api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<BookDto>> Create([FromForm] CreateBookRequest request, CancellationToken cancellationToken)
    {
        var coverFile = await ReadCoverFileAsync(request.CoverImage, cancellationToken);
        var command = new CreateBookCommand(request.Title, request.Author, request.Subject, request.Description, coverFile);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedBooksDto>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetBooksQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<BookDto>> Update(Guid id, [FromForm] UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var coverFile = await ReadCoverFileAsync(request.CoverImage, cancellationToken);
        var command = new UpdateBookCommand(id, request.Title, request.Author, request.Subject, request.Description, coverFile);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return result.Error == "Book not found"
                ? NotFound(result.Error)
                : BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return NoContent();
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<BookVersionDto>>> GetHistory(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookHistoryQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    private static async Task<BookCoverFile?> ReadCoverFileAsync(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return null;

        try
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            return new BookCoverFile(ms.ToArray(), file.FileName, file.ContentType);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }
}
