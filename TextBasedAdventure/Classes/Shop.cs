using System;

namespace TextBasedAdventure.Classes
{
    public class Shop
    {
        public static void LoadShop(Player player) => RunShop(player);

        private static void RunShop(Player player)
        {
            while (true)
            {
                int potionPrice = Math.Min(50, 20 + 5 * player.Potions);
                int armorPrice = Math.Min(300, 50 * (player.Armor + 1));
                int weaponPrice = Math.Min(300, 50 * (player.Power + 1));

                Console.Clear();
                Console.WriteLine("You enter the mysterious shop. The shopkeeper eyes you with suspicion.");
                Console.WriteLine("‘I’ve heard of you,’ the shopkeeper says. ‘You’re the one who’s been causing a stir in the dungeon…’");
                Console.WriteLine("\nDo you wish to buy something?");

                Console.WriteLine("           SHOP        ");
                Console.WriteLine("========================");
                Console.WriteLine($"(W)eapon:            ${weaponPrice}");
                Console.WriteLine($"(A)rmor:             ${armorPrice}");
                Console.WriteLine($"(P)otion:            ${potionPrice}");
                Console.WriteLine("(E)xit");
                Console.WriteLine("========================\n");

                Console.WriteLine("\n       PLAYER STATS     ");
                Console.WriteLine("========================");
                Console.WriteLine($"Current health: {player.Health}");
                Console.WriteLine($"Coins: {player.Coins}");
                Console.WriteLine($"Weapon strength: {player.Power}");
                Console.WriteLine($"Armor toughness: {player.Armor}");
                Console.WriteLine($"Potions: {player.Potions}");
                Console.WriteLine("========================");

                Console.Write("\nEnter your choice: ");
                string input = Console.ReadLine()!.ToLower();

                if (input == "w" || input == "weapon")
                    TryToBuy("weapon", weaponPrice, player);
                else if (input == "a" || input == "armor")
                    TryToBuy("armor", armorPrice, player);
                else if (input == "p" || input == "potion")
                    TryToBuy("potion", potionPrice, player);
                else if (input == "e" || input == "exit")
                    break;
                else
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadKey();
            }
        }
        private static void TryToBuy(string item, int cost, Player player)
        {
            if (player.Coins >= cost)
            {
                player.DecreaseCoins(cost);  
                if (item == "weapon")
                    player.AddPower(1);
                else if (item == "armor")
                    player.AddArmor(1);
                else if (item == "potion")
                    player.AddPotion(1);

                Console.WriteLine($"You purchased {item} for {cost} coins.");
            }
            else
            {
                Console.WriteLine("You don't have enough coins.");
            }
            Console.ReadKey();
        }
    }
}