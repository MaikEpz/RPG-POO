# ⚔️ Mini RPG - Guía Educativa paso a paso para crear `Program.cs` (POO en C#)

Este repositorio contiene un proyecto de consola en **C# (.NET 10)** diseñado para practicar **Programación Orientada a Objetos (POO)**: abstracción, herencia, encapsulación, polimorfismo, enumeraciones y composición/agregación.

En esta guía aprenderás **paso a paso** cómo construir desde cero el archivo `RPG/Program.cs` para conectar todas las clases del juego ([`RPG/Clases/`](RPG/Clases/)) y dar vida a un combate por turnos con colores diferenciados en consola.

> 📐 Puedes consultar la arquitectura completa del proyecto en el [Diagrama de Clases](RPG/DiagramaClases.md).

---

## 📋 Requisitos previos y estructura del proyecto

Antes de escribir `Program.cs`, asegúrate de comprender qué piezas ya existen en la carpeta `RPG/Clases/`:

| Archivo | Namespace | Rol en el juego |
| :--- | :--- | :--- |
| [`Character.cs`](RPG/Clases/Character.cs) | `Parrex` | Clase abstracta base para héroes y enemigos (`LifePoint`, `Attack()`, `TakeDamage()`, `UseSkill()`, `EquipWeapon()`). |
| [`Warrior.cs`](RPG/Clases/Warrior.cs) | `Mappe` | Subclase de `Character`. Tiene `Force` y `Defend`. No usa libros de hechizos y valida el peso según su fuerza. |
| [`Wizard.cs`](RPG/Clases/Wizard.cs) | `harryppoter` | Subclase de `Character`. Incluye el enum `Listelement`, `PowerMag` y `Manna`. Usa libros o armas ligeras (`Weight <= 10`). |
| [`Enemy.cs`](RPG/Clases/Enemy.cs) | `Tetranova` | Subclase de `Character` para los adversarios del combate. |
| [`Weapon.cs`](RPG/Clases/Weapon.cs) | `Parrex` | Clase abstracta base de armas (`Name`, `BaseDamage`, `Weight`, `Attack()`). |
| [`Sword.cs`](RPG/Clases/Sword.cs), [`Axe.cs`](RPG/Clases/Axe.cs), [`SpellBook.cs`](RPG/Clases/SpellBook.cs) | `Parrex` | Tipos concretos de armas que heredan de `Weapon`. |
| [`Skill.cs`](RPG/Clases/Skill.cs) y [`SkillType.cs`](RPG/Clases/SkillType.cs) | `Parrex` | Sistema de habilidades (`Damage`, `HealHealth`, `RestoreMana`) ejecutadas mediante `Execute()`. |

---

## 🚀 Cómo ejecutar el proyecto

Una vez que hayas creado el archivo `RPG/Program.cs` siguiendo los pasos de esta guía, ejecuta desde la raíz del repositorio:

```bash
dotnet run --project RPG/RPG.csproj
```

---

## 🛠️ Paso a paso para construir `RPG/Program.cs`

### Paso 0: Importar namespaces, helper de color y estructura base

Como nuestras clases están repartidas en distintos *namespaces* (`Parrex`, `Mappe`, `harryppoter`, `Tetranova`), lo primero es importarlos con `using`. Además, añadimos un método auxiliar `PrintColor` para distinguir visualmente al **jugador (Cyan)** del **enemigo (Red)** sin modificar las clases de `RPG/Clases/`.

```csharp
using System;
using harryppoter;
using Mappe;
using Tetranova;

namespace Parrex
{
    public class Program
    {
        private static void PrintColor(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("         MINI RPG - DIAGRAMA OBJETIVO (POO)       ");
            Console.WriteLine("==================================================\n");

            // Aquí iremos agregando cada paso dentro de Main...
        }
    }
}
```

> 💡 **Concepto clave:** Los `using` permiten que `Program.cs` (en `Parrex`) reconozca clases definidas en otros namespaces, y `Console.ResetColor()` evita que el color se quede pegado en las siguientes líneas.

---

### Paso 1: Selección de personaje (Polimorfismo y habilidades)

#### 1.1 Mostrar el menú y leer la elección del usuario

```csharp
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
```

> 💡 **Polimorfismo:** Declarar `Character jugador;` permite guardar en la misma variable tanto un `Wizard` como un `Warrior`, porque ambos heredan de `Character`.

#### 1.2 Instanciar la clase elegida y asignar su arreglo `Skill[]`

```csharp
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
    PrintColor("\n¡Has elegido al Mago!", ConsoleColor.Cyan);
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
    PrintColor("\n¡Has elegido al Guerrero!", ConsoleColor.Cyan);
}
```

> 💡 **Conceptos clave:**
> - **Agregación con arreglos:** Cada personaje recibe un `Skill[]` con sus habilidades.
> - **Enumeraciones (`enum`):** `Listelement.fire` y `SkillType.Damage` usan valores cerrados y legibles.

---

### Paso 2: Armería y validación polimórfica al equipar

#### 2.1 Definir el inventario de armas (`Weapon[]`)

Creamos un arreglo polimórfico `Weapon[]` que mezcla `Sword`, `Axe` y `SpellBook`:

```csharp
Weapon[] armeria = new Weapon[]
{
    new Sword("Espada de Acero", 26, 10),
    new Axe("Hacha Vorpal", 36, 20),
    new Axe("Hacha Titánica Colosal", 55, 32),
    new SpellBook("Grimorio de las Sombras", 30, 5),
    new SpellBook("Tomo de la Tormenta", 42, 8)
};
```

#### 2.2 Validar y equipar el arma en un bucle `while`

Repetimos el menú hasta que `jugador.EquipWeapon(armaElegida)` devuelva `true`:

```csharp
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
```

> 💡 **¿Por qué `EquipWeapon(...)` valida diferente según el héroe?**
> Es un método `virtual` sobreescrito (`override`):
> - **`Warrior`:** Rechaza `SpellBook` y exige `force >= weapon.Weight` (Titan tiene fuerza `25`, así que no puede levantar el *Hacha Titánica Colosal* de peso `32`).
> - **`Wizard`:** Acepta `SpellBook` o armas ligeras (`Weight <= 10`), pero rechaza las hachas pesadas.

#### 2.3 Mostrar la ficha del personaje (`Info()`)

```csharp
Console.WriteLine("\n--- Ficha completa de tu personaje ---");
Console.ForegroundColor = ConsoleColor.Cyan;
jugador.Info();
Console.ResetColor();
```

---

### Paso 3: Creación del enemigo (`Enemy`)

Instanciamos al rival usando `Enemy` (que hereda de `Character`), le asignamos un arma y una habilidad, e imprimimos su presentación en rojo:

```csharp
Enemy enemigo = new Enemy("Drakula Oscuro", 20, 110, 'M');
enemigo.EquippedWeapon = new Sword("Espada Sombra", 22, 12);
enemigo.Skills = new Skill[]
{
    new Skill("Mordida Vampírica", 0, 28, SkillType.Damage)
};

PrintColor("¡Un enemigo salvaje ha aparecido!", ConsoleColor.Red);
if (enemigo.EquippedWeapon != null)
{
    PrintColor($"Enemigo: {enemigo.NameChar} | Vida: {enemigo.LifePoint} | Arma: {enemigo.EquippedWeapon.Name}", ConsoleColor.Red);
}
Console.WriteLine();

Console.WriteLine("==================================================");
Console.WriteLine("               ¡COMIENZA EL COMBATE!              ");
Console.WriteLine("==================================================");
```

> 💡 **Concepto clave:** `Enemy` reutiliza `NameChar`, `LifePoint`, `EquippedWeapon`, `Skills` y `TakeDamage()` directamente por **herencia** desde `Character`.

---

### Paso 4: El bucle de combate por turnos (`while`)

#### 4.1 Iniciar el bucle y mostrar el estado del turno

El combate sigue mientras ambos tengan vida (`> 0`). Usamos `jugador is Wizard wiz` (*pattern matching*) para mostrar el maná solo cuando el héroe es mago:

```csharp
int turno = 1;

while (jugador.LifePoint > 0 && enemigo.LifePoint > 0)
{
    Console.WriteLine($"\n------------------- TURNO {turno} -------------------");
    PrintColor($"[TÚ]      {jugador.NameChar} - Vida: {jugador.LifePoint}", ConsoleColor.Cyan);
    if (jugador is Wizard wiz)
    {
        PrintColor($"          Maná: {wiz.Manna}", ConsoleColor.Cyan);
    }
    PrintColor($"[ENEMIGO] {enemigo.NameChar} - Vida: {enemigo.LifePoint}", ConsoleColor.Red);
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
```

#### 4.2 Turno del jugador: usar habilidad o atacar con arma

Dentro del mismo `while`, si elige `"2"` ejecuta una habilidad con `UseSkill(...)`; de lo contrario ataca con `Attack()` y aplica el daño al enemigo (`TakeDamage`):

```csharp
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            jugador.UseSkill(numSkill - 1, target: enemigo);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Opción de habilidad inválida, perdiste tu turno.");
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"-> {jugador.NameChar} ataca:");
        jugador.Attack();
        int danioTotal = jugador.Attackpower;
        if (jugador.EquippedWeapon != null)
        {
            danioTotal = danioTotal + jugador.EquippedWeapon.BaseDamage;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        enemigo.TakeDamage(danioTotal);
        Console.ResetColor();
    }
```

> 💡 **Seguridad ante nulos y encapsulación:** Comprobamos `if (jugador.EquippedWeapon != null)` antes de leer `BaseDamage`, y usamos `TakeDamage(...)` para que el propio objeto controle sus límites de vida.

#### 4.3 Verificar victoria, turno del enemigo y derrota

Cerramos el `while` comprobando si el enemigo cayó, ejecutando su turno (habilidad en turnos pares, ataque normal en impares) y verificando si el jugador sobrevivió:

```csharp
    if (enemigo.LifePoint <= 0)
    {
        PrintColor($"\n==================================================\n¡VICTORIA! Has vencido a {enemigo.NameChar}.\n==================================================", ConsoleColor.Green);
        break;
    }

    // Turno del enemigo
    PrintColor($"\n--- Turno de {enemigo.NameChar} ---", ConsoleColor.Red);
    if (turno % 2 == 0 && enemigo.Skills.Length > 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"-> {enemigo.NameChar} utiliza una habilidad especial contra ti:");
        enemigo.UseSkill(0, target: jugador);
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        enemigo.Attack();
        int danioEnemigo = enemigo.Attackpower;
        if (enemigo.EquippedWeapon != null)
        {
            danioEnemigo = danioEnemigo + enemigo.EquippedWeapon.BaseDamage;
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        jugador.TakeDamage(danioEnemigo);
        Console.ResetColor();
    }

    if (jugador.LifePoint <= 0)
    {
        PrintColor("\n==================================================\nDERROTA: Has caído en batalla...\n==================================================", ConsoleColor.Red);
        break;
    }

    turno++;
}

Console.WriteLine("\nFin del juego.");
```

---

## 📄 Código completo de `Program.cs`

<details>
<summary>Haz clic aquí para desplegar el archivo RPG/Program.cs completo</summary>

```csharp
using System;
using harryppoter;
using Mappe;
using Tetranova;

namespace Parrex
{
    public class Program
    {
        private static void PrintColor(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

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
                PrintColor("\n¡Has elegido al Mago!", ConsoleColor.Cyan);
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
                PrintColor("\n¡Has elegido al Guerrero!", ConsoleColor.Cyan);
            }

            // --------------------------------------------------------
            // 2. SELECCIÓN Y VALIDACIÓN DE ARMA SEGÚN EL PESO
            // --------------------------------------------------------
            Weapon[] armeria = new Weapon[]
            {
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            jugador.Info();
            Console.ResetColor();

            // --------------------------------------------------------
            // 3. CREACIÓN DEL ENEMIGO
            // --------------------------------------------------------
            Enemy enemigo = new Enemy("Drakula Oscuro", 20, 110, 'M');
            enemigo.EquippedWeapon = new Sword("Espada Sombra", 22, 12);
            enemigo.Skills = new Skill[]
            {
                new Skill("Mordida Vampírica", 0, 28, SkillType.Damage)
            };

            PrintColor("¡Un enemigo salvaje ha aparecido!", ConsoleColor.Red);
            if (enemigo.EquippedWeapon != null)
            {
                PrintColor($"Enemigo: {enemigo.NameChar} | Vida: {enemigo.LifePoint} | Arma: {enemigo.EquippedWeapon.Name}", ConsoleColor.Red);
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
                PrintColor($"[TÚ]      {jugador.NameChar} - Vida: {jugador.LifePoint}", ConsoleColor.Cyan);
                if (jugador is Wizard wiz)
                {
                    PrintColor($"          Maná: {wiz.Manna}", ConsoleColor.Cyan);
                }
                PrintColor($"[ENEMIGO] {enemigo.NameChar} - Vida: {enemigo.LifePoint}", ConsoleColor.Red);
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
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        jugador.UseSkill(numSkill - 1, target: enemigo);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("Opción de habilidad inválida, perdiste tu turno.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"-> {jugador.NameChar} ataca:");
                    jugador.Attack();
                    int danioTotal = jugador.Attackpower;
                    if (jugador.EquippedWeapon != null)
                    {
                        danioTotal = danioTotal + jugador.EquippedWeapon.BaseDamage;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    enemigo.TakeDamage(danioTotal);
                    Console.ResetColor();
                }

                if (enemigo.LifePoint <= 0)
                {
                    PrintColor($"\n==================================================\n¡VICTORIA! Has vencido a {enemigo.NameChar}.\n==================================================", ConsoleColor.Green);
                    break;
                }

                // Turno del enemigo
                PrintColor($"\n--- Turno de {enemigo.NameChar} ---", ConsoleColor.Red);
                if (turno % 2 == 0 && enemigo.Skills.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"-> {enemigo.NameChar} utiliza una habilidad especial contra ti:");
                    enemigo.UseSkill(0, target: jugador);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    enemigo.Attack();
                    int danioEnemigo = enemigo.Attackpower;
                    if (enemigo.EquippedWeapon != null)
                    {
                        danioEnemigo = danioEnemigo + enemigo.EquippedWeapon.BaseDamage;
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    jugador.TakeDamage(danioEnemigo);
                    Console.ResetColor();
                }

                if (jugador.LifePoint <= 0)
                {
                    PrintColor("\n==================================================\nDERROTA: Has caído en batalla...\n==================================================", ConsoleColor.Red);
                    break;
                }

                turno++;
            }

            Console.WriteLine("\nFin del juego.");
        }
    }
}
```

</details>
