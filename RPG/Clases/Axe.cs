using System;

namespace Parrex
{
    public class Axe : Weapon
    {
        public Axe() : base("Hacha de Batalla", 35, 22)
        {
        }

        public Axe(string name, int baseDamage, int weight) : base(name, baseDamage, weight)
        {
        }

        public override void Attack()
        {
            Console.WriteLine($"🪓 {this.name} desata un golpe pesado que rompe defensas (Peso: {this.weight}, Daño: {this.baseDamage}).");
        }
    }
}
