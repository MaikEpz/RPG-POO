using System;

namespace Parrex
{
    public class Skill
    {
        private string name;
        private int cost;
        private int power;
        private SkillType type;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Cost
        {
            get
            {
                return this.cost;
            }
        }

        public int Power
        {
            get
            {
                return this.power;
            }
        }

        public SkillType Type
        {
            get
            {
                return this.type;
            }
        }

        public Skill(string name, int cost, int power, SkillType type)
        {
            this.name = name;
            this.cost = cost;
            this.power = power;
            this.type = type;
        }

        public void Execute(Character user, Character? target = null)
        {
            if (user is harryppoter.Wizard wizard && this.cost > 0)
            {
                if (wizard.Manna < this.cost)
                {
                    Console.WriteLine($"[!] {user.NameChar} no tiene suficiente maná para '{this.name}' (requiere {this.cost}).");
                    return;
                }
                wizard.Manna = wizard.Manna - this.cost;
            }

            switch (this.type)
            {
                case SkillType.Damage:
                    if (target != null && target.LifePoint > 0)
                    {
                        Console.WriteLine($"💥 {user.NameChar} lanza '{this.name}' contra {target.NameChar} causando {this.power} de daño.");
                        target.TakeDamage(this.power);
                    }
                    else
                    {
                        Console.WriteLine($"[!] No hay un objetivo válido para '{this.name}'.");
                    }
                    break;

                case SkillType.HealHealth:
                    Console.WriteLine($"💚 {user.NameChar} usa '{this.name}' (Coste {this.cost}) y se cura.");
                    user.Heal(this.power);
                    break;

                case SkillType.RestoreMana:
                    if (user is harryppoter.Wizard wiz)
                    {
                        wiz.Manna = wiz.Manna + this.power;
                        Console.WriteLine($"✨ {user.NameChar} usa '{this.name}' (Coste {this.cost}) y regenera {this.power} de maná. (Maná actual: {wiz.Manna})");
                    }
                    else
                    {
                        Console.WriteLine($"✨ {user.NameChar} usó '{this.name}', pero no usa maná.");
                    }
                    break;
            }
        }
    }
}
