using LivrosEducacionaisApi.Application.DTOs;
using LivrosEducacionaisApi.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace LivrosEducacionaisApi.IntegrationTests.Controllers;

public class BooksControllerTests : IClassFixture<TestApplicationFactory>
{
    private readonly TestApplicationFactory _factory;
    private readonly HttpClient _client;

    public BooksControllerTests(TestApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task ResetDatabaseAsync()
    {
        var dbContext = _factory.GetDbContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task GetBooks_WhenNoBooksExist_ShouldReturnEmptyList()
    {
        // Arrange
        await ResetDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/api/v1/books");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        books.Should().NotBeNull();
        books.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateBook_WithValidData_ShouldCreateBook()
    {
        // Arrange
        await ResetDatabaseAsync();
        var newBook = new
        {
            Title = "Integration Test Book",
            Author = "Integration Test Author",
            Subject = "Integration Test Subject",
            Description = "Integration Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/books", newBook);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdBook = await response.Content.ReadFromJsonAsync<BookDto>();
        createdBook.Should().NotBeNull();
        createdBook.Id.Should().NotBeEmpty();
        createdBook.Title.Should().Be(newBook.Title);
        createdBook.Author.Should().Be(newBook.Author);
        createdBook.Subject.Should().Be(newBook.Subject);
        createdBook.Description.Should().Be(newBook.Description);

        // Verifica no banco de dados
        var dbContext = _factory.GetDbContext();
        var dbBook = await dbContext.Books.FindAsync(createdBook.Id);
        dbBook.Should().NotBeNull();
        dbBook.Title.Should().Be(newBook.Title);
    }

    [Fact]
    public async Task GetBookById_WhenBookExists_ShouldReturnBook()
    {
        // Arrange
        await ResetDatabaseAsync();
        var book = new Book("Test Book", "Test Author", "Test Subject");
        
        var dbContext = _factory.GetDbContext();
        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/v1/books/{book.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var bookDto = await response.Content.ReadFromJsonAsync<BookDto>();
        bookDto.Should().NotBeNull();
        bookDto.Id.Should().Be(book.Id);
        bookDto.Title.Should().Be(book.Title);
    }

    [Fact]
    public async Task UpdateBook_WithValidData_ShouldUpdateBook()
    {
        // Arrange
        await ResetDatabaseAsync();
        var book = new Book("Original Title", "Original Author", "Original Subject");
        
        var dbContext = _factory.GetDbContext();
        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync();

        var updateData = new
        {
            Title = "Updated Title",
            Author = "Updated Author",
            Subject = "Updated Subject",
            Description = "Updated Description"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/books/{book.Id}", updateData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verifica no banco
        dbContext = _factory.GetDbContext();
        var updatedBook = await dbContext.Books.FindAsync(book.Id);
        updatedBook.Should().NotBeNull();
        updatedBook.Title.Should().Be(updateData.Title);
        updatedBook.Version.Should().Be(2);
    }

    [Fact]
    public async Task DeleteBook_WhenBookExists_ShouldDeleteBook()
    {
        // Arrange
        await ResetDatabaseAsync();
        var book = new Book("Test Book", "Test Author", "Test Subject");
        
        var dbContext = _factory.GetDbContext();
        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/v1/books/{book.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica no banco
        dbContext = _factory.GetDbContext();
        var deletedBook = await dbContext.Books.FindAsync(book.Id);
        deletedBook.Should().NotBeNull();
        deletedBook.IsDeleted.Should().BeTrue();
    }
}
