using LivrosEducacionais.Dtos;
using LivrosEducacionais.Results;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LivrosEducacionais.Services;

public class BookApiClient : IBookApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public BookApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<Result<PaginatedBooksDto>> GetBooksAsync(int pageNumber = 1, int pageSize = 10)
    {
        var queryParams = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        var queryString = string.Join("&", queryParams);
        var response = await _httpClient.GetAsync($"api/v1/books?{queryString}");
        return await ReadResponseAsync<PaginatedBooksDto>(response);
    }

    public async Task<Result<BookDto?>> GetBookByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/v1/books/{id}");
        return await ReadResponseAsync<BookDto?>(response);
    }

    public async Task<Result<BookDto?>> CreateBookAsync(CreateBookDto book, Stream? coverStream = null, string? coverFileName = null, string? coverContentType = null)
    {
        using var content = BuildBookFormContent(book.Title, book.Author, book.Subject, book.Description, coverStream, coverFileName, coverContentType);
        var response = await _httpClient.PostAsync("api/v1/books", content);
        return await ReadBookResponseAsync(response);
    }

    public async Task<Result<BookDto?>> UpdateBookAsync(Guid id, UpdateBookDto book, Stream? coverStream = null, string? coverFileName = null, string? coverContentType = null)
    {
        using var content = BuildBookFormContent(book.Title, book.Author, book.Subject, book.Description, coverStream, coverFileName, coverContentType);
        var response = await _httpClient.PutAsync($"api/v1/books/{id}", content);
        return await ReadBookResponseAsync(response);
    }

    public async Task<Result> DeleteBookAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/books/{id}");
        if (response.IsSuccessStatusCode)
            return new Result { IsSuccess = true };

        var error = await response.Content.ReadAsStringAsync();
        return new Result { IsSuccess = false, Error = error };
    }

    public async Task<Result<List<BookVersionDto>>> GetBookHistoryAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/v1/books/{id}/history");
        return await ReadResponseAsync<List<BookVersionDto>>(response);
    }

    private static MultipartFormDataContent BuildBookFormContent(
        string title,
        string author,
        string subject,
        string? description,
        Stream? coverStream,
        string? coverFileName,
        string? coverContentType)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(title), "Title" },
            { new StringContent(author), "Author" },
            { new StringContent(subject), "Subject" }
        };

        if (!string.IsNullOrWhiteSpace(description))
            content.Add(new StringContent(description), "Description");

        if (coverStream is not null && !string.IsNullOrWhiteSpace(coverFileName))
        {
            var fileContent = new StreamContent(coverStream);
            if (!string.IsNullOrWhiteSpace(coverContentType))
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(coverContentType);

            content.Add(fileContent, "CoverImage", coverFileName);
        }

        return content;
    }

    private async Task<Result<BookDto?>> ReadBookResponseAsync(HttpResponseMessage response)
    {
        return await ReadResponseAsync<BookDto?>(response);
    }

    private async Task<Result<T>> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return new Result<T> { IsSuccess = false, Error = await ReadErrorAsync(response) };
        }

        var value = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        return value is null
            ? new Result<T> { IsSuccess = false, Error = "Unknown error" }
            : new Result<T> { IsSuccess = true, Value = value };
    }

    private async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        var error = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(error))
            return response.ReasonPhrase ?? "Unknown error";

        try
        {
            return JsonSerializer.Deserialize<string>(error, _jsonOptions) ?? error;
        }
        catch (JsonException)
        {
            return error;
        }
    }
}
