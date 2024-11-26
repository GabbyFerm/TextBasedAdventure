using System;

namespace TextBasedAdventure.Classes
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Power { get; private set; }
        public int Armor { get; private set; }
        public int Coins { get; private set; }
        public int Potions { get; private set; }
        public int Experience { get; private set; }

        public Player(string name)
        {
            Name = name;
            MaxHealth = 100;
            Health = MaxHealth;
            Power = 15;
            Armor = 5;
            Coins = 100;
            Potions = 1;
            Experience = 0;
        }

        public enum PlayerClass { Mage, Archer, Warrior }
        public PlayerClass CurrentClass { get; set; } 

        public int Attack()
        {
            return new Random().Next(5, Power + 1);
        }
        public void UsePotion()
        {
            if (Potions > 0)
            {
                int healAmount = Math.Min(20, MaxHealth - Health);
                Health += healAmount;
                Potions--;
                Console.WriteLine($"{Name} uses a potion and heals {healAmount} health. Remaining potions: {Potions}");
            }
            else
            {
                Console.WriteLine("No potions left!");
            }
        }
        public void AddReward(int coins, int potions, int experience = 0)
        {
            Coins += coins;
            Potions += potions;
            Experience += experience;  
            Console.WriteLine($"{Name} receives {coins} coins, {potions} potions, and {experience} experience points. Total coins: {Coins}, Total potions: {Potions}, Total experience: {Experience}");
        }
        public void TakeDamage(int damage)
        {
            int damageTaken = Math.Max(0, damage - Armor);  
            Health -= damageTaken;
            Console.WriteLine($"{Name} takes {damageTaken} damage after armor. Remaining health: {Health}");
        }
        public void AddCoins(int amount)
        {
            Coins += amount;
        }
        public void DecreaseCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
            }
            else
            {
                Console.WriteLine("Not enough coins!");
            }
        }
        public void AddArmor(int amount)
        {
            Armor += amount;
        }
        public void AddPotion(int amount)
        {
            Potions += amount;
        }
        public void AddPower(int amount)
        {
            Power += amount;
        }
    }
}