namespace GameStore.Api.DTOs;

public record CreateGameDto
(
    int Id,
    string Title,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);


