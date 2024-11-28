using TextBasedAdventure.Classes;
using TextBasedAdventure.Validators;

public class GameManager
{
    private static Player? CurrentPlayer;

    public static void StartGame()
    {
        ShowIntro();
        CurrentPlayer = PlayerManager.LoadOrCreatePlayer();
        ShowMainMenu(CurrentPlayer);
    }
    public static void ShowIntro()
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
    public static void ShowMainMenu(Player player)
    {
        EncounterManager encounterManager = new EncounterManager();
        var menuValidator = new MenuChoiceValidator(1, 5);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=========== MAIN MENU ===========");
            Console.WriteLine("1. Start your quest");
            Console.WriteLine("2. Visit the shop");
            Console.WriteLine("3. Check player stats");
            Console.WriteLine("4. Save player");
            Console.WriteLine("5. Back to Start Menu");
            Console.WriteLine("6. Exit game");
            Console.WriteLine("=================================");
            Console.Write("Choose an option (1/2/3/4/5/6): ");
            string menuChoice = Console.ReadLine()!;

            var result = menuValidator.Validate(menuChoice);

            if (!result.IsValid)
            {
                Console.WriteLine(result.Errors[0].ErrorMessage);
                Console.ReadKey();
                continue;
            }

            switch (menuChoice)
            {
                case "1":
                    encounterManager.StartEncounter(player);
                    break;
                case "2":
                    Shop.LoadShop(player);
                    break;
                case "3":
                    PlayerManager.ShowPlayerStats(player);
                    break;
                case "4":
                    if (CurrentPlayer != null)
                    {
                        PlayerManager.SavePlayer(CurrentPlayer); 
                    }
                    else
                    {
                        Console.WriteLine("No player to save.");
                    }
                    break;
                case "5":  
                    player = PlayerManager.LoadOrCreatePlayer();
                    return;
                case "6":
                    Console.WriteLine("Thank you for playing Dungeon Quest!");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}