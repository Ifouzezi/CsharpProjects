namespace GameStore.Api.DTOs;

public record GamesDetailsDto
(
    int Id,
    string Title,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
