# ⚔️ Mini RPG - Guía Educativa paso a paso para crear `Program.cs` (POO en C#)

Este repositorio contiene un proyecto de consola en **C# (.NET 10)** diseñado para practicar **Programación Orientada a Objetos (POO)**: abstracción, herencia, encapsulación, polimorfismo, enumeraciones y composición/agregación.

En esta guía aprenderás **paso a paso** cómo construir desde cero el archivo [`RPG/Program.cs`](RPG/Program.cs) para conectar todas las clases del juego ([`RPG/Clases/`](RPG/Clases/)) y dar vida a un combate por turnos.

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

Desde la raíz del repositorio:

```bash
dotnet run --project RPG/RPG.csproj
```

---

## 🛠️ Paso a paso para construir `RPG/Program.cs`

### Paso 0: Importar namespaces y crear la estructura base

Como nuestras clases están repartidas en distintos *namespaces* (`Parrex`, `Mappe`, `harryppoter`, `Tetranova`), lo primero es importarlos con `using` para poder instanciar `Warrior`, `Wizard` y `Enemy` dentro del namespace `Parrex`.

```csharp
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

            // Aquí iremos agregando cada bloque del juego...
        }
    }
}
```

> 💡 **Concepto clave:** Los `using` permiten que `Program.cs` (en `Parrex`) reconozca clases definidas en otros namespaces sin tener que escribir su nombre completo cada vez.

---

### Paso 1: Selección de personaje (Polimorfismo y arreglos de habilidades)

Queremos que el usuario elija entre ser un **Guerrero** o un **Mago**. Para no duplicar el resto del juego según la elección, declaramos una variable del tipo padre abstracto: `Character jugador;`.

```csharp
// 1. SELECCIÓN DE PERSONAJE
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
```

> 💡 **Conceptos clave:**
> - **Polimorfismo:** `jugador` es de tipo `Character`, pero en tiempo de ejecución apunta a un objeto `Wizard` o `Warrior`.
> - **Agregación con arreglos:** Cada personaje recibe un arreglo `Skill[]` con sus habilidades propias.
> - **Enumeraciones (`enum`):** `Listelement.fire` y `SkillType.Damage` evitan errores de escritura usando valores cerrados y legibles.

---

### Paso 2: Armería y validación polimórfica al equipar (`EquipWeapon`)

Creamos un inventario de armas usando un arreglo polimórfico `Weapon[]` que mezcla objetos `Sword`, `Axe` y `SpellBook`. Después, usamos un bucle `while` hasta que el jugador elija un arma válida para su clase.

```csharp
// 2. SELECCIÓN Y VALIDACIÓN DE ARMA SEGÚN EL PESO
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
```

> 💡 **¿Por qué `jugador.EquipWeapon(armaElegida)` se comporta distinto según el héroe?**
> Porque `EquipWeapon` es un método `virtual` en `Character` y está sobreescrito (`override`) en las subclases:
> - **En `Warrior`:** Rechaza cualquier `SpellBook` (no usa maná) y exige `force >= weapon.Weight` (por ejemplo, Titan con fuerza `25` no puede levantar el *Hacha Titánica Colosal* de peso `32`).
> - **En `Wizard`:** Acepta cualquier `SpellBook` o armas ligeras con `Weight <= 10`, pero rechaza las hachas pesadas.
> - **En `jugador.Info()`:** También aplica polimorfismo, mostrando la información base más los atributos propios de `Warrior` o `Wizard`.

---

### Paso 3: Creación del enemigo (`Enemy`)

Ahora instanciamos al rival usando la clase `Enemy`, que también hereda de `Character`, y configuramos sus propiedades `EquippedWeapon` y `Skills`.

```csharp
// 3. CREACIÓN DEL ENEMIGO
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
```

> 💡 **Concepto clave:** `Enemy` no tuvo que volver a programar `NameChar`, `LifePoint`, `EquippedWeapon`, `Skills` ni `TakeDamage()`; los reutiliza directamente por **herencia** desde `Character`.

---

### Paso 4: El bucle de combate por turnos (`while`)

El combate continúa mientras ambos personajes tengan vida (`jugador.LifePoint > 0 && enemigo.LifePoint > 0`). En cada iteración:
1. Se muestra el estado actual (usando `jugador is Wizard wiz` para mostrar el maná solo si el jugador es un mago).
2. El jugador decide entre atacar con su arma (`jugador.Attack()`) o usar una habilidad (`jugador.UseSkill(...)`).
3. Si el enemigo cae (`enemigo.LifePoint <= 0`), termina el combate con victoria.
4. Si sigue vivo, el enemigo toma su turno (alternando su habilidad en turnos pares y ataque básico en impares).
5. Se verifica si el jugador cayó y se incrementa el contador `turno`.

```csharp
int turno = 1;

// 4. BUCLE DE COMBATE POR TURNOS
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
```

> 💡 **Conceptos clave en el bucle de combate:**
> - **Pattern Matching (`is`):** `if (jugador is Wizard wiz)` verifica de forma segura si el `Character` actual es un `Wizard` para acceder a `wiz.Manna`.
> - **Encapsulación:** Nunca modificamos `lifePoint` directamente desde `Program.cs`; usamos `TakeDamage(...)` y `UseSkill(...)` para que cada objeto controle sus propias reglas (como no bajar de 0 ni exceder la vida máxima, y descontar el maná).
> - **Seguridad ante nulos:** Antes de sumar `BaseDamage`, comprobamos `if (jugador.EquippedWeapon != null)` para evitar errores de referencia nula.

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
```

</details>
