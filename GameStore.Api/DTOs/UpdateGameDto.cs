using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record UpdateGameDto(
    [Required] string Title,
    [Range(1, 100)] int GenreId,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
);
