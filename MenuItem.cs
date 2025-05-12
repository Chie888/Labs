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

        /// <summary>
        /// Создаёт новый пункт меню.
        /// </summary>
        /// <param name="id">Уникальный идентификатор (неотрицательный).</param>
        /// <param name="name">Название блюда.</param>
        /// <param name="category">Категория.</param>
        /// <param name="price">Цена (неотрицательная).</param>
        /// <param name="weight">Вес (неотрицательный).</param>
        /// <param name="isAvailable">Доступность.</param>
        /// <exception cref="ArgumentNullException">Если name или category равны null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Если id, price или weight отрицательные.</exception>
        public MenuItem(int id, string name, string category, decimal price, double weight, bool isAvailable)
        {
            if (!Validator.IsNonNegative(id))
                throw new ArgumentOutOfRangeException(nameof(id), "ID не может быть отрицательным.");
            if (!Validator.IsNonNegative(price))
                throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной.");
            if (!Validator.IsNonNegative(weight))
                throw new ArgumentOutOfRangeException(nameof(weight), "Вес не может быть отрицательным.");

            _id = id;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _category = category ?? throw new ArgumentNullException(nameof(category));
            _price = price;
            _weight = weight;
            _isAvailable = isAvailable;
        }

        /// <summary>
        /// Уникальный идентификатор (неотрицательный).
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                if (!Validator.IsNonNegative(value))
                    throw new ArgumentOutOfRangeException(nameof(Id), "ID не может быть отрицательным.");
                _id = value;
            }
        }

        /// <summary>
        /// Название блюда.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        /// <summary>
        /// Категория блюда.
        /// </summary>
        public string Category
        {
            get => _category;
            set => _category = value ?? string.Empty;
        }

        /// <summary>
        /// Цена (неотрицательная).
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                if (!Validator.IsNonNegative(value))
                    throw new ArgumentOutOfRangeException(nameof(Price), "Цена не может быть отрицательной.");
                _price = value;
            }
        }

        /// <summary>
        /// Вес блюда (неотрицательный).
        /// </summary>
        public double Weight
        {
            get => _weight;
            set
            {
                if (!Validator.IsNonNegative(value))
                    throw new ArgumentOutOfRangeException(nameof(Weight), "Вес не может быть отрицательным.");
                _weight = value;
            }
        }

        /// <summary>
        /// Доступность блюда.
        /// </summary>
        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = value;
        }

        /// <summary>
        /// Возвращает строковое представление пункта меню.
        /// </summary>
        /// <returns>Строка с информацией о пункте меню.</returns>
        public override string ToString()
        {
            return $"ID: {Id}, {Name} ({Category}), Цена: {Price:F2} руб, Вес: {Weight:F2} г, В наличии: {(IsAvailable ? "Да" : "Нет")}";
        }
    }
}
