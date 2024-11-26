using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using TextBasedAdventure.Classes;

public class Program
{
    public static Player? CurrentPlayer { get; private set; }
    private static EncounterManager encounterManager = new EncounterManager();

    public static void Main(string[] args)
    {
        StartGame();

        CurrentPlayer = LoadOrCreatePlayer();

        if (CurrentPlayer != null)
        {
            ShowMainMenu(CurrentPlayer);
        }
        else
        {
            Console.WriteLine("Error: Could not load or create player.");
        }
    }
    public static void StartGame()
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("          DUNGEON QUEST           ");
        Console.WriteLine("===================================");
        Console.WriteLine("The Dark Dungeon awaits...");
        Console.WriteLine("\nA long time ago, in a land filled with mystery and danger, dark forces began to rise.");
        Console.WriteLine("Heroes from all over the world gathered to face the darkness that threatened to engulf everything.");
        Console.WriteLine("Now, it's your turn to step into the fray. Will you be the one to stop the evil that lurks in the shadows?");
        Console.WriteLine("The adventure begins now...");
        Console.WriteLine("===================================");
        Console.WriteLine("\nPress any key to begin...");
        Console.ReadKey();
    }
    public static Player LoadOrCreatePlayer()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Would you like to:");
            Console.WriteLine("1. Load an existing player");
            Console.WriteLine("2. Create a new player");
            Console.Write("Choose an option (1/2): ");
            string choice = Console.ReadLine()!.Trim();

            if (choice == "1")
            {
                return LoadPlayer();
            }
            else if (choice == "2")
            {
                return NewStart();
            }
            else
            {
                Console.WriteLine("Invalid input. Please choose 1 or 2.");
                Console.ReadKey();
            }
        }
    }
    public static void ShowMainMenu(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=========== MAIN MENU ===========");
            Console.WriteLine("1. Start your quest");
            Console.WriteLine("2. Visit the shop");
            Console.WriteLine("3. Check player stats");
            Console.WriteLine("4. Save game");
            Console.WriteLine("5. Exit game");
            Console.WriteLine("=================================");

            Console.Write("\nChoose an option: ");
            string menuChoice = Console.ReadLine()!;

            switch (menuChoice)
            {
                case "1":
                    var encounterManager = new EncounterManager();
                    encounterManager.StartEncounter(player);
                    break;
                case "2":
                    Shop.LoadShop(player);
                    break;
                case "3":
                    DisplayPlayerStats(player);
                    break;
                case "4":
                    SavePlayer();
                    break;
                case "5":
                    Console.WriteLine("Thank you for playing Dungeon Quest!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    public static Player NewStart()
    {
        DisplayBackstory();

        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine()!.Trim();

        Player player = new Player(playerName);

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

        Console.Clear();
        Console.WriteLine($"Welcome {player.Name}, a brave {player.CurrentClass} with {player.Health} health, {player.Power} power, {player.Coins} coins, and {player.Potions} potions!");
        Console.WriteLine("\nPress enter to continue...");
        Console.ReadKey();

        CurrentPlayer = player;
        return player;
    }

    public static void DisplayBackstory()
    {
        Console.Clear();
        Console.WriteLine("In a world of shadows and peril, you are the last hope against ancient evils.");
        Console.WriteLine("Embark on a journey filled with danger, riddles, and glory!\n");
    }

    public static List<Player> AllPlayers = new List<Player>();

    public static void SavePlayer()
    {
        if (CurrentPlayer == null)
        {
            Console.WriteLine("No player to save.");
            return;
        }

        try
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json");
            string filePath = Path.Combine(folderPath, "players.json");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                AllPlayers = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();
            }

            var existingPlayer = AllPlayers.FirstOrDefault(p => p.Name == CurrentPlayer.Name);
            if (existingPlayer != null)
            {
                AllPlayers.Remove(existingPlayer);
            }
            AllPlayers.Add(CurrentPlayer);

            string updatedJson = JsonConvert.SerializeObject(AllPlayers, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);

            Console.WriteLine($"Player '{CurrentPlayer.Name}' saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving player: {ex.Message}");
        }

        Thread.Sleep(1500);
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
            return JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading players: {ex.Message}");
            return new List<Player>();
        }
    }
    public static Player LoadPlayer()
    {
        AllPlayers = LoadAllPlayers();

        if (AllPlayers.Count == 0)
        {
            Console.WriteLine("\nNo saved players found, redirecting to Create new player...");
            Thread.Sleep(1500);
            return NewStart();
        }

        Console.WriteLine("Saved players:");
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {AllPlayers[i].Name}");
        }

        Console.Write("Select a player by number: ");
        while (true)
        {
            string input = Console.ReadLine()!;
            if (int.TryParse(input, out int index) && index >= 1 && index <= AllPlayers.Count)
            {
                CurrentPlayer = AllPlayers[index - 1];
                Console.WriteLine($"Player '{CurrentPlayer.Name}' loaded successfully!");
                return CurrentPlayer;
            }
            else
            {
                Console.WriteLine("Invalid selection. Please choose a valid player number.");
            }
        }
    }
    private static void DisplayPlayerStats(Player player)
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