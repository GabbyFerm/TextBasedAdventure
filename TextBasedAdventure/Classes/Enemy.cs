public class Enemy
{
    public string Name { get; set; }
    public int Health { get; protected set; }
    public int Power { get; protected set; }

    public bool IsAlive => Health > 0;

    protected static Random rand = new Random();

    public Enemy(string name, int health, int power)
    {
        Name = name;
        Health = health;
        Power = power;
    }

    public virtual int Attack()
    {
        return rand.Next(1, Power + 1); 
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            Health -= damage;
            if (Health > 0)
            {
                Console.WriteLine($"{Name} takes {damage} damage. Remaining health: {Health}");
            }
            else
            {
                Console.WriteLine($"{Name} takes {damage} damage and is defeated!");
            }
        }
    }

    public virtual void Die()
    {
        Console.WriteLine($"{Name} has been defeated!");
    }
}