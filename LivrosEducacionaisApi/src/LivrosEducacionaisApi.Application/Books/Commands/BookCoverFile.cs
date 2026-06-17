namespace LivrosEducacionaisApi.Application.Books.Commands;

public record BookCoverFile(byte[] FileData, string FileName, string ContentType);
