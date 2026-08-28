using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record UpdateGameDto
(
    [Required] string Title,
    [Required] string Genre,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
);

