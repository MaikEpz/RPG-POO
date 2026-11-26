using System;

namespace Parrex
{
    public class SpellBook : Weapon
    {
        public SpellBook() : base("Grimorio Antiguo", 30, 5)
        {
        }

        public SpellBook(string name, int baseDamage, int weight) : base(name, baseDamage, weight)
        {
        }

        public override void Attack()
        {
            Console.WriteLine($"📖 {this.name} canaliza energía arcana desde sus páginas (Peso: {this.weight}, Daño: {this.baseDamage}).");
        }
    }
}
