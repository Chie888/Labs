using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

public struct Baggage
{
    public string Name;
    public double Weight;

    public Baggage(string name, double weight)
    {
        Name = name;
        Weight = weight;
    }
}

public class LabTasks
{
    private static Random _random = new Random();

    // --- Решения задач (твой исходный код, без изменений) ---

    public static void fillFileTask1(string path, int count, int minValue, int maxValue)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            for (int i = 0; i < count; i++)
            {
                int value = _random.Next(minValue, maxValue + 1);
                writer.WriteLine(value);
            }
        }
    }

    public static bool task1NoZeroInFile(string path)
    {
        using (var reader = new StreamReader(path, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (int.TryParse(line.Trim(), out int value) && value == 0)
                    return false;
            }
        }
        return true;
    }

    public static void fillFileTask2(string path, int linesCount, int numbersPerLine, int minValue, int maxValue)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            for (int i = 0; i < linesCount; i++)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < numbersPerLine; j++)
                {
                    int value = _random.Next(minValue, maxValue + 1);
                    sb.Append(value);
                    if (j < numbersPerLine - 1)
                        sb.Append(' ');
                }
                writer.WriteLine(sb.ToString());
            }
        }
    }

    public static int task2MaxElement(string path)
    {
        int maxValue = int.MinValue;
        using (var reader = new StreamReader(path, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int value) && value > maxValue)
                        maxValue = value;
                }
            }
        }
        return maxValue;
    }

    public static void task3CopyLinesEndingWithChar(string sourcePath, string destPath, char endingChar)
    {
        using (var reader = new StreamReader(sourcePath, Encoding.UTF8))
        using (var writer = new StreamWriter(destPath, false, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0 && line[line.Length - 1] == endingChar)
                    writer.WriteLine(line);
            }
        }
    }

    public static void fillBinaryFileTask4(string path, int count, int minValue, int maxValue)
    {
        using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            for (int i = 0; i < count; i++)
            {
                int value = _random.Next(minValue, maxValue + 1);
                writer.Write(value);
            }
        }
    }

    public static int task4CountOppositePairs(string path)
    {
        int countPairs = 0;
        int[] numbers;

        using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            long length = reader.BaseStream.Length / sizeof(int);
            numbers = new int[length];
            for (int i = 0; i < length; i++)
                numbers[i] = reader.ReadInt32();
        }

        for (int i = 0; i < numbers.Length - 1; i++)
        {
            if (numbers[i] == -numbers[i + 1])
                countPairs++;
        }

        return countPairs;
    }

    public static void fillBinaryFileTask5(string path, Baggage[][] passengersBaggage)
    {
        var serializer = new XmlSerializer(typeof(Baggage[][]));
        using (var fs = new FileStream(path, FileMode.Create))
            serializer.Serialize(fs, passengersBaggage);
    }

    public static double task5MaxMinWeightDifference(string path)
    {
        var serializer = new XmlSerializer(typeof(Baggage[][]));
        Baggage[][] passengersBaggage;

        using (var fs = new FileStream(path, FileMode.Open))
            passengersBaggage = (Baggage[][])serializer.Deserialize(fs)!;

        double maxWeight = double.MinValue;
        double minWeight = double.MaxValue;

        for (int i = 0; i < passengersBaggage.Length; i++)
        {
            for (int j = 0; j < passengersBaggage[i].Length; j++)
            {
                double weight = passengersBaggage[i][j].Weight;
                if (weight > maxWeight)
                    maxWeight = weight;
                if (weight < minWeight)
                    minWeight = weight;
            }
        }

        return maxWeight - minWeight;
    }

    public static void task6RemoveAfterE(List<int> list, int e)
    {
        int i = 0;
        while (i < list.Count)
        {
            if (list[i] == e)
            {
                if (i + 1 < list.Count && list[i + 1] != e)
                    list.RemoveAt(i + 1);
                else
                    i++;
                continue;
            }
            i++;
        }
    }

    public static bool task7HasEqualNext(LinkedList<int> list)
    {
        if (list.Count == 0)
            return false;

        LinkedListNode<int>? current = list.First;
        while (current != null)
        {
            LinkedListNode<int>? next = current.Next ?? list.First!;
            if (next == null)
                return false;

            if (current.Value == next.Value)
                return true;

            current = current.Next;
        }

        return false;
    }

    public static void task8AnalyzeVisits(List<HashSet<string>> touristsCountries, HashSet<string> allCountries)
    {
        if (touristsCountries == null || touristsCountries.Count == 0)
        {
            Console.WriteLine("Нет данных о туристах.");
            return;
        }

        int touristsCount = touristsCountries.Count;

        var countryCounts = new Dictionary<string, int>();

        foreach (string country in allCountries)
        {
            int count = 0;
            for (int i = 0; i < touristsCount; i++)
            {
                if (touristsCountries[i].Contains(country))
                    count++;
            }
            countryCounts[country] = count;
        }

        var visitedByAll = new List<string>();
        var visitedBySome = new List<string>();
        var visitedByNone = new List<string>();

        foreach (var country in allCountries)
        {
            int count = countryCounts[country];
            if (count == touristsCount)
                visitedByAll.Add(country);
            else if (count > 0)
                visitedBySome.Add(country);
            else
                visitedByNone.Add(country);
        }

        Console.WriteLine("Страны, посещённые всеми туристами:");
        foreach (string country in visitedByAll)
            Console.WriteLine(country);

        Console.WriteLine("\nСтраны, посещённые некоторыми туристами:");
        foreach (string country in visitedBySome)
            Console.WriteLine(country);

        Console.WriteLine("\nСтраны, не посещённые ни одним туристом:");
        foreach (string country in visitedByNone)
            Console.WriteLine(country);
    }

    public static void task9PrintDeafConsonantsNotInOneWord(string path)
    {
        char[] deafConsonants = { 'п', 'ф', 'к', 'т', 'ш', 'с', 'х', 'ц', 'ч' };

        var letterWordCount = new Dictionary<char, int>();
        foreach (char c in deafConsonants)
            letterWordCount[c] = 0;

        using (var reader = new StreamReader(path, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] words = splitToWords(line);

                foreach (string word in words)
                {
                    string lowerWord = word.ToLowerInvariant();

                    bool[] foundInWord = new bool[deafConsonants.Length];
                    for (int i = 0; i < deafConsonants.Length; i++)
                    {
                        if (lowerWord.IndexOf(deafConsonants[i]) >= 0)
                            foundInWord[i] = true;
                    }

                    for (int i = 0; i < deafConsonants.Length; i++)
                    {
                        if (foundInWord[i])
                            letterWordCount[deafConsonants[i]]++;
                    }
                }
            }
        }

        var result = new List<char>();
        foreach (char c in deafConsonants)
        {
            if (letterWordCount[c] != 1)
                result.Add(c);
        }

        result.Sort();

        Console.WriteLine("Глухие согласные, не входящие ровно в одно слово:");
        foreach (char c in result)
            Console.WriteLine(c);
    }

    private static string[] splitToWords(string line)
    {
        var words = new List<string>();
        var sb = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char ch = line[i];
            if (isRussianLetter(ch))
                sb.Append(ch);
            else if (sb.Length > 0)
            {
                words.Add(sb.ToString());
                sb.Clear();
            }
        }
        if (sb.Length > 0)
            words.Add(sb.ToString());

        return words.ToArray();
    }

    private static bool isRussianLetter(char ch)
    {
        ch = char.ToLowerInvariant(ch);
        return (ch >= 'а' && ch <= 'я') || ch == 'ё';
    }

    public static void task10PrintFailedApplicants(string path)
    {
        int numberOfApplicants = 0;
        string[] surnames = new string[500];
        string[] names = new string[500];
        int[] scores1 = new int[500];
        int[] scores2 = new int[500];
        int count = 0;

        using (var reader = new StreamReader(path, Encoding.UTF8))
        {
            string? line = reader.ReadLine();
            if (line == null)
                return;

            if (int.TryParse(line.Trim(), out int n))
                numberOfApplicants = n;

            for (int i = 0; i < numberOfApplicants; i++)
            {
                line = reader.ReadLine();
                if (line == null)
                    break;

                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4)
                    continue;

                surnames[count] = parts[0]!;
                names[count] = parts[1]!;

                if (!int.TryParse(parts[2], out int score1))
                    score1 = 0;
                if (!int.TryParse(parts[3], out int score2))
                    score2 = 0;

                scores1[count] = score1;
                scores2[count] = score2;
                count++;
            }
        }

        var failedSurnames = new string[count];
        var failedNames = new string[count];
        int failedCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (scores1[i] < 30 || scores2[i] < 30)
            {
                failedSurnames[failedCount] = surnames[i];
                failedNames[failedCount] = names[i];
                failedCount++;
            }
        }

        for (int i = 0; i < failedCount - 1; i++)
        {
            for (int j = i + 1; j < failedCount; j++)
            {
                if (string.Compare(failedSurnames[i], failedSurnames[j], StringComparison.Ordinal) > 0)
                {
                    string tempSurname = failedSurnames[i];
                    failedSurnames[i] = failedSurnames[j];
                    failedSurnames[j] = tempSurname;

                    string tempName = failedNames[i];
                    failedNames[i] = failedNames[j];
                    failedNames[j] = tempName;
                }
            }
        }

        for (int i = 0; i < failedCount; i++)
            Console.WriteLine(failedSurnames[i] + " " + failedNames[i]);
    }

    // --- Базовые тесты ---

    public static void fillFileTask1WithZero(string path)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("1");
            writer.WriteLine("2");
            writer.WriteLine("0");
            writer.WriteLine("3");
            writer.WriteLine("4");
        }
    }

    public static void fillFileTask2WithMax(string path)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("1 5 3 7");
            writer.WriteLine("10 1000 50 20");
            writer.WriteLine("4 2 6 8");
        }
    }

    public static void fillFileTask3Extended(string path)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("Hello!");
            writer.WriteLine("World.");
            writer.WriteLine("Test!");
            writer.WriteLine("Example.");
            writer.WriteLine("End!");
            writer.WriteLine(" ");
            writer.WriteLine("");
            writer.WriteLine("NoEnd");
        }
    }

    public static void fillBinaryFileTask4WithPairs(string path)
    {
        int[] numbers = { 5, -5, 3, -3, 7, 8, -7, 0, -8, 10, -10, 1, 2 };
        using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            foreach (int num in numbers)
                writer.Write(num);
        }
    }

    public static void fillBinaryFileTask5Extended(string path)
    {
        Baggage[][] passengers = new Baggage[3][];
        passengers[0] = new Baggage[]
        {
            new Baggage("Suitcase", 15.5),
            new Baggage("Bag", 7.2)
        };
        passengers[1] = new Baggage[]
        {
            new Baggage("Box", 50.0),
            new Baggage("Backpack", 5.0),
            new Baggage("Bag", 3.3)
        };
        passengers[2] = new Baggage[0];

        fillBinaryFileTask5(path, passengers);
    }

    public static List<int> getListTask6Extended()
    {
        return new List<int> { 2, 2, 3, 2, 2, 4, 2, 5, 2 };
    }

    public static LinkedList<int> getLinkedListTask7NoEqual()
    {
        return new LinkedList<int>(new int[] { 1, 2, 3, 4, 5 });
    }

    public static LinkedList<int> getLinkedListTask7WithEqual()
    {
        return new LinkedList<int>(new int[] { 1, 2, 2, 3, 4 });
    }

    public static void runTask8Extended()
    {
        var tourists = new List<HashSet<string>>
        {
            new HashSet<string> { "Russia", "France", "Germany" },
            new HashSet<string> { "Russia", "Spain" },
            new HashSet<string>()
        };

        var allCountries = new HashSet<string> { "Russia", "France", "Germany", "Spain", "Italy", "Portugal" };

        task8AnalyzeVisits(tourists, allCountries);
    }

    public static void fillFileTask9Extended(string path)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("Пётр пошёл в парк с собакой.");
            writer.WriteLine("Фёдор любит читать книги.");
            writer.WriteLine("Кот спит на крыше.");
            writer.WriteLine("Тимофей играет в шахматы.");
            writer.WriteLine("Школа закрыта.");
            writer.WriteLine("Пфк");
            writer.WriteLine("Пфк пфк");
        }
    }

    public static void fillFileTask10Extended(string path)
    {
        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("7");
            writer.WriteLine("Vetrov Roman 30 30");
            writer.WriteLine("Anisimova Ekaterina 29 88");
            writer.WriteLine("Ivanov Ivan 20 40");
            writer.WriteLine("Petrov Petr 30 29");
            writer.WriteLine("Sidorov Sidor 10 10");
            writer.WriteLine("Smith John 50 50");
            writer.WriteLine("InvalidLine");
        }
    }

    public static void runExtendedTests()
    {
        Console.WriteLine("=== Расширенные тесты ===");

        string task1File = "task1_extended.txt";
        fillFileTask1WithZero(task1File);
        Console.WriteLine("Задание 1 (с нулём): Нет нуля? " + task1NoZeroInFile(task1File));

        string task2File = "task2_extended.txt";
        fillFileTask2WithMax(task2File);
        Console.WriteLine("Задание 2 (с максимальным элементом): Максимум = " + task2MaxElement(task2File));

        string task3File = "task3_extended.txt";
        fillFileTask3Extended(task3File);
        string task3Dest = "task3_extended_dest.txt";
        task3CopyLinesEndingWithChar(task3File, task3Dest, '!');
        Console.WriteLine("Задание 3 (строки, оканчивающиеся на '!'):");
        using (var reader = new StreamReader(task3Dest))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
                Console.WriteLine(line);
        }

        string task4File = "task4_extended.bin";
        fillBinaryFileTask4WithPairs(task4File);
        Console.WriteLine("Задание 4 (известные пары): Кол-во пар = " + task4CountOppositePairs(task4File));

        string task5File = "task5_extended.xml";
        fillBinaryFileTask5Extended(task5File);
        Console.WriteLine("Задание 5 (расширенное): Разница веса = " + task5MaxMinWeightDifference(task5File));

        var list6 = getListTask6Extended();
        Console.WriteLine("Задание 6 (до): " + string.Join(", ", list6));
        task6RemoveAfterE(list6, 2);
        Console.WriteLine("Задание 6 (после): " + string.Join(", ", list6));

        var linkedList7NoEqual = getLinkedListTask7NoEqual();
        var linkedList7WithEqual = getLinkedListTask7WithEqual();
        Console.WriteLine("Задание 7 (без равных): " + task7HasEqualNext(linkedList7NoEqual));
        Console.WriteLine("Задание 7 (с равными): " + task7HasEqualNext(linkedList7WithEqual));

        Console.WriteLine("Задание 8 (расширенное):");
        runTask8Extended();

        string task9File = "task9_extended.txt";
        fillFileTask9Extended(task9File);
        Console.WriteLine("Задание 9 (расширенное):");
        task9PrintDeafConsonantsNotInOneWord(task9File);

        string task10File = "task10_extended.txt";
        fillFileTask10Extended(task10File);
        Console.WriteLine("Задание 10 (расширенное): Отчисленные абитуриенты:");
        task10PrintFailedApplicants(task10File);

        Console.WriteLine("=== Конец расширенных тестов ===");
    }

    public static void runBasicDemo()
    {
        Console.WriteLine("=== Базовые тесты ===");

        string fileTask1 = "task1.txt";
        fillFileTask1(fileTask1, 20, -5, 10);
        Console.WriteLine("Задание 1: Нет нуля? " + task1NoZeroInFile(fileTask1));

        string fileTask2 = "task2.txt";
        fillFileTask2(fileTask2, 5, 6, -10, 20);
        Console.WriteLine("Задание 2: Максимум = " + task2MaxElement(fileTask2));

        using (var writer = new StreamWriter("task3_source.txt", false, Encoding.UTF8))
        {
            writer.WriteLine("Hello!");
            writer.WriteLine("World.");
            writer.WriteLine("Test!");
            writer.WriteLine("Example.");
            writer.WriteLine("End!");
        }
        task3CopyLinesEndingWithChar("task3_source.txt", "task3_dest.txt", '!');
        Console.WriteLine("Задание 3: Строки, оканчивающиеся на '!':");
        using (var reader = new StreamReader("task3_dest.txt"))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
                Console.WriteLine(line);
        }

        fillBinaryFileTask4("task4.bin", 10, -10, 10);
        Console.WriteLine("Задание 4: Кол-во пар противоположных чисел = " + task4CountOppositePairs("task4.bin"));

        Baggage[][] passengers = new Baggage[2][];
        passengers[0] = new Baggage[]
        {
            new Baggage("Suitcase", 15.5),
            new Baggage("Bag", 7.2)
        };
        passengers[1] = new Baggage[]
        {
            new Baggage("Box", 12.0),
            new Baggage("Backpack", 5.0),
            new Baggage("Bag", 3.3)
        };
        fillBinaryFileTask5("task5.xml", passengers);
        Console.WriteLine("Задание 5: Разница веса багажа = " + task5MaxMinWeightDifference("task5.xml"));

        var list6 = new List<int> { 1, 2, 3, 2, 4, 2, 2, 5 };
        Console.WriteLine("Задание 6: До удаления: " + string.Join(", ", list6));
        task6RemoveAfterE(list6, 2);
        Console.WriteLine("Задание 6: После удаления: " + string.Join(", ", list6));

        var linkedList7 = new LinkedList<int>(new int[] { 1, 2, 2, 3, 4 });
        Console.WriteLine("Задание 7: Есть равные соседние? " + task7HasEqualNext(linkedList7));

        var tourists = new List<HashSet<string>>
        {
            new HashSet<string> { "Russia", "France", "Germany" },
            new HashSet<string> { "Russia", "Spain" },
            new HashSet<string> { "France", "Spain", "Italy" }
        };

        var allCountries = new HashSet<string> { "Russia", "France", "Germany", "Spain", "Italy", "Portugal" };

        Console.WriteLine("Задание 8:");
        task8AnalyzeVisits(tourists, allCountries);

        using (var writer = new StreamWriter("task9.txt", false, Encoding.UTF8))
        {
            writer.WriteLine("Пётр пошёл в парк с собакой.");
            writer.WriteLine("Фёдор любит читать книги.");
            writer.WriteLine("Кот спит на крыше.");
            writer.WriteLine("Тимофей играет в шахматы.");
            writer.WriteLine("Школа закрыта.");
        }
        Console.WriteLine("Задание 9:");
        task9PrintDeafConsonantsNotInOneWord("task9.txt");

        using (var writer = new StreamWriter("task10.txt", false, Encoding.UTF8))
        {
            writer.WriteLine("5");
            writer.WriteLine("Vetrov Roman 68 59");
            writer.WriteLine("Anisimova Ekaterina 64 88");
            writer.WriteLine("Ivanov Ivan 20 40");
            writer.WriteLine("Petrov Petr 30 29");
            writer.WriteLine("Sidorov Sidor 10 10");
        }
        Console.WriteLine("Задание 10: Отчисленные абитуриенты:");
        task10PrintFailedApplicants("task10.txt");

        Console.WriteLine("=== Конец базовых тестов ===");
    }

    // --- Интерактивный интерфейс ---

    public static void RunInteractive()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите задание (1-10), 11 - базовые тесты, 12 - расширенные тесты, 0 - выход:");
            int choice = InputHelper.ReadInt("Ваш выбор: ", 0, 12);
            if (choice == 0) break;

            switch (choice)
            {
                case 1:
                    RunTask1Interactive();
                    break;
                case 2:
                    RunTask2Interactive();
                    break;
                case 3:
                    RunTask3Interactive();
                    break;
                case 4:
                    RunTask4Interactive();
                    break;
                case 5:
                    RunTask5Interactive();
                    break;
                case 6:
                    RunTask6Interactive();
                    break;
                case 7:
                    RunTask7Interactive();
                    break;
                case 8:
                    RunTask8Interactive();
                    break;
                case 9:
                    RunTask9Interactive();
                    break;
                case 10:
                    RunTask10Interactive();
                    break;
                case 11:
                    runBasicDemo();
                    break;
                case 12:
                    runExtendedTests();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }

    private static void RunTask1Interactive()
    {
        Console.WriteLine("Задание 1: Проверка отсутствия нуля в файле.");

        string path = InputHelper.ReadString("Введите имя файла: ");
        int count = InputHelper.ReadInt("Введите количество чисел: ", 1);
        int min = InputHelper.ReadInt("Введите минимальное значение: ");
        int max = InputHelper.ReadInt("Введите максимальное значение: ", min);

        fillFileTask1(path, count, min, max);

        bool noZero = task1NoZeroInFile(path);
        Console.WriteLine(noZero ? "В файле нет нулей." : "В файле есть нули.");
    }

    private static void RunTask2Interactive()
    {
        Console.WriteLine("Задание 2: Поиск максимального элемента в файле.");

        string path = InputHelper.ReadString("Введите имя файла: ");
        int linesCount = InputHelper.ReadInt("Введите количество строк: ", 1);
        int numbersPerLine = InputHelper.ReadInt("Введите количество чисел в строке: ", 1);
        int min = InputHelper.ReadInt("Введите минимальное значение: ");
        int max = InputHelper.ReadInt("Введите максимальное значение: ", min);

        fillFileTask2(path, linesCount, numbersPerLine, min, max);

        int maxElement = task2MaxElement(path);
        Console.WriteLine($"Максимальный элемент в файле: {maxElement}");
    }

    private static void RunTask3Interactive()
    {
        Console.WriteLine("Задание 3: Копирование строк, оканчивающихся на заданный символ.");

        string sourcePath = InputHelper.ReadString("Введите имя исходного файла: ");
        string destPath = InputHelper.ReadString("Введите имя файла для записи: ");
        char endingChar = InputHelper.ReadChar("Введите символ окончания строки: ");

        task3CopyLinesEndingWithChar(sourcePath, destPath, endingChar);

        Console.WriteLine($"Строки, оканчивающиеся на '{endingChar}', скопированы в файл {destPath}.");
    }

    private static void RunTask4Interactive()
    {
        Console.WriteLine("Задание 4: Подсчёт пар противоположных чисел в бинарном файле.");

        string path = InputHelper.ReadString("Введите имя бинарного файла: ");
        int count = InputHelper.ReadInt("Введите количество чисел: ", 1);
        int min = InputHelper.ReadInt("Введите минимальное значение: ");
        int max = InputHelper.ReadInt("Введите максимальное значение: ", min);

        fillBinaryFileTask4(path, count, min, max);

        int pairsCount = task4CountOppositePairs(path);
        Console.WriteLine($"Количество пар противоположных чисел: {pairsCount}");
    }

    private static void RunTask5Interactive()
    {
        Console.WriteLine("Задание 5: Разница между максимальным и минимальным весом багажа.");

        string path = InputHelper.ReadString("Введите имя XML файла с багажом: ");

        double diff = task5MaxMinWeightDifference(path);
        Console.WriteLine($"Разница между максимальным и минимальным весом: {diff}");
    }

    private static void RunTask6Interactive()
    {
        Console.WriteLine("Задание 6: Удаление элементов после заданного.");

        int listSize = InputHelper.ReadInt("Введите количество элементов списка: ", 1);
        var list = new List<int>();
        for (int i = 0; i < listSize; i++)
        {
            int val = InputHelper.ReadInt($"Введите элемент {i + 1}: ");
            list.Add(val);
        }

        int e = InputHelper.ReadInt("Введите элемент E: ");

        Console.WriteLine("Список до удаления: " + string.Join(", ", list));
        task6RemoveAfterE(list, e);
        Console.WriteLine("Список после удаления: " + string.Join(", ", list));
    }

    private static void RunTask7Interactive()
    {
        Console.WriteLine("Задание 7: Проверка равенства соседних элементов в связном списке.");

        int listSize = InputHelper.ReadInt("Введите количество элементов связного списка: ", 1);
        var linkedList = new LinkedList<int>();
        for (int i = 0; i < listSize; i++)
        {
            int val = InputHelper.ReadInt($"Введите элемент {i + 1}: ");
            linkedList.AddLast(val);
        }

        bool hasEqualNext = task7HasEqualNext(linkedList);
        Console.WriteLine(hasEqualNext ? "Есть равные соседние элементы." : "Равных соседних элементов нет.");
    }

    private static void RunTask8Interactive()
    {
        Console.WriteLine("Задание 8: Анализ посещённых стран туристами.");

        int touristsCount = InputHelper.ReadInt("Введите количество туристов: ", 1);
        var tourists = new List<HashSet<string>>();
        for (int i = 0; i < touristsCount; i++)
        {
            Console.WriteLine($"Введите страны, посещённые туристом {i + 1} (через запятую):");
            string input = InputHelper.ReadString("> ");
            var countries = new HashSet<string>(input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            tourists.Add(countries);
        }

        Console.WriteLine("Введите полный список стран (через запятую):");
        string allCountriesInput = InputHelper.ReadString("> ");
        var allCountries = new HashSet<string>(allCountriesInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        task8AnalyzeVisits(tourists, allCountries);
    }

    private static void RunTask9Interactive()
    {
        Console.WriteLine("Задание 9: Вывод глухих согласных, не входящих ровно в одно слово.");

        string path = InputHelper.ReadString("Введите имя текстового файла: ");
        task9PrintDeafConsonantsNotInOneWord(path);
    }

    private static void RunTask10Interactive()
    {
        Console.WriteLine("Задание 10: Вывод абитуриентов, не допущенных к экзаменам.");

        string path = InputHelper.ReadString("Введите имя файла с данными абитуриентов: ");
        task10PrintFailedApplicants(path);
    }
}
