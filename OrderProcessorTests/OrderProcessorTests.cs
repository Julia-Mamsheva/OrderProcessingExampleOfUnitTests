using OrderProcessing;

namespace OrderProcessorTests;

[TestClass]
public class OrderProcessorTests
{
    
    private OrderProcessor _orderProcessor;
    private Dictionary<string, int> _inventory;

    [SetUp]
    public void SetUp()
    {
        _inventory = new Dictionary<string, int>
        {
            { "Apple", 10 },
            { "Banana", 5 },
            { "Orange", 0 }
        };
        _orderProcessor = new OrderProcessor(_inventory);
    }

    [TestMethod]
    public void ProcessOrder_ValidOrder_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Apple", 5);
        Assert.IsTrue(result.Success);
        Assert.AreEqual("Order for 5 Apple(s) processed successfully.", result.Message);
    }

    [TestMethod]
    public void ProcessOrder_ProductNotFound_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Grapes", 1);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Product 'Grapes' not found in inventory.", result.Message);
    }

    [TestMethod]
    public void ProcessOrder_InsufficientStock_ReturnsFailure()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 6);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Insufficient stock for 'Banana'. Available: 5, Requested: 6", result.Message);
    }

    [TestMethod]
    public void ProcessOrder_QuantityZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", 0));
    }

    [Test]
    public void ProcessOrder_NegativeQuantity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("Apple", -1));
    }

    [Test]
    public void ProcessOrder_NullProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder(null, 1));
    }

    [Test]
    public void ProcessOrder_EmptyProduct_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.ProcessOrder("", 1));
    }

    [Test]
    public void CalculateTotalPrice_ValidInput_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 5, 2.0m);
        Assert.AreEqual(10.0m, total);
    }

    [Test]
    public void CalculateTotalPrice_ZeroPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, 0));
    }

    [Test]
    public void CalculateTotalPrice_NegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _orderProcessor.CalculateTotalPrice("Apple", 5, -1));
    }

    [Test]
    public void GetInventory_ReturnsCopyOfInventory()
    {
        var inventory = _orderProcessor.GetInventory();
        Assert.AreEqual(3, inventory.Count);
        Assert.AreEqual(10, inventory["Apple"]);
        Assert.AreEqual(5, inventory["Banana"]);
        Assert.AreEqual(0, inventory["Orange"]);
    }

    [Test]
    public void GetInventory_InventoryIsNotModified()
    {
        var inventory = _orderProcessor.GetInventory();
        inventory["Apple"] = 100; // Modify the copy
        var originalInventory = _orderProcessor.GetInventory();
        Assert.AreEqual(10, originalInventory["Apple"]); // Original should remain unchanged
    }

    [Test]
    public void ProcessOrder_ExactStock_ReturnsSuccess()
    {
        var result = _orderProcessor.ProcessOrder("Banana", 5);
        Assert.IsTrue(result.Success);
        Assert.AreEqual("Order for 5 Banana(s) processed successfully.", result.Message);
    }

    [Test]
    public void ProcessOrder_StockDecreasesAfterOrder()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        Assert.AreEqual(7, _inventory["Apple"]);
    }

    [Test]
    public void ProcessOrder_MultipleOrders_StockUpdatesCorrectly()
    {
        _orderProcessor.ProcessOrder("Apple", 3);
        _orderProcessor.ProcessOrder("Banana", 2);
        Assert.AreEqual(7, _inventory["Apple"]);
        Assert.AreEqual(3, _inventory["Banana"]);
    }

    [Test]
    public void ProcessOrder_StockCannotGoNegative()
    {
        var result = _orderProcessor.ProcessOrder("Orange", 1);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Product 'Orange' not found in inventory.", result.Message);
    }

    [Test]
    public void CalculateTotalPrice_OneItem_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Apple", 1, 2.0m);
        Assert.AreEqual(2.0m, total);
    }

    [Test]
    public void CalculateTotalPrice_MultipleItems_ReturnsCorrectTotal()
    {
        var total = _orderProcessor.CalculateTotalPrice("Banana", 3, 1.5m);
        Assert.AreEqual(4.5m, total);
    }

    [Test]
    public void ProcessOrder_EmptyInventory_ReturnsFailure()
    {
        var emptyInventoryProcessor = new OrderProcessor(new Dictionary<string, int>());
        var result = emptyInventoryProcessor.ProcessOrder("Apple", 1);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Product 'Apple' not found in inventory.", result.Message);
    }

    [Test]
    public void ProcessOrder_InventoryIsCaseSensitive()
    {
        var result = _orderProcessor.ProcessOrder("apple", 1);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Product 'apple' not found in inventory.", result.Message);
    }
}
