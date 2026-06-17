using LivrosEducacionaisApi.Application.Books.Commands;

namespace LivrosEducacionaisApi.Application.Books;

public static class CoverFileRules
{
    public static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "image/webp" };
    public const int MaxFileSize = 5 * 1024 * 1024;

    public static string? Validate(BookCoverFile? coverFile)
    {
        if (coverFile is null)
            return null;

        if (coverFile.FileData.Length == 0)
            return "Cover image is required";

        if (coverFile.FileData.Length > MaxFileSize)
            return $"File size must not exceed {MaxFileSize / 1024 / 1024}MB";

        if (!AllowedContentTypes.Contains(coverFile.ContentType))
            return "Only JPEG, PNG, and WEBP files are allowed";

        return null;
    }
}
