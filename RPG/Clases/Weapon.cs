using System;

namespace Parrex
{
    public abstract class Weapon
    {
        protected string name;
        protected int baseDamage;
        protected int weight;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public int BaseDamage
        {
            get
            {
                return this.baseDamage;
            }
            set
            {
                this.baseDamage = value;
            }
        }

        public int Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        public Weapon()
        {
            this.name = "Arma básica";
            this.baseDamage = 10;
            this.weight = 5;
        }

        public Weapon(string name, int baseDamage, int weight)
        {
            this.name = name;
            this.baseDamage = baseDamage;
            this.weight = weight;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"[Arma] {this.name} | Daño Base: {this.baseDamage} | Peso: {this.weight}");
        }

        public abstract void Attack();
    }
}
