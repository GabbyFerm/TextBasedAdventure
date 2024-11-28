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

        Console.Write("Enter your name: ");
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
            Console.WriteLine("Please re-enter player details.");
            return CreateNewPlayer();
        }

        Console.WriteLine("Choose your class: Mage, Archer, Warrior");
        while (true)
        {
            string input = Console.ReadLine()!.Trim().ToLower();
            if (Enum.TryParse<Player.PlayerClass>(input, true, out var playerClass))
            {
                player.CurrentClass = playerClass;
                break;
            }
            Console.WriteLine("Invalid class. Please choose Mage, Archer, or Warrior.");
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
            Console.WriteLine($"Error loading players: {ex.Message}");
            return new List<Player>();
        }
    }
    public static Player LoadOrCreatePlayer()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=========== START MENU ===========");
            Console.WriteLine("1. Load an existing player");
            Console.WriteLine("2. Create a new player");
            Console.WriteLine("3. Exit game");
            Console.WriteLine("=================================");
            Console.Write("Choose an option (1/2/3): ");
            string choice = Console.ReadLine()!.Trim();

            if (choice == "1")
            {
                return LoadPlayer(); 
            }
            else if (choice == "2")
            {
                return CreateNewPlayer(); 
            }
            else if (choice == "3")
            {
                Console.WriteLine("Thank you for playing Dungeon Quest!");
                Environment.Exit(0); 
            }
            else
            {
                Console.WriteLine("Invalid input. Please choose 1, 2, or 3.");
                Console.ReadKey();
            }
        }
    }
    public static Player LoadPlayer()
    {
        AllPlayers = LoadAllPlayers();

        if (AllPlayers.Count == 0)
        {
            Console.WriteLine("\nNo saved players found, redirecting to Create new player...");
            Thread.Sleep(1500);
            return CreateNewPlayer();
        }

        Console.WriteLine("Saved players:");
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {AllPlayers[i].Name}");
        }

        Console.Write("Select a player by number: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= AllPlayers.Count)
            {
                return AllPlayers[index - 1];
            }
            else
            {
                Console.WriteLine("Invalid selection. Please choose a valid player number.");
            }
        }
    }
    public static void SavePlayer(Player player)
    {
        if (player == null)
        {
            Console.WriteLine("No player to save.");
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

            Console.WriteLine($"Player '{player.Name}' saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving player: {ex.Message}");
        }

        Thread.Sleep(1500);
    }

    public static void ShowBackstory()
    {
        Console.Clear();
        Console.WriteLine("In a world of shadows and peril, you are the last hope against ancient evils.");
        Console.WriteLine("Embark on a journey filled with danger, riddles, and glory!\n");
    }

    public static void ShowPlayerStats(Player player)
    {
        Console.Clear();
        if (player == null)
        {
            Console.WriteLine("Error: Player object is null!");
            return;
        }

        Console.WriteLine("==========================");
        Console.WriteLine($"Name: {player.Name}");
        Console.WriteLine($"Health: {player.Health}/{player.MaxHealth}");
        Console.WriteLine($"Power: {player.Power}");
        Console.WriteLine($"Armor: {player.Armor}");
        Console.WriteLine($"Coins: {player.Coins}");
        Console.WriteLine($"Potions: {player.Potions}");
        Console.WriteLine($"Experience: {player.Experience}");
        Console.WriteLine("==========================");
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }
}