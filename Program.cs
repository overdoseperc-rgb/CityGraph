using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

class Program
{
    static string ReadText(string prompt)
    {
        Console.Write(prompt);
        string value = Console.ReadLine();
        if (value == null) throw new InvalidOperationException("Ввод завершён.");
        return value.Trim();
    }

    static int ReadCount(string prompt)
    {
        while (true)
        {
            int value;
            if (int.TryParse(ReadText(prompt), out value) && value >= 0) return value;
            Console.WriteLine("Введите целое неотрицательное число.");
        }
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try { Run(); }
        catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
        if (!Console.IsInputRedirected)
        {
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey(true);
        }
    }

    static void Run()
    {
        var graph = new Dictionary<string, Dictionary<string, double>>(StringComparer.OrdinalIgnoreCase);
        string[] cities = { "Казань", "Москва", "Сочи", "Санкт-Петербург", "Чебоксары" };
        Console.WriteLine("Доступные города:");
        foreach (string city in cities)
        {
            graph[city] = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("  " + city);
        }
        Console.WriteLine("Дороги двусторонние.");
        int roads = ReadCount("Количество дорог: ");
        for (int i = 0; i < roads; i++)
        {
            Console.WriteLine("Дорога " + (i + 1));
            string from = ReadText("Первый город: ");
            string to = ReadText("Второй город: ");
            string input = ReadText("Расстояние (км): ").Replace(',', '.');
            double distance;
            if (!graph.ContainsKey(from) || !graph.ContainsKey(to))
            {
                Console.WriteLine("Город не найден."); i--; continue;
            }
            if (string.Equals(from, to, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Выберите разные города."); i--; continue;
            }
            if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out distance)
                || double.IsNaN(distance) || double.IsInfinity(distance) || distance <= 0)
            {
                Console.WriteLine("Введите положительное конечное расстояние."); i--; continue;
            }
            graph[from][to] = distance;
            graph[to][from] = distance;
        }
        Console.WriteLine("\nСписки смежности:");
        foreach (var city in graph)
        {
            Console.WriteLine(city.Key + ":");
            if (city.Value.Count == 0) Console.WriteLine("  Нет дорог.");
            foreach (var neighbor in city.Value)
                Console.WriteLine("  " + neighbor.Key + " — " + neighbor.Value + " км");
        }
    }
}
