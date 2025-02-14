using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    // Класс OrderProcessor отвечает за обработку заказов и управление запасами.
    public class OrderProcessor
    {
        // Словарь для хранения информации о запасах, где ключ - название продукта, а значение - количество на складе.
        private readonly Dictionary<string, int> _inventory;

        // Конструктор класса, который принимает словарь запасов и инициализирует поле _inventory.
        public OrderProcessor(Dictionary<string, int> inventory)
        {
            _inventory = inventory;
        }
        /// <summary>
        /// Метод для обработки заказа
        /// </summary>
        /// <param name="product">название продукта</param>
        /// <param name="quantity">количество</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public OrderResult ProcessOrder(string product, int quantity)
        {
            // Проверка на пустое или null название продукта
            if (string.IsNullOrEmpty(product))
            {
                throw new ArgumentException("Product name cannot be null or empty.");
            }

            // Проверка на корректное количество (должно быть больше нуля)
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            // Проверка наличия продукта в запасах
            if (!_inventory.ContainsKey(product))
            {
                // Если продукт не найден, возвращаем результат с ошибкой
                return new OrderResult(false, $"Product '{product}' not found in inventory.");
            }

            // Проверка на достаточность запасов для выполнения заказа
            if (_inventory[product] < quantity)
            {
                // Если запасов недостаточно, возвращаем результат с ошибкой
                return new OrderResult(false, $"Insufficient stock for '{product}'. Available: {_inventory[product]}, Requested: {quantity}");
            }

            // Уменьшаем количество продукта в запасах на запрашиваемое количество
            _inventory[product] -= quantity;

            // Возвращаем успешный результат обработки заказа
            return new OrderResult(true, $"Order for {quantity} {product}(s) processed successfully.");
        }
        /// <summary>
        /// Метод для расчета общей стоимости заказа
        /// </summary>
        /// <param name="product">название продукта</param>
        /// <param name="quantity">количество</param>
        /// <param name="price">цена за единицу</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public decimal CalculateTotalPrice(string product, int quantity, decimal price)
        {
            // Проверка на корректную цену (должна быть больше нуля)
            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            // Возвращаем общую стоимость, умножая количество на цену за единицу
            return quantity * price;
        }
        /// <summary>
        /// Метод для получения текущего состояния запасов
        /// </summary>
        /// <returns>Возвращает копию словаря запасов</returns>
        public Dictionary<string, int> GetInventory()
        {
            // Возвращаем новую копию словаря, чтобы избежать изменения оригинального словаря
            return new Dictionary<string, int>(_inventory);
        }
    }
}
