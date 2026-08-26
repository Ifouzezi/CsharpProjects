using GameStore.Api.DTOs;

const string GetGameEndpointName = "GetGame";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameDto(2, "Super Mario Odyssey", "Platformer", 49.99m, new DateOnly(2017, 10, 27)),
    new GameDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    new GameDto(4, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new GameDto(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.FirstOrDefault(g => g.Id == id))
    .WithName(GetGameEndpointName);
 
// POST /games
app.MapPost("/games", (CreateGameDto newGame) => {
   
   GameDto game = new(
    games.Count + 1,
    newGame.Title,
    newGame.Genre,
    newGame.Price,
    newGame.ReleaseDate
   );
   
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) => 
{
    var index = games.FindIndex(g => g.Id == id);
    
    games[index] = new GameDto(
        id,
        updatedGame.Title,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.Run(); 
