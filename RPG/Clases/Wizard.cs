using System;
using Parrex;

namespace harryppoter
{
    public enum Listelement
    {
        water,
        earth,
        fire,
        wind
    }

    public class Wizard : Character
    {
        private int powerMag;
        private int manna;
        private Listelement typeElement;

        public int PowerMag
        {
            get
            {
                return this.powerMag;
            }
            set
            {
                if (value < 1)
                {
                    this.powerMag = 1;
                }
                else
                {
                    this.powerMag = value;
                }
            }
        }

        public int Manna
        {
            get
            {
                return this.manna;
            }
            set
            {
                if (value < 0)
                {
                    this.manna = 0;
                }
                else if (value > 150)
                {
                    this.manna = 150;
                }
                else
                {
                    this.manna = value;
                }
            }
        }

        public Listelement TypeElement
        {
            get
            {
                return this.typeElement;
            }
            set
            {
                this.typeElement = value;
            }
        }

        public Wizard()
        {
        }

        public Wizard(string nameChar, int attackPower, int maxLifepoint, char gender, int powerMag, int manna, Listelement typeElement, Weapon? weapon = null)
            : base(nameChar, attackPower, maxLifepoint, gender)
        {
            this.powerMag = powerMag;
            this.manna = manna;
            this.typeElement = typeElement;
            if (weapon != null)
            {
                this.EquipWeapon(weapon);
            }
        }

        // Valida si el mago puede usar el arma (libros de hechizos o armas ligeras de peso <= 10)
        public override bool EquipWeapon(Weapon weapon)
        {
            if (weapon is SpellBook)
            {
                this.equippedWeapon = weapon;
                Console.WriteLine($"¡{this.nameChar} equipó el libro {weapon.Name} con éxito!");
                return true;
            }
            else if (weapon.Weight <= 10)
            {
                this.equippedWeapon = weapon;
                Console.WriteLine($"¡{this.nameChar} equipó el arma ligera {weapon.Name} con éxito!");
                return true;
            }
            else
            {
                Console.WriteLine($"[!] {this.nameChar} es un mago y no puede cargar armas pesadas como {weapon.Name} (Peso: {weapon.Weight}).");
                return false;
            }
        }

        public override void Attack()
        {
            if (this.manna >= 5)
            {
                this.manna = this.manna - 5;
                if (this.equippedWeapon != null)
                {
                    Console.WriteLine($"{this.nameChar} canaliza magia elemental ({this.typeElement}) a través de su {this.equippedWeapon.Name} (Gasta 5 maná):");
                    this.equippedWeapon.Attack();
                }
                else
                {
                    Console.WriteLine($"{this.nameChar} lanzó un rayo de elemento {this.typeElement} (Gasta 5 maná).");
                }
            }
            else
            {
                Console.WriteLine($"{this.nameChar} no dispone de maná suficiente (5) para atacar.");
            }
        }

        public override void Info()
        {
            base.Info();
            Console.WriteLine($"Poder mágico: {this.powerMag} | Maná: {this.manna} | Elemento: {this.typeElement}");
        }
    }
}
