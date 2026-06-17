using LivrosEducacionais.Dtos;
using LivrosEducacionais.Results;
using LivrosEducacionais.Services;
using LivrosEducacionais.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivrosEducacionais.Controllers;

public class BooksController : Controller
{
    private readonly IBookApiClient _bookApiClient;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookApiClient bookApiClient, ILogger<BooksController> logger)
    {
        _bookApiClient = bookApiClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
    {
        ViewData["Title"] = "Livros";
        try
        {
            var result = await _bookApiClient.GetBooksAsync(pageNumber, pageSize);
            if (!result.IsSuccess || result.Value == null)
            {
                TempData["ErrorMessage"] = result.Error ?? "Ocorreu um erro ao carregar a lista de livros.";
                return View(new BookListViewModel());
            }

            var viewModel = new BookListViewModel
            {
                Books = result.Value.Books.Select(b => new BookItemViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Subject = b.Subject,
                    CoverUrl = b.CoverUrl,
                    Version = b.Version
                }).ToList(),
                PageNumber = result.Value.PageNumber,
                PageSize = result.Value.PageSize,
                TotalCount = result.Value.TotalCount,
                TotalPages = result.Value.TotalPages
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar lista de livros");
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar a lista de livros. Por favor, tente novamente.";
            return View(new BookListViewModel());
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        ViewData["Title"] = "Detalhes do Livro";
        try
        {
            var result = await _bookApiClient.GetBookByIdAsync(id);
            if (!result.IsSuccess || result.Value == null)
            {
                TempData["ErrorMessage"] = result.Error ?? "Livro não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new BookDetailsViewModel
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Author = result.Value.Author,
                Subject = result.Value.Subject,
                Description = result.Value.Description,
                CoverUrl = result.Value.CoverUrl,
                Version = result.Value.Version,
                CreatedAt = result.Value.CreatedAt,
                UpdatedAt = result.Value.UpdatedAt
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar detalhes do livro {Id}", id);
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar os detalhes do livro.";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Livro";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookViewModel viewModel)
    {
        ViewData["Title"] = "Novo Livro";
        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var createDto = new CreateBookDto
            {
                Title = viewModel.Title,
                Author = viewModel.Author,
                Subject = viewModel.Subject,
                Description = viewModel.Description
            };

            Result<BookDto?> createResult;
            if (viewModel.CoverImage != null && viewModel.CoverImage.Length > 0)
            {
                await using var fileStream = viewModel.CoverImage.OpenReadStream();
                createResult = await _bookApiClient.CreateBookAsync(
                    createDto,
                    fileStream,
                    viewModel.CoverImage.FileName,
                    viewModel.CoverImage.ContentType);
            }
            else
            {
                createResult = await _bookApiClient.CreateBookAsync(createDto);
            }

            if (!createResult.IsSuccess || createResult.Value == null)
            {
                TempData["ErrorMessage"] = createResult.Error ?? "Ocorreu um erro ao criar o livro.";
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Livro criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar livro");
            TempData["ErrorMessage"] = "Ocorreu um erro ao criar o livro. Por favor, tente novamente.";
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        ViewData["Title"] = "Editar Livro";
        try
        {
            var result = await _bookApiClient.GetBookByIdAsync(id);
            if (!result.IsSuccess || result.Value == null)
            {
                TempData["ErrorMessage"] = result.Error ?? "Livro não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookViewModel
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Author = result.Value.Author,
                Subject = result.Value.Subject,
                Description = result.Value.Description,
                CurrentCoverUrl = result.Value.CoverUrl
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar livro para edição {Id}", id);
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar o livro para edição.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateBookViewModel viewModel)
    {
        ViewData["Title"] = "Editar Livro";
        if (id != viewModel.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var updateDto = new UpdateBookDto
            {
                Title = viewModel.Title,
                Author = viewModel.Author,
                Subject = viewModel.Subject,
                Description = viewModel.Description
            };

            Result<BookDto?> updateResult;
            if (viewModel.CoverImage != null && viewModel.CoverImage.Length > 0)
            {
                await using var fileStream = viewModel.CoverImage.OpenReadStream();
                updateResult = await _bookApiClient.UpdateBookAsync(
                    id,
                    updateDto,
                    fileStream,
                    viewModel.CoverImage.FileName,
                    viewModel.CoverImage.ContentType);
            }
            else
            {
                updateResult = await _bookApiClient.UpdateBookAsync(id, updateDto);
            }

            if (!updateResult.IsSuccess)
            {
                TempData["ErrorMessage"] = updateResult.Error ?? "Ocorreu um erro ao atualizar o livro.";
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Livro atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar livro {Id}", id);
            TempData["ErrorMessage"] = "Ocorreu um erro ao atualizar o livro. Por favor, tente novamente.";
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _bookApiClient.DeleteBookAsync(id);
            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Livro excluído com sucesso!";
            else
                TempData["ErrorMessage"] = result.Error ?? "Ocorreu um erro ao excluir o livro.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir livro {Id}", id);
            TempData["ErrorMessage"] = "Ocorreu um erro ao excluir o livro.";
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> History(Guid id)
    {
        ViewData["Title"] = "Histórico de Versões";
        try
        {
            var bookResult = await _bookApiClient.GetBookByIdAsync(id);
            if (!bookResult.IsSuccess || bookResult.Value == null)
            {
                TempData["ErrorMessage"] = bookResult.Error ?? "Livro não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var historyResult = await _bookApiClient.GetBookHistoryAsync(id);
            if (!historyResult.IsSuccess || historyResult.Value == null)
            {
                TempData["ErrorMessage"] = historyResult.Error ?? "Ocorreu um erro ao carregar o histórico.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new BookHistoryViewModel
            {
                BookId = bookResult.Value.Id,
                BookTitle = bookResult.Value.Title,
                Versions = historyResult.Value.OrderByDescending(v => v.VersionNumber).Select(v => new BookVersionItemViewModel
                {
                    VersionNumber = v.VersionNumber,
                    Title = v.Title,
                    Author = v.Author,
                    Subject = v.Subject,
                    Description = v.Description,
                    CoverUrl = v.CoverUrl,
                    CreatedAt = v.CreatedAt
                }).ToList()
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar histórico do livro {Id}", id);
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar o histórico do livro.";
            return RedirectToAction(nameof(Index));
        }
    }
}
