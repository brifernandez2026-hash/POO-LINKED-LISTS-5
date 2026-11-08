using Backend;

Console.WriteLine("TALLER 5 - LISTAS DOBLEMENTE LIGADAS");
Console.WriteLine("Selecciona el tipo de dato:");
Console.WriteLine("1. Numeros enteros");
Console.WriteLine("2. Palabras");

int type;

while (true)
{
    Console.Write("Opcion: ");

    if (int.TryParse(Console.ReadLine(), out type)
        && (type == 1 || type == 2))
        break;

    Console.WriteLine("Selecciona 1 o 2.");
}

if (type == 1)
    RunMenu<int>(int.Parse);
else
    RunMenu<string>(text => text);


static void RunMenu<T>(Func<string, T> convert)
    where T : notnull, IComparable<T>
{
    var list = new DoublyLinkedList<T>();

    while (true)
    {
        Console.WriteLine("\n----- MENU -----");
        Console.WriteLine("1. Adicionar");
        Console.WriteLine("2. Mostrar hacia adelante");
        Console.WriteLine("3. Mostrar hacia atras");
        Console.WriteLine("4. Ordenar descendentemente");
        Console.WriteLine("5. Mostrar la(s) moda(s)");
        Console.WriteLine("6. Mostrar grafico");
        Console.WriteLine("7. Existe");
        Console.WriteLine("8. Eliminar una ocurrencia");
        Console.WriteLine("9. Eliminar todas las ocurrencias");
        Console.WriteLine("0. Salir");
        Console.Write("Elige una opcion: ");

        if (!int.TryParse(Console.ReadLine(), out int option))
        {
            Console.WriteLine("Debes escribir un numero del menu.");
            continue;
        }

        switch (option)
        {
            case 1:
                list.Add(ReadValue(convert));
                Console.WriteLine("Elemento agregado.");
                break;

            case 2:
                PrintList(list.Forward());
                break;

            case 3:
                PrintList(list.Backward());
                break;

            case 4:
                list.SortDescending();
                Console.WriteLine("Lista en orden descendente:");
                PrintList(list.Forward());
                break;

            case 5:
                var modes = list.GetModes();

                Console.WriteLine(modes.Count == 0
                    ? "La lista esta vacia."
                    : $"Moda(s): {string.Join(", ", modes)}");
                break;

            case 6:
                var frequencies = list.GetFrequencies();

                if (frequencies.Count == 0)
                {
                    Console.WriteLine("La lista esta vacia.");
                    break;
                }

                Console.WriteLine("Grafico de ocurrencias:");

                foreach (var item in frequencies)
                {
                    Console.WriteLine(
                        $"{item.Key} {new string('*', item.Value)}");
                }
                break;

            case 7:
                Console.WriteLine(list.Exists(ReadValue(convert))
                    ? "Si existe en la lista."
                    : "No existe en la lista.");
                break;

            case 8:
                Console.WriteLine(list.RemoveFirst(ReadValue(convert))
                    ? "Se elimino la primera ocurrencia."
                    : "No se encontro el elemento.");
                break;

            case 9:
                int removed = list.RemoveAll(ReadValue(convert));

                Console.WriteLine(
                    $"Se eliminaron {removed} ocurrencia(s).");
                break;

            case 0:
                Console.WriteLine("Fin del programa.");
                return;

            default:
                Console.WriteLine("Opcion no valida.");
                break;
        }
    }
}

static T ReadValue<T>(Func<string, T> convert)
{
    while (true)
    {
        Console.Write("Ingresa el valor: ");
        string text = (Console.ReadLine() ?? "").Trim();

        if (text.Length == 0)
        {
            Console.WriteLine("El valor no puede estar vacio.");
            continue;
        }

        try
        {
            return convert(text);
        }
        catch (FormatException)
        {
            Console.WriteLine("Formato incorrecto.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Numero fuera del rango permitido.");
        }
    }
}

static void PrintList<T>(IEnumerable<T> values)
{
    var text = string.Join(", ", values);

    Console.WriteLine(text.Length == 0
        ? "La lista esta vacia."
        : text);
}
