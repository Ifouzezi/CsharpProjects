using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record CreateGameDto
(
    int Id,
    [Required] string Title,
    [Required] string Genre,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
)
{
    public string? Name { get; internal set; }
}