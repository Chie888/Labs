using System;

namespace CafeMenuApp
{
    /// <summary>
    /// Представляет пункт меню кафе.
    /// </summary>
    public class MenuItem
    {
        private int _id;
        private string _name = string.Empty;
        private string _category = string.Empty;
        private decimal _price;
        private double _weight;
        private bool _isAvailable;

        public MenuItem() { }

        public MenuItem(int id, string name, string category, decimal price, double weight, bool isAvailable)
        {
            _id = id;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _category = category ?? throw new ArgumentNullException(nameof(category));
            _price = price;
            _weight = weight;
            _isAvailable = isAvailable;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value ?? string.Empty; }
        public string Category { get => _category; set => _category = value ?? string.Empty; }
        public decimal Price { get => _price; set => _price = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public bool IsAvailable { get => _isAvailable; set => _isAvailable = value; }

        public override string ToString()
        {
            return $"ID: {Id}, {Name} ({Category}), Цена: {Price:F2} руб, Вес: {Weight:F2} г, В наличии: {(IsAvailable ? "Да" : "Нет")}";
        }
    }
}
