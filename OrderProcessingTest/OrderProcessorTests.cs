using OrderProcessing;

namespace OrderProcessorTests;


// ����� OrderProcessorTests �������� ����� ��� �������� ���������������� ������ OrderProcessor
public class OrderProcessorTests
{
    // ���� ��� �������� ���������� OrderProcessor � ������� �������
    private OrderProcessor _orderProcessor;
    private Dictionary<string, int> _inventory;

    // �����, ������� ����������� ����� ������ ������. ����� �� �������������� ������ � ��������� OrderProcessor
    [SetUp]
    public void SetUp()
    {
        // ������������� ������� ������� � ����������� ���������� � �� �����������
        _inventory = new Dictionary<string, int>
        {
            { "Apple", 10 },
            { "Banana", 5 },
            { "Orange", 0 }
        };
        // �������� ���������� OrderProcessor � ������������������ �������� �������
        _orderProcessor = new OrderProcessor(_inventory);
    }

    // ���� ��� �������� �������� ��������� ����������� ������
    [Test]
    public void ProcessOrder_ValidOrder_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Apple", 5);
        Assert.IsTrue(result.Success); // ��������, ��� ����� ��� ��������
        Assert.That(result.Message, Is.EqualTo("Order for 5 Apple(s) processed successfully.")); // �������� ���������
    }

    // ���� ��� �������� ��������� ������ �� �������, ������� �� ������ � �������
    [Test]
    public void ProcessOrder_ProductNotFound_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Grapes", 1);
        Assert.IsFalse(result.Success); // ��������, ��� ����� �� ��� ��������.
        Assert.That(result.Message, Is.EqualTo("Product 'Grapes' not found in inventory.")); // �������� ��������� �� ������
    }

    // ���� ��� �������� ��������� ������ � ������������� ����������� �� ������
    [Test]
    public void ProcessOrder_InsufficientStock_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 6);
        Assert.IsFalse(result.Success); // ��������, ��� ����� �� ��� ��������
        Assert.That(result.Message, Is.EqualTo("Insufficient stock for 'Banana'. Available: 5, Requested: 6")); // �������� ��������� �� ������
    }

    // ���� ��� �������� ��������� ������ � ������� �����������
    [Test]
    public void ProcessOrder_QuantityZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", 0)); // ��������, ��� ������������� ����������
    }

    // ���� ��� �������� ��������� ������ � ������������� �����������
    [Test]
    public void ProcessOrder_NegativeQuantity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", -1)); // ��������, ��� ������������� ����������
    }

    // ���� ��� �������� ��������� ������ � null � �������� �������� ��������
    [Test]
    public void ProcessOrder_NullProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder(null, 1)); // ��������, ��� ������������� ����������
    }

    // ���� ��� �������� ��������� ������ � ������ ��������� ��������
    [Test]
    public void ProcessOrder_EmptyProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("", 1)); // ��������, ��� ������������� ����������
    }

    // ���� ��� �������� ����������� ������� ����� ��������� ������
    [Test]
    public void CalculateTotalPrice_ValidInput_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 5, 2.0m);
        Assert.That(total, Is.EqualTo(10.0m)); // ��������, ��� ����� ��������� ���������� ���������
    }

    // ���� ��� �������� ��������� ������� ��������� � ������� �����
    [Test]
    public void CalculateTotalPrice_ZeroPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, 0)); // ��������, ��� ������������� ����������
    }

    // ���� ��� �������� ��������� ������� ��������� � ������������� �����
    [Test]
    public void CalculateTotalPrice_NegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, -1)); // ��������, ��� ������������� ����������
    }

    // ���� ��� ��������, ��� ����� GetInventory ���������� ����� �������
    [Test]
    public void GetInventory_ReturnsCopyOfInventory()
    {
        var inventory = _orderProcessor.GetInventory();
        Assert.That(inventory.Count, Is.EqualTo(3)); // ��������, ��� ���������� ��������� � ����� ������������� ���������
        Assert.That(inventory["Apple"], Is.EqualTo(10)); // ��������, ��� ���������� ����� � ����� ������������� ���������
        Assert.That(inventory["Banana"], Is.EqualTo(5)); // ��������, ��� ���������� ������� � ����� ������������� ���������
        Assert.That(inventory["Orange"], Is.EqualTo(0)); // ��������, ��� ���������� ���������� � ����� ������������� ���������
    }

    // ���� ��� ��������, ��� ������������ ������� ������� �� ���������� ��� ��������� �����
    [Test]
    public void GetInventory_InventoryIsNotModified()
    {
        var inventory = _orderProcessor.GetInventory();
        inventory["Apple"] = 100; // �������� �����
        var originalInventory = _orderProcessor.GetInventory();
        Assert.That(originalInventory["Apple"], Is.EqualTo(10)); // ��������, ��� ������������ ������� ������� ����������
    }

    // ���� ��� �������� �������� ��������� ������, ����� ������������� ����������, ������ ��������� �������
    [Test]
    public void ProcessOrder_ExactStock_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 5);
        Assert.IsTrue(result.Success); // ��������, ��� ����� ��� ��������
        Assert.That(result.Message, Is.EqualTo("Order for 5 Banana(s) processed successfully.")); // �������� ���������
    }

    // ���� ��� ��������, ��� ������ ����������� ����� ��������� ������
    [Test]
    public void ProcessOrder_StockDecreasesAfterOrder()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        Assert.That(_inventory["Apple"], Is.EqualTo(7)); // ��������, ��� ���������� ����� ����������� �� 3
    }

    // ���� ��� ��������, ��� ������ ����������� ��������� ����� ���������� �������
    [Test]
    public void ProcessOrder_MultipleOrders_StockUpdatesCorrectly()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        _orderProcessor.ProcessOrder("Banana", 2);
        Assert.That(_inventory["Apple"], Is.EqualTo(7)); // ��������, ��� ���������� ����� ����������� �� 3
        Assert.That(_inventory["Banana"], Is.EqualTo(3)); // ��������, ��� ���������� ������� ����������� �� 2
    }

    // ���� ��� ��������, ��� ������ �� ����� ����� ��������������
    [Test]
    public void ProcessOrder_StockCannotGoNegative()
    {
        var result = _orderProcessor.ProcessOrder("Orange", 1);
        Assert.IsFalse(result.Success); // ��������, ��� ����� �� ��� ��������.
        Assert.That(result.Message, Is.EqualTo("Insufficient stock for 'Orange'. Available: 0, Requested: 1")); // �������� ��������� �� ������
    }

    // ���� ��� �������� ����������� ������� ��������� ��� ������ ������
    [Test]
    public void CalculateTotalPrice_OneItem_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 1, 2.0m);
        Assert.That(total, Is.EqualTo(2.0m)); // ��������, ��� ����� ��������� ���������� ���������
    }

    // ���� ��� �������� ����������� ������� ��������� ��� ���������� �������
    [Test]
    public void CalculateTotalPrice_MultipleItems_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Banana", 3, 1.5m);
        Assert.That(total, Is.EqualTo(4.5m)); // ��������, ��� ����� ��������� ���������� ���������
    }

    // ���� ��� �������� ��������� ������, ����� ������ �����
    [Test]
    public void ProcessOrder_EmptyInventory_ReturnsFailure()
    {
        var emptyInventoryProcessor = new OrderProcessor(new Dictionary<string, int>()); // �������� OrderProcessor � ������ ��������
        var result = emptyInventoryProcessor.ProcessOrder("Apple", 1);
        Assert.IsFalse(result.Success); // ��������, ��� ����� �� ��� ��������
        Assert.That(result.Message, Is.EqualTo("Product 'Apple' not found in inventory.")); // �������� ��������� �� ������
    }

    // ���� ��� ��������, ��� ����� �������� � ������� ������������ � ��������
    [Test]
    public void ProcessOrder_InventoryIsCaseSensitive()
    {
        var result = _orderProcessor.ProcessOrder("apple", 1); // ������ � ��������� �����
        Assert.IsFalse(result.Success); // ��������, ��� ����� �� ��� ��������
        Assert.That(result.Message, Is.EqualTo("Product 'apple' not found in inventory.")); // �������� ��������� �� ������
    }
}