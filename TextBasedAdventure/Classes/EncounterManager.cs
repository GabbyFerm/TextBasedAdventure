using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventure.Classes
{
    public class EncounterManager
    {
        private Random rand;

        private int lastEncounterType = -1;

        public EncounterManager() 
        { 
            rand = new Random();
        }

        public void StartEncounter(Player player)
        {
            Console.Clear();
            int encounterType = rand.Next(0, 2);

            if (encounterType == lastEncounterType)
            {
                encounterType = 1 - lastEncounterType;  
            }

            if (encounterType == 0)
            {
                var enemy = GenerateRandomEnemy();
                Combat(player, enemy);
            }
            else
            {
                StartRiddleEncounter(player);
            }

            lastEncounterType = encounterType;
        }

        private Enemy GenerateRandomEnemy()
        {
            int enemyType = rand.Next(0, 4); 
            switch (enemyType)
            {
                case 0:
                    return new Enemy("Skeleton", 40, 5);
                case 1:
                    return new Enemy("Zombie", 45, 6);
                case 2:
                    return new Enemy("Human Cultist", 55, 7);
                case 3:
                    return new WizardEnemy();
                default:
                    return new Enemy("Grave Robber", 35, 4);
            }
        }

        private void StartRiddleEncounter(Player player)
        {
            var riddles = new List<Riddle>
            {
                new Riddle("What has keys but can't open locks?", "Piano"),
                new Riddle("I speak without a mouth and hear without ears. I have no body, but I come alive with wind. What am I?", "Echo"),
                new Riddle("The more of this there is, the less you see. What is it?", "Darkness"),
                new Riddle("I’m light as a feather, yet the strongest person can’t hold me for five minutes. What am I?", "Breath"),
                new Riddle("What can travel around the world while staying in the same corner?", "Stamp"),
                new Riddle("What has to be broken before you can use it?", "Egg")
            };

            int riddleIndex = rand.Next(riddles.Count); 
            var selectedRiddle = riddles[riddleIndex];

            Console.WriteLine($"Riddle: {selectedRiddle.Question}");
            Console.Write("Your answer: ");
            string playerAnswer = Console.ReadLine()!;

            if (playerAnswer.Equals(selectedRiddle.Answer, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Correct! You may continue on your journey.");
                GrantReward(player); 
            }
            else
            {
                Console.WriteLine($"Incorrect. The correct answer was: {selectedRiddle.Answer}");
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        private void Combat(Player player, Enemy enemy)
        {
            Console.WriteLine($"A wild {enemy.Name} appears!");

            if (enemy is WizardEnemy wizardEnemy)
            {
                Console.WriteLine($"You step into a dimly lit room and encounter a wise wizard...");
                Thread.Sleep(1500);
            }

            while (player.Health > 0 && enemy.IsAlive)
            {
                DisplayStatus(player, enemy);

                string playerInput = GetPlayerAction();

                if (string.IsNullOrEmpty(playerInput) || !IsValidAction(playerInput))
                {
                    Console.WriteLine("Invalid action! Please enter a valid command: (A)ttack, (D)efend, (H)eal, or (R)un.");
                    Thread.Sleep(800);
                    continue; 
                }

                ExecutePlayerAction(playerInput, player, enemy);

                if (enemy.IsAlive)
                {
                    EnemyTurn(player, enemy);
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine($"You were defeated by {enemy.Name}. Game Over!");
                    return;
                }
            }

            if (!enemy.IsAlive)
            {
                Console.Clear();
                Console.WriteLine($"You defeated {enemy.Name}!");

                if (enemy is WizardEnemy)
                {
                    Console.WriteLine("You have earned a magical reward for defeating the wizard!");
                    GrantWizardReward(player);
                }
                else
                {
                    GrantReward(player);
                }

                Console.WriteLine("\nUpdated Stats:");
                Console.WriteLine($"Health: {player.Health}");
                Console.WriteLine($"Coins: {player.Coins}");
                Console.WriteLine($"Potions: {player.Potions}");
                Console.WriteLine($"Experience: {player.Experience}");

                Console.WriteLine("\nPress Enter to return to the main menu...");
                Console.ReadLine();
            }
        }
        private bool IsValidAction(string input)
        {
            return input == "a" || input == "attack" || input == "d" || input == "defend" ||
                   input == "r" || input == "run" || input == "h" || input == "heal";
        }

        private void DisplayStatus(Player player, Enemy enemy)
        {
            Console.Clear();
            Console.WriteLine("======================");
            Console.WriteLine($"{enemy.Name}: {enemy.Health} HP");
            Console.WriteLine($"{player.Name}: {player.Health} HP | Potions: {player.Potions}");
            Console.WriteLine("======================");
            Console.WriteLine("(A)ttack, (D)efend, (H)eal, or (R)un");
            Console.WriteLine("======================");
        }

        private string GetPlayerAction()
        {
            string input = Console.ReadLine()!.ToLower();
            return input.Trim();
        }

        private void ExecutePlayerAction(string playerInput, Player player, Enemy enemy)
        {
            switch (playerInput)
            {
                case "a":
                case "attack":
                    PlayerAttack(player, enemy);
                    break;
                case "d":
                case "defend":
                    PlayerDefend(player, enemy);
                    break;
                case "r":
                case "run":
                    if (AttemptEscape(player, enemy))
                        return;
                    break;
                case "h":
                case "heal":
                    player.UsePotion();
                    break;
                default:
                    Console.WriteLine("Invalid action. Please try again!");
                    break;
            }
        }

        private void PlayerAttack(Player player, Enemy enemy)
        {
            int playerAttack = player.Attack();
            Console.WriteLine($"{player.Name} attacks {enemy.Name} for {playerAttack} damage!");
            enemy.TakeDamage(playerAttack);
            Thread.Sleep(800);
        }

        private void PlayerDefend(Player player, Enemy enemy)
        {
            Console.WriteLine($"{player.Name} braces for the attack!");
            int reducedAttack = enemy.Attack() / 2;
            player.TakeDamage(reducedAttack);
            Thread.Sleep(800);
        }

        private bool AttemptEscape(Player player, Enemy enemy)
        {
            Console.WriteLine($"{player.Name} tries to escape...");
            if (rand.Next(0, 2) == 0)
            {
                Console.WriteLine("Escape successful!");
                Thread.Sleep(800);
                return true;
            }
            else
            {
                Console.WriteLine($"...but {enemy.Name} blocks your way! Escape failed!");
                player.TakeDamage(enemy.Attack());
                Thread.Sleep(800);
                return false;
            }
        }
        private void EnemyTurn(Player player, Enemy enemy)
        {
            Console.WriteLine($"{enemy.Name} takes its turn...");

            if (enemy is WizardEnemy wizardEnemy)
            {
                if (!wizardEnemy.HasShownWizardMessage)
                {
                    Console.WriteLine($"Wizard {enemy.Name} detected!");
                    wizardEnemy.ShowWizardMessage();
                }

                if (enemy.Health < 30)
                {
                    Console.WriteLine($"{enemy.Name} casts a healing spell on itself!");
                    wizardEnemy.TryHeal();
                    Thread.Sleep(800);
                }
                else
                {
                    Console.WriteLine($"{enemy.Name} casts a powerful spell!");
                    int enemyAttack = enemy.Attack();
                    player.TakeDamage(enemyAttack);
                    Console.WriteLine($"Sarafin takes {enemyAttack} damage. Remaining health: {player.Health}");
                    Thread.Sleep(800);
                }
            }
            else
            {
                Console.WriteLine($"{enemy.Name} attacks!");
                int enemyAttack = enemy.Attack();
                player.TakeDamage(enemyAttack);
                Console.WriteLine($"Sarafin takes {enemyAttack} damage. Remaining health: {player.Health}");
                Thread.Sleep(800);
            }
        }
        private void GrantReward(Player player)
        {
            int coins = rand.Next(10, 51); 
            int potions = rand.Next(0, 2);
            int experience = rand.Next(5, 15); 

            player.AddReward(coins, potions, experience);
        }
        private void GrantWizardReward(Player player)
        {
            int coins = rand.Next(20, 101); 
            int potions = rand.Next(1, 3);  
            int experience = rand.Next(10, 25);  

            player.AddReward(coins, potions, experience);
        }
    }
}