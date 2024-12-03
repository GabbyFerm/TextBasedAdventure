using Spectre.Console;
using System;
using TextBasedAdventure.Validators;

namespace TextBasedAdventure.Classes
{
    public class Shop
    {
        public static void LoadShop(Player player) => RunShop(player);

        private static void RunShop(Player player)
        {
            while (true)
            {
                int weaponPrice = CalculatePrice("weapon", player);
                int armorPrice = CalculatePrice("armor", player);
                int potionPrice = CalculatePrice("potion", player);

                Console.Clear();
                AnsiConsole.Markup("[springgreen4]You enter the mysterious shop. The shopkeeper eyes you with suspicion.[/]\n");
                AnsiConsole.Markup("[springgreen4]‘I’ve heard of you,’ the shopkeeper says. ‘You’re the one who’s been causing a stir in the dungeon…’[/]\n");
                AnsiConsole.Markup("\n[springgreen4]Do you wish to buy something?[/]\n");

                AnsiConsole.Markup("\n[grey53]       PLAYER STATS     [/]\n");
                AnsiConsole.Markup("[grey53]========================[/]\n");
                AnsiConsole.Markup($"[grey53]Current health: {player.Health}[/]\n");
                AnsiConsole.Markup($"[grey53]Coins: {player.Coins}[/]\n");
                AnsiConsole.Markup($"[grey53]Weapon strength: {player.Power}[/]\n");
                AnsiConsole.Markup($"[grey53]Armor toughness: {player.Armor}[/]\n");
                AnsiConsole.Markup($"[grey53]Potions: {player.Potions}[/]\n");
                AnsiConsole.Markup("[grey53]========================[/]\n\n");

                AnsiConsole.Markup("[plum4]-------- SHOP MENU --------[/]\n\n");

                var shopMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[italic grey53]Choose what you want to buy or exit:[/]")
                    .PageSize(4)
                    .HighlightStyle(new Style(foreground: Color.SpringGreen4))  
                    .AddChoices(
                           $"[plum4]Weapon: ${weaponPrice}[/]",
                        $"[plum4]Armor: ${armorPrice}[/]",
                        $"[plum4]Potion: ${potionPrice}[/]",
                        "[plum4]Exit[/]"
                    )
                );

                if (shopMenu == "[plum4]Exit[/]")
                {
                    break;  
                }

                string selectedItem = shopMenu.Replace("[plum4]", "").Trim();  

                string itemType = selectedItem.Split(':')[0].Trim().ToLower();  
                int price = itemType switch
                {
                    "weapon" => weaponPrice,
                    "armor" => armorPrice,
                    "potion" => potionPrice,
                    _ => throw new ArgumentException($"Invalid item type: {selectedItem}")  
                };

                TryToBuy(itemType, price, player);
            }
        }
        private static int CalculatePrice(string itemType, Player player)
        {
            return itemType switch
            {
                "weapon" => Math.Min(300, 50 * (player.Power + 1)),
                "armor" => Math.Min(300, 50 * (player.Armor + 1)),
                "potion" => Math.Min(50, 20 + 5 * player.Potions),
                _ => throw new ArgumentException("Invalid item type.")
            };
        }
        private static void TryToBuy(string item, int cost, Player player)
        {
            if (!new[] { "weapon", "armor", "potion" }.Contains(item))
            {
                AnsiConsole.Markup("\n[italic red]Invalid item type.[/]\n");
                Thread.Sleep(1000); 
                return;
            }

            if (player.Coins < cost)
            {
                AnsiConsole.Markup("\n[italic red]You don't have enough coins.[/]\n");
                Thread.Sleep(1000); 
            }
            else if (item == "weapon" && player.Power >= 30)
            {
                AnsiConsole.Markup("\n[italic red]Your weapon is already at maximum strength.[/]\n");
                Thread.Sleep(1000); 
            }
            else if (item == "armor" && player.Armor >= 30)
            {
                AnsiConsole.Markup("\n[italic red]Your armor is already at maximum toughness.[/]\n");
                Thread.Sleep(1000); 
            }
            else
            {
                player.DecreaseCoins(cost);

                if (item == "weapon")
                    player.AddPower(1);
                else if (item == "armor")
                    player.AddArmor(1);
                else if (item == "potion")
                    player.AddPotion(1);

                AnsiConsole.Markup($"[italic plum4]You purchased {item} for {cost} coins.[/]\n");
                Thread.Sleep(1000); 
            }
        }
    }
}