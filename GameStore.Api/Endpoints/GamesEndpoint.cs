using GameStore.Api.Data;
using GameStore.Api.DTOs;
using GameStore.Api.Models;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = 
    [
        new (1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new (2, "Super Mario Odyssey", "Platformer", 49.99m, new DateOnly(2017, 10, 27)),
        new (3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
        new (4, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19)),
        new (5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>

        {
            var game = games.Find(g => g.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName);
        
        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
        {
            
            Game game = new()
            {
                Title = newGame.Title,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GamesDetailsDto gamesDetailsDto = new(
                    game.Id,
                    game.Title,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gamesDetailsDto.Id }, gamesDetailsDto);
            });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => 
        {
            var index = games.FindIndex(g => g.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            
            games[index] = new GameDto(
                id,
                updatedGame.Title,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) => 
        {
            var index = games.FindIndex(g => g.Id == id);
            games.RemoveAt(index);
            return Results.NoContent();
        });
    }
}