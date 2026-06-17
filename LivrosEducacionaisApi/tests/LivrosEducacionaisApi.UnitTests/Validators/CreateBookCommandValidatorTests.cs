using LivrosEducacionaisApi.Application.Books.Commands;
using LivrosEducacionaisApi.Application.Books.Validators;
using FluentAssertions;
using Xunit;

namespace LivrosEducacionaisApi.UnitTests.Validators;

public class CreateBookCommandValidatorTests
{
    private readonly CreateBookCommandValidator _validator;

    public CreateBookCommandValidatorTests()
    {
        _validator = new CreateBookCommandValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateBookCommand(
            Title: "Valid Title",
            Author: "Valid Author",
            Subject: "Valid Subject",
            Description: "Valid Description"
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyTitle_ShouldFail(string title)
    {
        // Arrange
        var command = new CreateBookCommand(
            Title: title,
            Author: "Valid Author",
            Subject: "Valid Subject",
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Title));
    }

    [Fact]
    public void Validate_WithTooLongTitle_ShouldFail()
    {
        // Arrange
        var longTitle = new string('A', 201);
        var command = new CreateBookCommand(
            Title: longTitle,
            Author: "Valid Author",
            Subject: "Valid Subject",
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Title));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyAuthor_ShouldFail(string author)
    {
        // Arrange
        var command = new CreateBookCommand(
            Title: "Valid Title",
            Author: author,
            Subject: "Valid Subject",
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Author));
    }

    [Fact]
    public void Validate_WithTooLongAuthor_ShouldFail()
    {
        // Arrange
        var longAuthor = new string('A', 101);
        var command = new CreateBookCommand(
            Title: "Valid Title",
            Author: longAuthor,
            Subject: "Valid Subject",
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Author));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptySubject_ShouldFail(string subject)
    {
        // Arrange
        var command = new CreateBookCommand(
            Title: "Valid Title",
            Author: "Valid Author",
            Subject: subject,
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Subject));
    }

    [Fact]
    public void Validate_WithTooLongSubject_ShouldFail()
    {
        // Arrange
        var longSubject = new string('A', 101);
        var command = new CreateBookCommand(
            Title: "Valid Title",
            Author: "Valid Author",
            Subject: longSubject,
            Description: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Subject));
    }
}
