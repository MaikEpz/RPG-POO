using System;

namespace Parrex
{
    public abstract class Character
    {
        // Atributos protegidos
        protected string nameChar = ""; 
        protected int attackPower;
        protected int lifePoint;
        protected char gender;

        protected bool IsAlive = true;
        protected readonly int MaxLifepoint;

        // Nuevos atributos del diagrama objetivo usando arreglos []:
        protected Weapon? equippedWeapon;
        protected Skill[] skills = new Skill[0];

        // Propiedades públicas tradicionales con get y set completos (sin =>)
        public string NameChar
        {
            get
            {
                return this.nameChar;
            }
            set
            {
                this.nameChar = value;
            }
        }

        public int Attackpower
        {
            get
            {
                return this.attackPower;
            }
            set
            {
                if (value < 0)
                {
                    this.attackPower = 0;
                }
                else
                {
                    this.attackPower = value;
                }
            }
        }

        public int LifePoint
        {
            get
            {
                return this.lifePoint;
            }
            set
            {
                if (value < 0)
                {
                    this.lifePoint = 0;
                }
                else
                {
                    this.lifePoint = value;
                }
            }
        }

        public char Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (value == 'M' || value == 'F')
                {
                    this.gender = value;
                }
                else
                {
                    this.gender = 'M';
                }
            }
        }

        public Weapon? EquippedWeapon
        {
            get
            {
                return this.equippedWeapon;
            }
            set
            {
                this.equippedWeapon = value;
            }
        }

        public Skill[] Skills
        {
            get
            {
                return this.skills;
            }
            set
            {
                this.skills = value;
            }
        }

        // Constructores
        public Character()
        {
        }

        public Character(string nameChar, int attackPower, int maxLifepoint, char gender)
        {
            this.nameChar = nameChar; 
            this.attackPower = attackPower;
            this.lifePoint = maxLifepoint;
            this.MaxLifepoint = maxLifepoint;    
            this.gender = gender;  
        }

        // Método virtual para equipar arma (valida según el personaje)
        public virtual bool EquipWeapon(Weapon weapon)
        {
            this.equippedWeapon = weapon;
            return true;
        }

        // Método para usar una habilidad del arreglo por su índice
        public void UseSkill(int index, Character? target = null)
        {
            if (this.skills != null && index >= 0 && index < this.skills.Length)
            {
                this.skills[index].Execute(this, target);
            }
            else
            {
                Console.WriteLine("[!] Habilidad no disponible.");
            }
        }

        public virtual void Info()
        {
            string genderString;
            if (this.Gender == 'M')
            {
                genderString = "Masculino";
            }
            else
            {
                genderString = "Femenino";
            }

            Console.WriteLine(
                $"**** Personajes ****\n" +
                $"Nombre: {this.nameChar}\n" +
                $"Poder: {this.attackPower}\n" +
                $"Vida: {this.lifePoint} / {this.MaxLifepoint}\n" +
                $"Género: {genderString}");

            if (this.equippedWeapon != null)
            {
                Console.WriteLine($"Arma equipada: {this.equippedWeapon.Name} (Daño: {this.equippedWeapon.BaseDamage}, Peso: {this.equippedWeapon.Weight})");
            }

            if (this.skills != null && this.skills.Length > 0)
            {
                Console.Write("Habilidades: ");
                for (int i = 0; i < this.skills.Length; i++)
                {
                    if (this.skills[i] != null)
                    {
                        Console.Write($"[{i + 1}] {this.skills[i].Name} (Coste: {this.skills[i].Cost}) ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void TakeDamage(int damage)
        {
            if (this.IsAlive)
            {
                this.LifePoint = this.LifePoint - damage;
                if (this.LifePoint <= 0)
                {
                    Console.WriteLine($"{this.NameChar} ha caído en combate.");
                    this.IsAlive = false;
                }
                else
                {
                    Console.WriteLine($"{this.NameChar} recibe {damage} de daño. (Vida restante: {this.LifePoint})");
                }
            }
        }

        public void Heal(int healAmount)
        {
            if (this.IsAlive)
            {
                if (this.lifePoint >= this.MaxLifepoint)
                {
                    Console.WriteLine($"La vida de {this.NameChar} ya está al máximo.");
                }
                else
                {
                    this.lifePoint = this.lifePoint + healAmount;
                    if (this.lifePoint > this.MaxLifepoint)
                    {
                        this.lifePoint = this.MaxLifepoint;
                    }
                    Console.WriteLine($"{this.NameChar} se cura {healAmount} de vida. (Vida actual: {this.lifePoint})");
                }
            }
        }

        public abstract void Attack();
    }
}
