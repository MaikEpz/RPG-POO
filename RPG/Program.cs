using System;
using harryppoter;
using Mappe;
using Tetranova;

namespace Parrex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("         MINI RPG - DIAGRAMA OBJETIVO (POO)       ");
            Console.WriteLine("==================================================\n");

            // --------------------------------------------------------
            // 1. SELECCIÓN DE PERSONAJE
            // --------------------------------------------------------
            Console.WriteLine("Selecciona tu héroe:");
            Console.WriteLine("1. Guerrero (Titan) - Fuerza: 25 | Vida: 130");
            Console.WriteLine("2. Mago (Itachi)     - Poder Mágico: 30 | Maná: 70 | Vida: 110");
            Console.Write("Elige una opción (1 o 2): ");

            string? eleccion = Console.ReadLine();
            if (eleccion != null)
            {
                eleccion = eleccion.Trim();
            }

            Character jugador;

            if (eleccion == "2")
            {
                Wizard mago = new Wizard("Itachi", 20, 110, 'M', 30, 70, Listelement.fire);
                mago.Skills = new Skill[]
                {
                    new Skill("Llamarada Ígnea", 20, 50, SkillType.Damage),
                    new Skill("Meditación Arcana", 0, 30, SkillType.RestoreMana),
                    new Skill("Luz de Curación", 15, 35, SkillType.HealHealth)
                };
                jugador = mago;
                Console.WriteLine("\n¡Has elegido al Mago!");
            }
            else
            {
                Warrior guerrero = new Warrior("Titan", 25, 130, 'M', 25, 15);
                guerrero.Skills = new Skill[]
                {
                    new Skill("Tajo Feroz", 0, 40, SkillType.Damage),
                    new Skill("Grito de Guerra", 0, 55, SkillType.Damage),
                    new Skill("Primeros Auxilios", 0, 25, SkillType.HealHealth)
                };
                jugador = guerrero;
                Console.WriteLine("\n¡Has elegido al Guerrero!");
            }

            // --------------------------------------------------------
            // 2. SELECCIÓN Y VALIDACIÓN DE ARMA SEGÚN EL PESO
            // --------------------------------------------------------
            Weapon[] armeria = new Weapon[]
            {
                new Sword("Daga Ligera", 18, 5),
                new Sword("Espada de Acero", 26, 10),
                new Axe("Hacha Vorpal", 36, 20),
                new Axe("Hacha Titánica Colosal", 55, 32),
                new SpellBook("Grimorio de las Sombras", 30, 5),
                new SpellBook("Tomo de la Tormenta", 42, 8)
            };

            bool armaEquipada = false;
            while (!armaEquipada)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("   ARMERÍA DISPONIBLE - ELIGE UN ARMA PARA TU PJ  ");
                Console.WriteLine("==================================================");
                for (int i = 0; i < armeria.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {armeria[i].Name} | Daño: {armeria[i].BaseDamage} | Peso: {armeria[i].Weight}");
                }
                Console.Write($"Elige el número de arma (1-{armeria.Length}): ");

                string? entradaArma = Console.ReadLine();
                if (int.TryParse(entradaArma, out int seleccion) && seleccion >= 1 && seleccion <= armeria.Length)
                {
                    Weapon armaElegida = armeria[seleccion - 1];
                    Console.WriteLine($"\nIntentando equipar '{armaElegida.Name}'...");

                    // Se valida automáticamente según la clase del personaje y el peso del arma:
                    armaEquipada = jugador.EquipWeapon(armaElegida);

                    if (!armaEquipada)
                    {
                        Console.WriteLine(">> El arma seleccionada no es apta para tu personaje. Elige otra.");
                    }
                }
                else
                {
                    Console.WriteLine("Opción no válida. Por favor introduce un número de la lista.");
                }
            }

            Console.WriteLine("\n--- Ficha completa de tu personaje ---");
            jugador.Info();

            // --------------------------------------------------------
            // 3. CREACIÓN DEL ENEMIGO
            // --------------------------------------------------------
            Enemy enemigo = new Enemy("Drakula Oscuro", 20, 110, 'M');
            enemigo.EquippedWeapon = new Sword("Espada Sombra", 22, 12);
            enemigo.Skills = new Skill[]
            {
                new Skill("Mordida Vampírica", 0, 28, SkillType.Damage)
            };

            Console.WriteLine("¡Un enemigo salvaje ha aparecido!");
            if (enemigo.EquippedWeapon != null)
            {
                Console.WriteLine($"Enemigo: {enemigo.NameChar} | Vida: {enemigo.LifePoint} | Arma: {enemigo.EquippedWeapon.Name}");
            }
            Console.WriteLine();

            Console.WriteLine("==================================================");
            Console.WriteLine("               ¡COMIENZA EL COMBATE!              ");
            Console.WriteLine("==================================================");

            int turno = 1;

            // --------------------------------------------------------
            // 4. BUCLE DE COMBATE POR TURNOS
            // --------------------------------------------------------
            while (jugador.LifePoint > 0 && enemigo.LifePoint > 0)
            {
                Console.WriteLine($"\n------------------- TURNO {turno} -------------------");
                Console.WriteLine($"[TÚ]      {jugador.NameChar} - Vida: {jugador.LifePoint}");
                if (jugador is Wizard wiz)
                {
                    Console.WriteLine($"          Maná: {wiz.Manna}");
                }
                Console.WriteLine($"[ENEMIGO] {enemigo.NameChar} - Vida: {enemigo.LifePoint}");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("Elige tu acción:");
                Console.WriteLine("1. Atacar con el arma equipada");
                Console.WriteLine("2. Usar una Habilidad (Skill)");
                Console.Write("Opción (1 o 2): ");

                string? opcion = Console.ReadLine();
                if (opcion != null)
                {
                    opcion = opcion.Trim();
                }
                Console.WriteLine();

                if (opcion == "2")
                {
                    Console.WriteLine("--- Tus Habilidades ---");
                    for (int i = 0; i < jugador.Skills.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {jugador.Skills[i].Name} (Poder: {jugador.Skills[i].Power}, Coste: {jugador.Skills[i].Cost}, Tipo: {jugador.Skills[i].Type})");
                    }
                    Console.Write("Selecciona una habilidad (número): ");
                    string? entradaSkill = Console.ReadLine();
                    if (int.TryParse(entradaSkill, out int numSkill) && numSkill >= 1 && numSkill <= jugador.Skills.Length)
                    {
                        jugador.UseSkill(numSkill - 1, target: enemigo);
                    }
                    else
                    {
                        Console.WriteLine("Opción de habilidad inválida, perdiste tu turno.");
                    }
                }
                else
                {
                    Console.WriteLine($"-> {jugador.NameChar} ataca:");
                    jugador.Attack();
                    int danioTotal = jugador.Attackpower;
                    if (jugador.EquippedWeapon != null)
                    {
                        danioTotal = danioTotal + jugador.EquippedWeapon.BaseDamage;
                    }
                    enemigo.TakeDamage(danioTotal);
                }

                if (enemigo.LifePoint <= 0)
                {
                    Console.WriteLine("\n==================================================");
                    Console.WriteLine($"¡VICTORIA! Has vencido a {enemigo.NameChar}.");
                    Console.WriteLine("==================================================");
                    break;
                }

                // Turno del enemigo
                Console.WriteLine($"\n--- Turno de {enemigo.NameChar} ---");
                if (turno % 2 == 0 && enemigo.Skills.Length > 0)
                {
                    Console.WriteLine($"-> {enemigo.NameChar} utiliza una habilidad especial contra ti:");
                    enemigo.UseSkill(0, target: jugador);
                }
                else
                {
                    enemigo.Attack();
                    int danioEnemigo = enemigo.Attackpower;
                    if (enemigo.EquippedWeapon != null)
                    {
                        danioEnemigo = danioEnemigo + enemigo.EquippedWeapon.BaseDamage;
                    }
                    jugador.TakeDamage(danioEnemigo);
                }

                if (jugador.LifePoint <= 0)
                {
                    Console.WriteLine("\n==================================================");
                    Console.WriteLine("DERROTA: Has caído en batalla...");
                    Console.WriteLine("==================================================");
                    break;
                }

                turno++;
            }

            Console.WriteLine("\nFin del juego.");
        }
    }
}
