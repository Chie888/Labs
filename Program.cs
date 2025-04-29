using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CafeMenuApp
{
    public static class MenuRepository
    {
        private static readonly string filePath = "menu.bin";

        public static void Save(List<MenuItem> items)
        {
            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                using var bw = new BinaryWriter(fs, Encoding.GetEncoding(1251));

                bw.Write(items.Count);
                foreach (var item in items)
                {
                    bw.Write(item.Id);
                    bw.Write(item.Name);
                    bw.Write(item.Category);
                    bw.Write(item.Price);
                    bw.Write(item.Weight);
                    bw.Write(item.IsAvailable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении: " + ex.Message);
            }
        }

        public static List<MenuItem> Load()
        {
            var items = new List<MenuItem>();
            if (!File.Exists(filePath))
                return items;

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs, Encoding.GetEncoding(1251));

                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int id = br.ReadInt32();
                    string name = br.ReadString();
                    string category = br.ReadString();
                    decimal price = br.ReadDecimal();
                    double weight = br.ReadDouble();
                    bool isAvailable = br.ReadBoolean();

                    items.Add(new MenuItem(id, name, category, price, weight, isAvailable));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке: " + ex.Message);
            }
            return items;
        }

        public static void AddItem(List<MenuItem> items, MenuItem item)
        {
            if (items.Any(i => i.Id == item.Id))
                throw new ArgumentException("Пункт меню с таким ID уже существует.");

            items.Add(item);
            Save(items);
        }

        public static bool RemoveItem(List<MenuItem> items, int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return false;

            items.Remove(item);
            Save(items);
            return true;
        }

        public static IEnumerable<MenuItem> GetItemsByCategory(List<MenuItem> items, string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<MenuItem>();

            return from i in items
                   where i.Category.IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0
                   select i;
        }

        public static IEnumerable<MenuItem> GetAvailableItemsCheaperThan(List<MenuItem> items, decimal maxPrice)
        {
            return from i in items
                   where i.IsAvailable && i.Price < maxPrice
                   select i;
        }

        public static double GetAverageWeight(List<MenuItem> items)
        {
            if (items.Count == 0)
                return 0;

            return items.Average(i => i.Weight);
        }

        public static int GetAvailableCount(List<MenuItem> items)
        {
            return items.Count(i => i.IsAvailable);
        }
    }

    class Program
    {
        static List<MenuItem> _menu = new List<MenuItem>();

        static void Main()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Console.OutputEncoding = Encoding.GetEncoding(1251);
            Console.InputEncoding = Encoding.GetEncoding(1251);

            _menu = MenuRepository.Load();

            while (true)
            {
                Console.WriteLine("\n=== Меню кафе ===");
                Console.WriteLine("1. Просмотр меню");
                Console.WriteLine("2. Добавить пункт меню");
                Console.WriteLine("3. Удалить пункт меню по ID");
                Console.WriteLine("4. Запросы");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowMenu();
                        break;
                    case "2":
                        AddMenuItem();
                        break;
                    case "3":
                        RemoveMenuItem();
                        break;
                    case "4":
                        QueriesMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Повторите ввод.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            if (!_menu.Any())
            {
                Console.WriteLine("Меню пустое.");
                return;
            }
            var ordered = from item in _menu
                          orderby item.Id
                          select item;
            foreach (var item in ordered)
                Console.WriteLine(item);
        }

        static void AddMenuItem()
        {
            int id = ReadInt("Введите ID (целое число): ");
            string name = ReadString("Введите название: ");
            string category = ReadString("Введите категорию (например, Закуски, Супы, Напитки): ");
            decimal price = ReadDecimal("Введите цену: ");
            double weight = ReadDouble("Введите вес в граммах: ");
            bool isAvailable = ReadBool("Доступно? (да/нет): ");

            try
            {
                MenuRepository.AddItem(_menu, new MenuItem(id, name, category, price, weight, isAvailable));
                Console.WriteLine("Пункт меню успешно добавлен.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        static void RemoveMenuItem()
        {
            int id = ReadInt("Введите ID пункта для удаления: ");
            bool removed = MenuRepository.RemoveItem(_menu, id);
            Console.WriteLine(removed ? "Пункт меню удалён." : "Пункт меню с таким ID не найден.");
        }

        static void QueriesMenu()
        {
            Console.WriteLine("\n=== Запросы ===");
            Console.WriteLine("1. Найти пункты меню по категории");
            Console.WriteLine("2. Найти доступные пункты дешевле заданной цены");
            Console.WriteLine("3. Средний вес всех пунктов");
            Console.WriteLine("4. Количество доступных пунктов");
            Console.Write("Выберите запрос: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    QueryItemsByCategory();
                    break;
                case "2":
                    QueryAvailableItemsCheaperThan();
                    break;
                case "3":
                    QueryAverageWeight();
                    break;
                case "4":
                    QueryAvailableCount();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор запроса.");
                    break;
            }
        }

        static void QueryItemsByCategory()
        {
            Console.Write("Введите категорию для поиска: ");
            string? category = Console.ReadLine();
            var items = MenuRepository.GetItemsByCategory(_menu, category).ToList();
            if (!items.Any())
            {
                Console.WriteLine("Пункты меню по данной категории не найдены.");
                return;
            }
            foreach (var item in items)
                Console.WriteLine(item);
        }

        static void QueryAvailableItemsCheaperThan()
        {
            decimal maxPrice = ReadDecimal("Введите максимальную цену: ");
            var items = MenuRepository.GetAvailableItemsCheaperThan(_menu, maxPrice).ToList();
            if (!items.Any())
            {
                Console.WriteLine("Подходящих пунктов меню не найдено.");
                return;
            }
            foreach (var item in items)
                Console.WriteLine(item);
        }

        static void QueryAverageWeight()
        {
            double avgWeight = MenuRepository.GetAverageWeight(_menu);
            Console.WriteLine($"Средний вес всех пунктов меню: {avgWeight:F2} г");
        }

        static void QueryAvailableCount()
        {
            int availableCount = MenuRepository.GetAvailableCount(_menu);
            Console.WriteLine($"Количество доступных пунктов меню: {availableCount}");
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int i))
                {
                    if (!Validator.IsNonNegative(i))
                    {
                        Console.WriteLine("Ошибка: ID не может быть отрицательным.");
                        continue;
                    }
                    return i;
                }
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal d))
                {
                    if (!Validator.IsNonNegative(d))
                    {
                        Console.WriteLine("Ошибка: значение не может быть отрицательным.");
                        continue;
                    }
                    return d;
                }
                Console.WriteLine("Ошибка: введите число.");
            }
        }

        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double d))
                {
                    if (!Validator.IsNonNegative(d))
                    {
                        Console.WriteLine("Ошибка: значение не может быть отрицательным.");
                        continue;
                    }
                    return d;
                }
                Console.WriteLine("Ошибка: введите число.");
            }
        }

        static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                Console.WriteLine("Ошибка: ввод не может быть пустым.");
            }
        }

        static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim().ToLower();
                if (input == "да" || input == "д")
                    return true;
                if (input == "нет" || input == "н")
                    return false;
                Console.WriteLine("Пожалуйста, введите 'да' или 'нет'.");
            }
        }
    }
}
