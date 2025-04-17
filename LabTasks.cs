using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace LabWork
{
    /// <summary>
    /// Structure describing passenger baggage item.
    /// </summary>
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

    /// <summary>
    /// Class containing static methods solving lab tasks 1-10 and extended tests.
    /// </summary>
    public class LabTasks
    {
        private static Random _random = new Random();

        #region Task 1

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

        #endregion

        #region Task 2

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

        #endregion

        #region Task 3

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

        #endregion

        #region Task 4

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

        #endregion

        #region Task 5

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

        #endregion

        #region Task 6

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

        #endregion

        #region Task 7

        public static bool task7HasEqualNext(LinkedList<int> list)
        {
            if (list.Count == 0)
                return false;

            LinkedListNode<int>? current = list.First;
            while (current != null)
            {
                LinkedListNode<int>? next = current.Next ?? list.First;
                if (next == null)
                    return false;
              
                if (current.Value == next.Value)
                    return true;

                current = current.Next;
            }

            return false;
        }

        #endregion

        #region Task 8

        public static void task8AnalyzeVisits(List<HashSet<string>> touristsCountries, HashSet<string> allCountries)
        {
            if (touristsCountries == null || touristsCountries.Count == 0)
            {
                Console.WriteLine("No tourist data.");
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

            Console.WriteLine("Countries visited by all tourists:");
            foreach (string country in visitedByAll)
                Console.WriteLine(country);

            Console.WriteLine("\nCountries visited by some tourists:");
            foreach (string country in visitedBySome)
                Console.WriteLine(country);

            Console.WriteLine("\nCountries visited by no tourists:");
            foreach (string country in visitedByNone)
                Console.WriteLine(country);
        }

        #endregion

        #region Task 9

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

            Console.WriteLine("Deaf consonants not appearing in exactly one word:");
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

        #endregion

        #region Task 10

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

                    surnames[count] = parts[0];
                    names[count] = parts[1];

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

        #endregion

        #region Extended Test Data Generators

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

        #endregion

        #region Extended Tests Demo

        public static void runExtendedTests()
        {
            Console.WriteLine("=== Extended Tests ===");

            string task1File = "task1_extended.txt";
            fillFileTask1WithZero(task1File);
            Console.WriteLine("Task 1 (with zero): No zero? " + task1NoZeroInFile(task1File));

            string task2File = "task2_extended.txt";
            fillFileTask2WithMax(task2File);
            Console.WriteLine("Task 2 (with max): Max element = " + task2MaxElement(task2File));

            string task3File = "task3_extended.txt";
            fillFileTask3Extended(task3File);
            string task3Dest = "task3_extended_dest.txt";
            task3CopyLinesEndingWithChar(task3File, task3Dest, '!');
            Console.WriteLine("Task 3 (lines ending with '!'):");
            using (var reader = new StreamReader(task3Dest))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                    Console.WriteLine(line);
            }

            string task4File = "task4_extended.bin";
            fillBinaryFileTask4WithPairs(task4File);
            Console.WriteLine("Task 4 (known pairs): Opposite pairs count = " + task4CountOppositePairs(task4File));

            string task5File = "task5_extended.xml";
            fillBinaryFileTask5Extended(task5File);
            Console.WriteLine("Task 5 (extended): Max-min baggage weight difference = " + task5MaxMinWeightDifference(task5File));

            var list6 = getListTask6Extended();
            Console.WriteLine("Task 6 (before): " + string.Join(", ", list6));
            task6RemoveAfterE(list6, 2);
            Console.WriteLine("Task 6 (after): " + string.Join(", ", list6));

            var linkedList7NoEqual = getLinkedListTask7NoEqual();
            var linkedList7WithEqual = getLinkedListTask7WithEqual();
            Console.WriteLine("Task 7 (no equal): " + task7HasEqualNext(linkedList7NoEqual));
            Console.WriteLine("Task 7 (with equal): " + task7HasEqualNext(linkedList7WithEqual));

            Console.WriteLine("Task 8 (extended):");
            runTask8Extended();

            string task9File = "task9_extended.txt";
            fillFileTask9Extended(task9File);
            Console.WriteLine("Task 9 (extended):");
            task9PrintDeafConsonantsNotInOneWord(task9File);

            string task10File = "task10_extended.txt";
            fillFileTask10Extended(task10File);
            Console.WriteLine("Task 10 (extended): Failed applicants:");
            task10PrintFailedApplicants(task10File);

            Console.WriteLine("=== End of Extended Tests ===");
        }

        #endregion

        #region Main method for demonstration

        public static void Main()
        {
            // !!! Для базового теста раскомментировать строку ниже
             RunBasicDemo();

            // !!! Для расширенного теста раскоментировать строку ниже
            // runExtendedTests();
        }

        private static void RunBasicDemo()
        {
            string fileTask1 = "task1.txt";
            string fileTask2 = "task2.txt";
            string fileTask3Source = "task3_source.txt";
            string fileTask3Dest = "task3_dest.txt";
            string fileTask4 = "task4.bin";
            string fileTask5 = "task5.xml";
            string fileTask9 = "task9.txt";
            string fileTask10 = "task10.txt";

            fillFileTask1(fileTask1, 20, -5, 10);
            Console.WriteLine("Task 1: No zero? " + task1NoZeroInFile(fileTask1));

            fillFileTask2(fileTask2, 5, 6, -10, 20);
            Console.WriteLine("Task 2: Max element = " + task2MaxElement(fileTask2));

            using (var writer = new StreamWriter(fileTask3Source, false, Encoding.UTF8))
            {
                writer.WriteLine("Hello!");
                writer.WriteLine("World.");
                writer.WriteLine("Test!");
                writer.WriteLine("Example.");
                writer.WriteLine("End!");
            }
            task3CopyLinesEndingWithChar(fileTask3Source, fileTask3Dest, '!');
            Console.WriteLine("Task 3: Lines ending with '!':");
            using (var reader = new StreamReader(fileTask3Dest))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                    Console.WriteLine(line);
            }

            fillBinaryFileTask4(fileTask4, 10, -10, 10);
            Console.WriteLine("Task 4: Opposite pairs count = " + task4CountOppositePairs(fileTask4));

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
            fillBinaryFileTask5(fileTask5, passengers);
            Console.WriteLine("Task 5: Max-min baggage weight difference = " + task5MaxMinWeightDifference(fileTask5));

            var list6 = new List<int> { 1, 2, 3, 2, 4, 2, 2, 5 };
            Console.WriteLine("Task 6: Before removal: " + string.Join(", ", list6));
            task6RemoveAfterE(list6, 2);
            Console.WriteLine("Task 6: After removal: " + string.Join(", ", list6));

            var linkedList7 = new LinkedList<int>(new int[] { 1, 2, 2, 3, 4 });
            Console.WriteLine("Task 7: Has equal next? " + task7HasEqualNext(linkedList7));

            var tourists = new List<HashSet<string>>
            {
                new HashSet<string> { "Russia", "France", "Germany" },
                new HashSet<string> { "Russia", "Spain" },
                new HashSet<string> { "France", "Spain", "Italy" }
            };

            var allCountries = new HashSet<string> { "Russia", "France", "Germany", "Spain", "Italy", "Portugal" };

            Console.WriteLine("Task 8:");
            task8AnalyzeVisits(tourists, allCountries);

            using (var writer = new StreamWriter(fileTask9, false, Encoding.UTF8))
            {
                writer.WriteLine("Peter went to the park with the dog.");
                writer.WriteLine("Fedor likes to read books.");
                writer.WriteLine("The cat sleeps on the roof.");
                writer.WriteLine("Timofey plays chess.");
                writer.WriteLine("The school is closed.");
            }
            Console.WriteLine("Task 9:");
            task9PrintDeafConsonantsNotInOneWord(fileTask9);

            using (var writer = new StreamWriter(fileTask10, false, Encoding.UTF8))
            {
                writer.WriteLine("5");
                writer.WriteLine("Vetrov Roman 68 59");
                writer.WriteLine("Anisimova Ekaterina 64 88");
                writer.WriteLine("Ivanov Ivan 20 40");
                writer.WriteLine("Petrov Petr 30 29");
                writer.WriteLine("Sidorov Sidor 10 10");
            }
            Console.WriteLine("Task 10: Failed applicants:");
            task10PrintFailedApplicants(fileTask10);
        }

        #endregion
    }
}
