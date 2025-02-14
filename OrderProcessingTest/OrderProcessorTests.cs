using OrderProcessing;

namespace OrderProcessorTests;


// Класс OrderProcessorTests содержит тесты для проверки функциональности класса OrderProcessor
public class OrderProcessorTests
{
    // Поля для хранения экземпляра OrderProcessor и словаря запасов
    private OrderProcessor _orderProcessor;
    private Dictionary<string, int> _inventory;

    // Метод, который выполняется перед каждым тестом. Здесь мы инициализируем запасы и экземпляр OrderProcessor
    [SetUp]
    public void SetUp()
    {
        // Инициализация словаря запасов с несколькими продуктами и их количеством
        _inventory = new Dictionary<string, int>
        {
            { "Apple", 10 },
            { "Banana", 5 },
            { "Orange", 0 }
        };
        // Создание экземпляра OrderProcessor с инициализированным словарем запасов
        _orderProcessor = new OrderProcessor(_inventory);
    }

    // Тест для проверки успешной обработки корректного заказа
    [Test]
    public void ProcessOrder_ValidOrder_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Apple", 5);
        Assert.IsTrue(result.Success); // Проверка, что заказ был успешным
        Assert.That(result.Message, Is.EqualTo("Order for 5 Apple(s) processed successfully.")); // Проверка сообщения
    }

    // Тест для проверки обработки заказа на продукт, который не найден в запасах
    [Test]
    public void ProcessOrder_ProductNotFound_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Grapes", 1);
        Assert.IsFalse(result.Success); // Проверка, что заказ не был успешным.
        Assert.That(result.Message, Is.EqualTo("Product 'Grapes' not found in inventory.")); // Проверка сообщения об ошибке
    }

    // Тест для проверки обработки заказа с недостаточным количеством на складе
    [Test]
    public void ProcessOrder_InsufficientStock_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 6);
        Assert.IsFalse(result.Success); // Проверка, что заказ не был успешным
        Assert.That(result.Message, Is.EqualTo("Insufficient stock for 'Banana'. Available: 5, Requested: 6")); // Проверка сообщения об ошибке
    }

    // Тест для проверки обработки заказа с нулевым количеством
    [Test]
    public void ProcessOrder_QuantityZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", 0)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки обработки заказа с отрицательным количеством
    [Test]
    public void ProcessOrder_NegativeQuantity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", -1)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки обработки заказа с null в качестве названия продукта
    [Test]
    public void ProcessOrder_NullProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder(null, 1)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки обработки заказа с пустым названием продукта
    [Test]
    public void ProcessOrder_EmptyProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("", 1)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки корректного расчета общей стоимости заказа
    [Test]
    public void CalculateTotalPrice_ValidInput_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 5, 2.0m);
        Assert.That(total, Is.EqualTo(10.0m)); // Проверка, что общая стоимость рассчитана правильно
    }

    // Тест для проверки обработки расчета стоимости с нулевой ценой
    [Test]
    public void CalculateTotalPrice_ZeroPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, 0)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки обработки расчета стоимости с отрицательной ценой
    [Test]
    public void CalculateTotalPrice_NegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, -1)); // Проверка, что выбрасывается исключение
    }

    // Тест для проверки, что метод GetInventory возвращает копию запасов
    [Test]
    public void GetInventory_ReturnsCopyOfInventory()
    {
        var inventory = _orderProcessor.GetInventory();
        Assert.That(inventory.Count, Is.EqualTo(3)); // Проверка, что количество продуктов в копии соответствует ожиданиям
        Assert.That(inventory["Apple"], Is.EqualTo(10)); // Проверка, что количество яблок в копии соответствует ожиданиям
        Assert.That(inventory["Banana"], Is.EqualTo(5)); // Проверка, что количество бананов в копии соответствует ожиданиям
        Assert.That(inventory["Orange"], Is.EqualTo(0)); // Проверка, что количество апельсинов в копии соответствует ожиданиям
    }

    // Тест для проверки, что оригинальный словарь запасов не изменяется при изменении копии
    [Test]
    public void GetInventory_InventoryIsNotModified()
    {
        var inventory = _orderProcessor.GetInventory();
        inventory["Apple"] = 100; // Изменяем копию
        var originalInventory = _orderProcessor.GetInventory();
        Assert.That(originalInventory["Apple"], Is.EqualTo(10)); // Проверка, что оригинальный словарь остался неизменным
    }

    // Тест для проверки успешной обработки заказа, когда запрашивается количество, равное имеющимся запасам
    [Test]
    public void ProcessOrder_ExactStock_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 5);
        Assert.IsTrue(result.Success); // Проверка, что заказ был успешным
        Assert.That(result.Message, Is.EqualTo("Order for 5 Banana(s) processed successfully.")); // Проверка сообщения
    }

    // Тест для проверки, что запасы уменьшаются после успешного заказа
    [Test]
    public void ProcessOrder_StockDecreasesAfterOrder()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        Assert.That(_inventory["Apple"], Is.EqualTo(7)); // Проверка, что количество яблок уменьшилось на 3
    }

    // Тест для проверки, что запасы обновляются корректно после нескольких заказов
    [Test]
    public void ProcessOrder_MultipleOrders_StockUpdatesCorrectly()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        _orderProcessor.ProcessOrder("Banana", 2);
        Assert.That(_inventory["Apple"], Is.EqualTo(7)); // Проверка, что количество яблок уменьшилось на 3
        Assert.That(_inventory["Banana"], Is.EqualTo(3)); // Проверка, что количество бананов уменьшилось на 2
    }

    // Тест для проверки, что запасы не могут стать отрицательными
    [Test]
    public void ProcessOrder_StockCannotGoNegative()
    {
        var result = _orderProcessor.ProcessOrder("Orange", 1);
        Assert.IsFalse(result.Success); // Проверка, что заказ не был успешным.
        Assert.That(result.Message, Is.EqualTo("Insufficient stock for 'Orange'. Available: 0, Requested: 1")); // Проверка сообщения об ошибке
    }

    // Тест для проверки корректного расчета стоимости для одного товара
    [Test]
    public void CalculateTotalPrice_OneItem_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 1, 2.0m);
        Assert.That(total, Is.EqualTo(2.0m)); // Проверка, что общая стоимость рассчитана правильно
    }

    // Тест для проверки корректного расчета стоимости для нескольких товаров
    [Test]
    public void CalculateTotalPrice_MultipleItems_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Banana", 3, 1.5m);
        Assert.That(total, Is.EqualTo(4.5m)); // Проверка, что общая стоимость рассчитана правильно
    }

    // Тест для проверки обработки заказа, когда запасы пусты
    [Test]
    public void ProcessOrder_EmptyInventory_ReturnsFailure()
    {
        var emptyInventoryProcessor = new OrderProcessor(new Dictionary<string, int>()); // Создание OrderProcessor с пустым словарем
        var result = emptyInventoryProcessor.ProcessOrder("Apple", 1);
        Assert.IsFalse(result.Success); // Проверка, что заказ не был успешным
        Assert.That(result.Message, Is.EqualTo("Product 'Apple' not found in inventory.")); // Проверка сообщения об ошибке
    }

    // Тест для проверки, что поиск продукта в запасах чувствителен к регистру
    [Test]
    public void ProcessOrder_InventoryIsCaseSensitive()
    {
        var result = _orderProcessor.ProcessOrder("apple", 1); // Запрос с маленькой буквы
        Assert.IsFalse(result.Success); // Проверка, что заказ не был успешным
        Assert.That(result.Message, Is.EqualTo("Product 'apple' not found in inventory.")); // Проверка сообщения об ошибке
    }
}