using System;

namespace Parrex
{
    public class Sword : Weapon
    {
        public Sword() : base("Espada de Acero", 25, 10)
        {
        }

        public Sword(string name, int baseDamage, int weight) : base(name, baseDamage, weight)
        {
        }

        public override void Attack()
        {
            Console.WriteLine($"⚔️ {this.name} realiza un tajo veloz (Peso: {this.weight}, Daño: {this.baseDamage}).");
        }
    }
}
