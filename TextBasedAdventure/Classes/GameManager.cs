using Spectre.Console;
using Figgle;
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
        var header = FiggleFonts.Slant.Render("Dungeon Quest");
        AnsiConsole.Markup($"[plum4]{header}[/]");

        AnsiConsole.Markup("\n[springgreen4]The Dark Dungeon awaits...[/]\n\n");
        AnsiConsole.Markup("[springgreen4]A long time ago, in a land filled with mystery and danger, dark forces began to rise.[/]\n");
        AnsiConsole.Markup("[springgreen4]Heroes from all over the world gathered to face the darkness that threatened to engulf everything.[/]\n");
        AnsiConsole.Markup("[springgreen4]Now, it's your turn to step into the fray. Will you be the one to stop the evil that lurks in the shadows?[/]\n");
        AnsiConsole.Markup("[springgreen4]The adventure begins now...[/]\n\n");

        AnsiConsole.Markup("[italic grey53]Press any key to begin...[/]");
        Console.ReadKey();
    }
    public static void ShowMainMenu(Player player)
    {
        if (player == null)
        {
            AnsiConsole.Markup("[red]Error: No current player found![/]\n");
            return; 
        }

        EncounterManager encounterManager = new EncounterManager();
        var menuValidator = new MenuChoiceValidator(1, 6);

        while (true)
        {
            Console.Clear();
            AnsiConsole.Markup("[plum4]-------- MAIN MENU --------[/]\n\n");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[italic grey53]Choose an option:[/]")
                    .PageSize(6)
                    .HighlightStyle(new Style(foreground: Color.SpringGreen4)) 
                    .AddChoices(new[]
                    {
                    "[plum4]1. Start your quest[/]",
                    "[plum4]2. Visit the shop[/]",
                    "[plum4]3. Check player stats[/]",
                    "[plum4]4. Save player[/]",
                    "[plum4]5. Back to Start Menu[/]",
                    "[plum4]6. Exit game[/]"
                    }));

            var selectedOption = choice.Replace("[plum4]", "").Split('.')[0].Trim();

            if (!menuValidator.Validate(selectedOption).IsValid)
            {
                AnsiConsole.Markup("[red]Invalid choice. Please select a valid option![/]\n");
                Console.ReadKey();
                continue;
            }

            switch (selectedOption)
            {
                case "1":
                    new EncounterManager().StartEncounter(player);
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
                        AnsiConsole.Markup("[red]No player to save.[/]\n");
                        Console.ReadKey();
                    }
                    break;
                case "5":
                    CurrentPlayer = PlayerManager.LoadOrCreatePlayer();
                    ShowMainMenu(CurrentPlayer);
                    return;
                case "6":
                    AnsiConsole.Markup("[purple]Thank you for playing Dungeon Quest![/]\n");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}