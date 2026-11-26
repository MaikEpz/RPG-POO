using System;
using Parrex;

namespace Mappe
{
    public class Warrior : Character
    {
        private int force;
        private int defend;

        public int Force
        {
            get
            {
                return this.force;
            }
            set
            {
                if (value < 1)
                {
                    this.force = 1;
                }
                else
                {
                    this.force = value;
                }
            }
        }

        public int Defend
        {
            get
            {
                return this.defend;
            }
            set
            {
                if (value < 0)
                {
                    this.defend = 0;
                }
                else
                {
                    this.defend = value;
                }
            }
        }

        public Warrior()
        {
        }

        public Warrior(string nameChar, int attackPower, int maxLifepoint, char gender, int force, int defend, Weapon? weapon = null)
            : base(nameChar, attackPower, maxLifepoint, gender)
        {
            this.force = force;
            this.defend = defend;
            if (weapon != null)
            {
                this.EquipWeapon(weapon);
            }
        }

        // Valida que el guerrero no use libros de hechizos (al no tener maná) y que tenga suficiente fuerza para el peso del arma
        public override bool EquipWeapon(Weapon weapon)
        {
            if (weapon is SpellBook)
            {
                Console.WriteLine($"[!] {this.nameChar} no puede equipar {weapon.Name} porque es un libro de hechizos y los guerreros no poseen maná.");
                return false;
            }

            if (this.force >= weapon.Weight)
            {
                this.equippedWeapon = weapon;
                Console.WriteLine($"¡{this.nameChar} equipó {weapon.Name} con éxito!");
                return true;
            }
            else
            {
                Console.WriteLine($"[!] {this.nameChar} no tiene suficiente fuerza ({this.force}) para levantar {weapon.Name} (Peso requerido: {weapon.Weight}).");
                return false;
            }
        }

        public override void Attack()
        {
            if (this.equippedWeapon != null)
            {
                Console.WriteLine($"{this.nameChar} blande su {this.equippedWeapon.Name}:");
                this.equippedWeapon.Attack();
            }
            else
            {
                Console.WriteLine($"{this.nameChar} realiza un ataque cuerpo a cuerpo.");
            }
        }

        public override void Info()
        {
            base.Info();
            Console.WriteLine($"Fuerza: {this.force} | Defensa: {this.defend}");
        }
    }
}
