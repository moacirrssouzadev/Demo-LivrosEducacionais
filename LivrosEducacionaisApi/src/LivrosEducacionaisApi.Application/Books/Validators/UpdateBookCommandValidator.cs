using LivrosEducacionaisApi.Application.Books.Commands;
using FluentValidation;

namespace LivrosEducacionaisApi.Application.Books.Validators;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CoverFile)
            .Must(coverFile => CoverFileRules.Validate(coverFile) is null)
            .WithMessage(command => CoverFileRules.Validate(command.CoverFile) ?? string.Empty);
    }
}
