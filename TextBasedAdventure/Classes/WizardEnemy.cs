using System;
using System.Collections.Generic;
using System.Linq;

namespace TextBasedAdventure.Classes
{
    public class WizardEnemy : Enemy
    {
        private static Random wizardRand = new Random(); 

        public WizardEnemy() : base("Rincewind the dark Wizard", 65, 10)
        {
        }

        private bool hasShownWizardMessage = false;

        public bool HasShownWizardMessage => hasShownWizardMessage;

        public void ShowWizardMessage()
        {
            hasShownWizardMessage = true;
        }

        public override int Attack()
        {
            Console.WriteLine($"{Name} casts a powerful spell!");

            int attackDamage = wizardRand.Next(5, Power + 5);

            return attackDamage;
        }
        protected void Heal()
        {
            Console.WriteLine($"{Name} casts a healing spell on itself!");
            int healAmount = wizardRand.Next(5, 10);  
            this.Health += healAmount;
            Console.WriteLine($"{Name} heals for {healAmount} health! New health: {this.Health}");
        }

        public void TryHeal()
        {
            Console.WriteLine($"{Name} is trying to heal...");
            if (this.Health < 20)  
            {
                Heal();
            }
            else
            {
                Console.WriteLine($"{Name} is healthy and doesn't need healing.");
            }
        }
        public override void Die()
        {
            Console.WriteLine($"The {Name} falls, and you sense the magic fade away...");
        }
    }
}