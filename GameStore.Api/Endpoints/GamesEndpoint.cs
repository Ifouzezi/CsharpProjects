using GameStore.Api.Data;
using GameStore.Api.DTOs;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext
                    .Games.Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Title,
                        game.Genre!.Title,
                        game.Price,
                        game.ReleaseDate
                    ))
                    .AsNoTracking()
                    .ToListAsync()
        );

        // GET /games/{id}
        group
            .MapGet(
                "/{id}",
                async (int id, GameStoreContext dbContext) =>
                {
                    var game = await dbContext.Games.FindAsync(id);

                    return game is null
                        ? Results.NotFound()
                        : Results.Ok(
                            new GamesDetailsDto(
                                game.Id,
                                game.Title,
                                game.GenreId,
                                game.Price,
                                game.ReleaseDate
                            )
                        );
                }
            )
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost(
            "/",
            async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Title = newGame.Title,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate,
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

                return Results.CreatedAtRoute(
                    GetGameEndpointName,
                    new { id = gamesDetailsDto.Id },
                    gamesDetailsDto
                );
            }
        );

        // PUT /games/{id}
        group.MapPut(
            "/{id}",
            async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Title = updatedGame.Title;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.Price = updatedGame.Price;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        );

        // DELETE /games/{id}
        group.MapDelete(
            "/{id}",
            async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            }
        );
    }
}
