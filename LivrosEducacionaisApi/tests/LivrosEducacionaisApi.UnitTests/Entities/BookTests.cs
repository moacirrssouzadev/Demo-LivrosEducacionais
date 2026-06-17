using FluentAssertions;
using LivrosEducacionaisApi.Domain.Entities;
using Xunit;

namespace LivrosEducacionaisApi.UnitTests.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateBook()
    {
        // Arrange
        var title = "Test Book";
        var author = "Test Author";
        var subject = "Test Subject";
        var description = "Test Description";

        // Act
        var book = new Book(title, author, subject, description);

        // Assert
        book.Should().NotBeNull();
        book.Title.Should().Be(title);
        book.Author.Should().Be(author);
        book.Subject.Should().Be(subject);
        book.Description.Should().Be(description);
        book.Version.Should().Be(1);
        book.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Update_WithValidParameters_ShouldUpdateBookAndIncreaseVersion()
    {
        // Arrange
        var book = new Book("Old Title", "Old Author", "Old Subject", "Old Description");
        var initialVersion = book.Version;
        var newTitle = "New Title";
        var newAuthor = "New Author";
        var newSubject = "New Subject";
        var newDescription = "New Description";

        // Act
        book.Update(newTitle, newAuthor, newSubject, newDescription);

        // Assert
        book.Title.Should().Be(newTitle);
        book.Author.Should().Be(newAuthor);
        book.Subject.Should().Be(newSubject);
        book.Description.Should().Be(newDescription);
        book.Version.Should().Be(initialVersion + 1);
        book.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetCoverUrl_WithValidUrl_ShouldSetCoverUrl()
    {
        // Arrange
        var book = new Book("Test Book", "Test Author", "Test Subject");
        var coverUrl = "https://example.com/cover.jpg";

        // Act
        book.SetCoverUrl(coverUrl);

        // Assert
        book.CoverUrl.Should().Be(coverUrl);
        book.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Delete_WhenCalled_ShouldMarkAsDeleted()
    {
        // Arrange
        var book = new Book("Test Book", "Test Author", "Test Subject");

        // Act
        book.Delete();

        // Assert
        book.IsDeleted.Should().BeTrue();
        book.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
