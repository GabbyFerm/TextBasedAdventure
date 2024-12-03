using Spectre.Console;
using Newtonsoft.Json;
using System.Numerics;
using System.Xml.Linq;
using TextBasedAdventure.Classes;
using TextBasedAdventure.Validators;

public static class PlayerManager
{
    private static Player? CurrentPlayer;

    public static void SetCurrentPlayer(Player player)
    {
        CurrentPlayer = player;
    }
    public static Player? GetCurrentPlayer()
    {
        return CurrentPlayer;
    }
    private static List<Player> AllPlayers = new();

    public static Player CreateNewPlayer()
    {
        ShowBackstory();

        AnsiConsole.Markup("[springgreen4]Enter your name:[/]\n");
        string playerName = Console.ReadLine()!.Trim();

        var player = new Player(playerName);

        var validator = new PlayerValidator();
        var validationResult = validator.Validate(player);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            AnsiConsole.Markup("[red]Please re-enter player details.[/]\n");
            return CreateNewPlayer();
        }

        AnsiConsole.Markup("[springgreen4]Choose your class: Mage, Archer, Warrior[/]\n");
        while (true)
        {
            string input = Console.ReadLine()!.Trim().ToLower();
            if (Enum.TryParse<Player.PlayerClass>(input, true, out var playerClass))
            {
                player.CurrentClass = playerClass;
                break;
            }
            AnsiConsole.Markup("[red]Invalid class. Please choose Mage, Archer, or Warrior.[/]\n");
        }

        return player;
    }

    public static List<Player> LoadAllPlayers()
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json");
        string filePath = Path.Combine(folderPath, "players.json");

        if (!File.Exists(filePath))
        {
            return new List<Player>();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            var players = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

            return players;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]Error loading players: {ex.Message}[/]\n");
            return new List<Player>();
        }
    }
    public static Player LoadOrCreatePlayer()
    {
        if (CurrentPlayer != null)
        {
            return CurrentPlayer;
        }

        while (true)
        {
            Console.Clear();

            AnsiConsole.Markup("[plum4]-------- START MENU --------[/]\n\n");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[italic grey53]Choose an option with arrowkeys up/down:[/]")
                    .PageSize(3)
                    .HighlightStyle(new Style(Color.SpringGreen4))
                    .AddChoices(new[]
                    {
                    "[plum4]1. Load an existing player[/]",
                    "[plum4]2. Create a new player[/]",
                    "[plum4]3. Exit game[/]"
                    }));

            var selectedOption = choice.Replace("[plum4]", "").Split('.')[0].Trim();

            switch (selectedOption)
            {
                case "1":
                    Player selectedPlayer = LoadPlayer();
                    if (selectedPlayer != null)
                    {
                        AnsiConsole.Clear();  
                        AnsiConsole.Markup($"\n\n[italic plum4]Player {selectedPlayer.Name} the mighty {selectedPlayer.CurrentClass}, loaded successfully.[/]\n\n");
                        AnsiConsole.Markup("[italic grey53]Press enter to continue...[/]");
                        Console.ReadLine();
                        GameManager.ShowMainMenu(selectedPlayer);
                        return selectedPlayer;
                    }
                    break;

                case "2":
                    return CreateNewPlayer();

                case "3":
                    AnsiConsole.Markup("[plum4]Thank you for playing Dungeon Quest![/]\n");
                    Environment.Exit(0);
                    break;

                default:
                    AnsiConsole.Markup("[red]Invalid input. Please choose a valid option.[/]\n");
                    Console.ReadKey();
                    break;
            }
        }
    }
    public static Player LoadPlayer()
    {
        AllPlayers = LoadAllPlayers();

        if (AllPlayers.Count == 0)
        {
            AnsiConsole.Markup("\n[red]No saved players found, redirecting to Create new player...[/]");
            Thread.Sleep(1500);
            return CreateNewPlayer();
        }

        var playerMenu = new SelectionPrompt<string>()
            .Title("[italic grey53]Saved players, choose with arrowkeys up/down:[/]")
            .PageSize(10)
            .HighlightStyle(new Style(Color.SpringGreen4));

        foreach (var player in AllPlayers)
        {
            playerMenu.AddChoice($"[plum4]{player.Name}[/]");
        }

        string selectedPlayerName = AnsiConsole.Prompt(playerMenu);

        selectedPlayerName = selectedPlayerName.Replace("[plum4]", "").Replace("[/]", "").Trim();

        var selectedPlayer = AllPlayers.FirstOrDefault(player => player.Name.Equals(selectedPlayerName, StringComparison.OrdinalIgnoreCase));

        if (selectedPlayer == null)
        {
            AnsiConsole.Markup("[red]Error: Selected player not found.[/]\n");
            return CreateNewPlayer();
        }

        return selectedPlayer;
    }

    public static void SavePlayer(Player player)
    {
        if (player == null)
        {
            AnsiConsole.Markup("[red]No player to save.[/]\n");
            return;
        }

        try
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json");
            string filePath = Path.Combine(folderPath, "players.json");

            Directory.CreateDirectory(folderPath);

            List<Player> allPlayers = File.Exists(filePath)
                ? JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(filePath)) ?? new List<Player>()
                : new List<Player>();

            var existingPlayer = allPlayers.FirstOrDefault(p => p.Name == player.Name);
            if (existingPlayer != null)
            {
                allPlayers.Remove(existingPlayer);
            }
            allPlayers.Add(player);

            File.WriteAllText(filePath, JsonConvert.SerializeObject(allPlayers, Formatting.Indented));

            AnsiConsole.Markup($"[red]Player '{player.Name}' saved successfully![/]\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]Error saving player: {ex.Message}[/]\n");
        }

        Thread.Sleep(1500);
    }

    public static void ShowBackstory()
    {
        Console.Clear();
        AnsiConsole.Markup("\n[springgreen4]In a world of shadows and peril, you are the last hope against ancient evils.[/]");
        AnsiConsole.Markup("\n[springgreen4]Embark on a journey filled with danger, riddles, and glory!\n\n[/]");
    }

    public static void ShowPlayerStats(Player player)
    {
        Console.Clear();
        if (player == null)
        {
            AnsiConsole.Markup($"[red]Error: Player object is null![/]\n");
            return;
        }
        AnsiConsole.Markup("[plum4]------- PLAYER STATS -------[/]\n");
        AnsiConsole.Markup($"[plum4]Name: {player.Name}[/]\n");
        AnsiConsole.Markup($"[plum4]Health: {player.Health}/{player.MaxHealth}[/]\n");
        AnsiConsole.Markup($"[plum4]Power: {player.Power}[/]\n");
        AnsiConsole.Markup($"[plum4]Armor: {player.Armor}[/]\n");
        AnsiConsole.Markup($"[plum4]Coins: {player.Coins}[/]\n");
        AnsiConsole.Markup($"[plum4]Potions: {player.Potions}[/]\n");
        AnsiConsole.Markup($"[plum4]Experience: {player.Experience}[/]\n");
        AnsiConsole.Markup("[plum4]-----------------------------[/]\n");
        AnsiConsole.Markup("\n[italic grey53]Press any key to return to the main menu...[/]\n");
        Console.ReadKey();
    }
}