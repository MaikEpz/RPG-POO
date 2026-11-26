using System;
using Parrex;

namespace Tetranova
{
    public class Enemy : Character
    {
        public string Nameenemy
        {
            get
            {
                return this.nameChar;
            }
        }

        public Enemy() : base("Enemigo", 20, 100, 'M')
        {
        }

        public Enemy(string nameenemy, int attackpower, int maxlifepoint, char gender)
            : base(nameenemy, attackpower, maxlifepoint, gender)
        {
        }

        public override void Attack()
        {
            Console.WriteLine($"😈 {this.nameChar} ataca ferozmente con garras oscuras.");
        }

        public void attack()
        {
            this.Attack();
        }
    }
}
